using PRN222.LAB2.Repository.Models;
using PRN222.LAB2.Repository.UnitOfWork;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PRN222.LAB2.Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AccountService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		}

		public async Task<AccountMember> GetAccountByEmailAsync(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
				throw new ArgumentException("Email cannot be null or empty", nameof(email));

			var accounts = await _unitOfWork.AccountMembers.Get();

			return accounts.FirstOrDefault(x => x.EmailAddress == email);
		}
	}
}