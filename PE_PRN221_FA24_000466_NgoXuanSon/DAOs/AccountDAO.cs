using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
	public class AccountDAO
	{
		private readonly Fall24PharmaceuticalDbContext _context;

		private static AccountDAO instance = null;

		private AccountDAO()
		{
			_context = new Fall24PharmaceuticalDbContext();
		}

		public static AccountDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AccountDAO();
				}
				return instance;
			}
			private set => instance = value;
		}

		public async Task<StoreAccount> Login(string email, string password)
		{
			return await _context.StoreAccounts.FirstOrDefaultAsync(acc => acc.EmailAddress == email && acc.StoreAccountPassword == password);
		}
	}
}
