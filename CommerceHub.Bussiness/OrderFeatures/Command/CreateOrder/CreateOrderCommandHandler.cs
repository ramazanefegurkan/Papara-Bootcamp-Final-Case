
using CommerceHub.Schema;
using MediatR;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerceHub.Data.Domain;
using CommerceHub.Data.UnitOfWork;
using CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder.Handlers;
using CommerceHub.Base;
using CommerceHub.Bussiness.Exceptions;
using Microsoft.AspNetCore.Http;
using static System.Collections.Specialized.BitVector32;
using CommerceHub.Bussiness.Messaging;
using CommerceHub.Bussiness.Messaging.Events;

namespace CommerceHub.Bussiness.OrderFeatures.Command.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISessionContext _sessionContext;
        private readonly IMessagePublisher _messagePublisher;
        private readonly List<IOrderHandler> _handlers;

        public CreateOrderCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ISessionContext sessionContext,
            IMessagePublisher messagePublisher,
            IEnumerable<IOrderHandler> handlers)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sessionContext = sessionContext;
            _messagePublisher = messagePublisher;
            _handlers = handlers.OrderBy(handler => handler.Order).ToList();
        }

        public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserId = _sessionContext.Session.UserId,
                OrderNumber = await GenerateOrderNumberAsync(),
            };

            var context = new OrderContext
            {
                OrderRequest = command.Request,
                Order = order,
                RemainingAmount = order.TotalAmount,
            };

            foreach (var handler in _handlers)
            {
                await handler.Handle(context);
            }

            await _unitOfWork.OrderRepository.Insert(order);
            await _unitOfWork.CompleteWithTransaction();

            _ = _messagePublisher.PublishOrderPlacedEventAsync(new OrderPlacedEvent
            {
                OrderNumber = order.OrderNumber,
                UserEmail = _sessionContext.Session.Email,
                OrderDate = order.InsertDate,
                UserFullName = _sessionContext.Session.FullName
            });

            var orderResponse = _mapper.Map<Order, OrderResponse>(order);

            return ApiResponse<OrderResponse>.SuccessResult(orderResponse, "Order created successfully.");
        }
        private async Task<string> GenerateOrderNumberAsync()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            string orderNumber;
            bool isUnique;

            do
            {
                var randomString = new string(Enumerable.Repeat(chars, 6)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                orderNumber = $"SIP{randomString}";

                var existingOrder = await _unitOfWork.OrderRepository
                    .FirstOrDefault(o => o.OrderNumber == orderNumber);
                isUnique = existingOrder == null;
            } while (!isUnique);

            return orderNumber;
        }
    }
}
