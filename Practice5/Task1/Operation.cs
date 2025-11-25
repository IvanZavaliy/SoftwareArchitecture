using System;


namespace Practice5
{
    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton
    {
        public Operation()
        {
            OperationId = Guid.NewGuid().ToString()[^4..]; // Беремо останні 4 символи для зручності
        }

        public string OperationId { get; }
    }
}