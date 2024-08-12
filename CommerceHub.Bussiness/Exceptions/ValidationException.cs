using CommerceHub.Base.Exception;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Bussiness.Exceptions
{
    public class ValidationException : CustomException
    {
        public List<ValidationFailure> Errors { get; }
        public ValidationException(List<ValidationFailure> failures) : base("Validation errors occurred.", StatusCodes.Status400BadRequest) 
        {
            Errors = failures;
        }

        public override string ToString()
        {
            return string.Join("; ", Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
        }
    }
}
