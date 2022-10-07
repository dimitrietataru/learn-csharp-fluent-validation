namespace Learn.CSharp.XFluentValidation.Application.Providers;

public sealed class UtcDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
