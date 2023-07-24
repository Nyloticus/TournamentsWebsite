using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Infrastructures.MediatR
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        //private readonly ILog _log;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators //,ILog log
        )
        {
            _validators = validators;
            //_log = log;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var context = new ValidationContext<TRequest>(request);
            var name = typeof(TRequest).Name;

            var failures = _validators
              .Select(v => (v.Validate(context)))
              .SelectMany(result => result.Errors)
              .ToList();

            if (failures.Count != 0)
            {
                var message = $"HelpApp Long Running Request: {name} \n";
                foreach (var failure in failures)
                {
                    message += $" {failure.ErrorMessage} \n";
                }

                var errors = failures.Select(s => new ErrorResult(s.PropertyName, s.ErrorMessage)).ToArray();
                //var errorMessage = string.Join(",", failures.Select(failure => failure.ErrorMessage));

                throw new ApiException(ApiExeptionType.ValidationError, errors);
            }

            return await next();
        }
    }
}