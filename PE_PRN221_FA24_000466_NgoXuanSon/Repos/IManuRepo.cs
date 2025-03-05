using BOs;

namespace Repos
{
	public interface IManuRepo
	{
		Task<List<Manufacturer>> GetManufacturerList();
	}
}
