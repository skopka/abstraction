namespace Skopka.Abstraction.OperationResult;

public static class ErrorCodes
{
    public const string Unknown = "skopka.common.unknown";
    public const string ValidationRequired = "skopka.validation.required";
    public const string ValidationFormat = "skopka.validation.format";
    public const string ValidationPredicateFailed = "skopka.validation.predicate_failed";
    public const string NotFound = "skopka.common.not_found";
    public const string Conflict = "skopka.common.conflict";
    public const string Unauthorized = "skopka.auth.unauthorized";
    public const string Forbidden = "skopka.auth.forbidden";
}