namespace Practice5
{
    public class MyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        private readonly IOperationSingleton _singletonOperation;
        private readonly IOperationTransient _transientOperation;

        public MyMiddleware(RequestDelegate next, ILogger<MyMiddleware> logger,
            IOperationSingleton singletonOperation, IOperationTransient transientOperation)
        {
            _next = next;
            _logger = logger;
            _singletonOperation = singletonOperation;
            _transientOperation = transientOperation;
        }

        public async Task InvokeAsync(HttpContext context, IOperationScoped scopedOperation)
        {
            _logger.LogInformation("Middleware Transient: " + _transientOperation.OperationId);
            _logger.LogInformation("Middleware Scoped:    " + scopedOperation.OperationId);
            _logger.LogInformation("Middleware Singleton: " + _singletonOperation.OperationId);

            await _next(context);
        }
    }
}