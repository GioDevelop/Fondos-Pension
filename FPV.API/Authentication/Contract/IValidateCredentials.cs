namespace FPV.API.Authentication.Contract
{
    public interface IValidateCredentials
    {
        bool ValidateCredential(string? username, string? password);
    }
}
