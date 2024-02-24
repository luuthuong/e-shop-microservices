using Core.Results;

namespace ProductSyncService.Infrastructure;

public class AuthorizeResponse
{
    public static Error NotAllowAccess = new Error("NotAllowAccess", "Cannot access this request");
}