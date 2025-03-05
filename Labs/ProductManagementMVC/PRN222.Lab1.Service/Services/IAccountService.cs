using PRN222.Lab1.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Lab1.Service.Services
{
	public interface IAccountService
	{
		AccountMember GetAccountById(string id);

		AccountMember GetAccountByEmail(string email);
	}
}
