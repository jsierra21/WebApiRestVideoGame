using Core.Entities;
using Core.Interfaces;
using Core.ModelResponse;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       // public async Task<ResponseAuth> LoginAccountService(AccountLogin user) => await _unitOfWork.AccountRepository.LoginAccountService(user);

    }
}
