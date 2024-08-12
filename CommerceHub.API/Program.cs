using CommerceHub.API.Middleware;
using CommerceHub.Base;
using CommerceHub.Base.Auth.Token;
using CommerceHub.Bussiness.Auth.CreateAuthorizationToken;
using CommerceHub.Bussiness.Auth.Token;
using CommerceHub.Bussiness.Behavior;
using CommerceHub.Bussiness.CategoryFeatures.Command.CreateCategory;
using CommerceHub.Bussiness.CategoryFeatures.Mapper;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Bussiness.OrderFeatures.Payment;
using CommerceHub.Data.Context;
using CommerceHub.Data.Seeder;
using CommerceHub.Data.UnitOfWork;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Serilog;
using CommerceHub.Data.Repositories.DapperRepository;
using CommerceHub.Data.Repositories.ReportRepository;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Npgsql;
using CommerceHub.Bussiness.Notification;
using MassTransit;
using MailKit.Net;
using MailKit.Net.Smtp;
using CommerceHub.Bussiness.Messaging.Consumers;
using CommerceHub.Bussiness.Messaging;
using CommerceHub.Bussiness.Cache;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<CommerceHubDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.AddSingleton<JwtConfig>(jwtConfig);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = true;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
        ValidAudience = jwtConfig.Audience,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(2)
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commerce Hub", Version = "v1.0" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Commerce Hub for Papara Final Case",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Redis";
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderPlacedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rabbitmq://localhost", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("order-placed-queue", e =>
        {
            e.ConfigureConsumer<OrderPlacedConsumer>(context);
        });
    });
});

builder.Services.AddMassTransitHostedService();

// SMTP konfigürasyonu
builder.Services.AddScoped<SmtpClient>(sp =>
{
    var client = new SmtpClient();
    client.Connect("smtp.gmail.com", 587, false);
    client.Authenticate("odevtest802@gmail.com", "zylx cnzf qukd oysb");
    return client;
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddScoped<INotificationSender, EmailSender>();
builder.Services.AddScoped<IMessagePublisher,  MessagePublisher>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPaymentService, FakePaymentService>();

builder.Services.AddScoped<IDapperRepository, DapperRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IOrderHandler, FetchProductsHandler>();
builder.Services.AddScoped<IOrderHandler, CalculateTotalAmountHandler>();
builder.Services.AddScoped<IOrderHandler, CreateOrderDetailsHandler>();
builder.Services.AddScoped<IOrderHandler, ApplyCouponHandler>();
builder.Services.AddScoped<IOrderHandler, ApplyPointsHandler>();
builder.Services.AddScoped<IOrderHandler, ProcessPaymentHandler>();
builder.Services.AddScoped<IOrderHandler, EarnPointsHandler>();





builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ISessionContext>(provider =>
{
    var context = provider.GetService<IHttpContextAccessor>();
    var sessionContext = new SessionContext();
    sessionContext.Session = JwtManager.GetSession(context.HttpContext);
    sessionContext.HttpContext = context.HttpContext;
    return sessionContext;
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(CreateAuthorizationTokenCommand).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateCategoryValidator).Assembly);
builder.Services.AddAutoMapper(typeof(CategoryProfile).Assembly);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DbInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
