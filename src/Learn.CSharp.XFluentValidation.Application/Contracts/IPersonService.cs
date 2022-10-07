namespace Learn.CSharp.XFluentValidation.Application.Contracts;

public interface IPersonService
{
    public Task<bool> ExistsAsync(int id);
}
