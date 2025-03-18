using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository;
using Repository.Models;

namespace Service
{
	public class StoreAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StoreAccountService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<StoreAccount> LoginAsync(string email, string password)
		{
			var accounts = await _unitOfWork.StoreAccountRepo.Get();

			return accounts.FirstOrDefault(x => x.EmailAddress == email && x.StoreAccountPassword == password);
		}
	}
}
