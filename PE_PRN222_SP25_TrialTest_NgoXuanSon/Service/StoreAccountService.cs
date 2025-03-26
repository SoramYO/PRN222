using Repository;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
	public class StoreAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		public StoreAccountService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public StoreAccount Login(string email, string password)
		{

			var acc = _unitOfWork.StoreAccountRepo.Get();
			return acc.Result.FirstOrDefault(x => x.EmailAddress == email && x.StoreAccountPassword == password);

		}

	}
}
