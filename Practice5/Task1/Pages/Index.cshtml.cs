using Microsoft.AspNetCore.Mvc.RazorPages;
using Practice5;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IOperationTransient _transientOperation;
    private readonly IOperationScoped _scopedOperation;
    private readonly IOperationSingleton _singletonOperation;

    public IndexModel(ILogger<IndexModel> logger, 
        IOperationTransient transientOperation,
        IOperationScoped scopedOperation,
        IOperationSingleton singletonOperation)
    {
        _logger = logger;
        _transientOperation = transientOperation;
        _scopedOperation = scopedOperation;
        _singletonOperation = singletonOperation;
    }

    public void OnGet()
    {
        _logger.LogInformation("PageModel Transient:  " + _transientOperation.OperationId);
        _logger.LogInformation("PageModel Scoped:     " + _scopedOperation.OperationId);
        _logger.LogInformation("PageModel Singleton:  " + _singletonOperation.OperationId);
    }
}