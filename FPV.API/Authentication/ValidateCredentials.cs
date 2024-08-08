using FPV.API.Authentication.Contract;

namespace FPV.API.Authentication
{
    public class ValidateCredentials : IValidateCredentials
    {
        public readonly IConfiguration configuration;

        public ValidateCredentials(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public bool ValidateCredential(string? username, string? password)
        {
            return username.Equals(this.configuration.GetSection("AppSettings:FPVApi:UserName").Value) && password.Equals(this.configuration.GetSection("AppSettings:FPVApi:Password").Value);
        }
    }
}
