using Core.Entities;
using Core.ModelResponse;

namespace Core.Interfaces
{
    public interface IAccountRepository
    {
        //interface account de logueo
        Task<ResponseAuth> LoginAccountService(AccountLogin user);

    }
}
