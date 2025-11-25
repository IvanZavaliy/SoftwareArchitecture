using System;

namespace Practice5
{
    // Базовий інтерфейс
    public interface IOperation
    {
        string OperationId { get; }
    }

    // Інтерфейси-маркери для різних типів реєстрації
    public interface IOperationTransient : IOperation { }
    public interface IOperationScoped : IOperation { }
    public interface IOperationSingleton : IOperation { }
}