namespace Practice5.Services
{
    public class MyTransientService
    {
        public MyTransientService(IOperationTransient transientOperation) { }
    }

    public class MyScopedService
    {
        public MyScopedService(IOperationScoped scopedOperation) { }
    }

    public class MySingletonService
    {
        public MySingletonService(IOperationSingleton singletonOperation) { }
    }
}