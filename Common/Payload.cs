namespace GraphQL.Common;

public abstract class Payload
{
    public Payload(IReadOnlyList<UserError>? errors = null)
    {
        Errors = errors;
    }

    public IReadOnlyList<UserError>? Errors { get; }
}
