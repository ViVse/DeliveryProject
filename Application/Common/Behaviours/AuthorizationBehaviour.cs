using Application.Common.Interfaces;
using Application.Common.Security;
using MediatR;
using System.Reflection;

namespace Application.Common.Behaviours
{
    public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ICurrentUserService _currentUserService;

        public AuthorizationBehaviour(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if(authorizeAttributes.Any())
            {
                if (_currentUserService.UserId == null)
                    throw new UnauthorizedAccessException();
            }

            return await next();
        }
    }
}
