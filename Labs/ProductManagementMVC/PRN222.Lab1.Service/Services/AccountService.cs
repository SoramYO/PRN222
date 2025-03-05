using PRN222.Lab1.Repository.Models;
using PRN222.Lab1.Repository.Repositories;
using PRN222.Lab1.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Service.Services
{
	public class AccountService : IAccountService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AccountService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public AccountMember GetAccountById(string id)
		{
			return _unitOfWork.AccountMembers.GetById(id);
		}

		public AccountMember GetAccountByEmail(string email)
		{
			return _unitOfWork.AccountMembers.GetAll().FirstOrDefault(x => x.EmailAddress == email);
		}
	}
}
