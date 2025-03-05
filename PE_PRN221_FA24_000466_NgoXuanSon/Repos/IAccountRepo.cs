using BOs;

namespace Repos
{
	public interface IAccountRepo
	{
		Task<StoreAccount> Login(string email, string password);
	}
}
