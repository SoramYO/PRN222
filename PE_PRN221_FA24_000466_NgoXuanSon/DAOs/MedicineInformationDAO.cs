using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
	public class MedicineInformationDAO
	{
		private readonly Fall24PharmaceuticalDbContext _context;

		private static MedicineInformationDAO instance = null;

		private MedicineInformationDAO()
		{
			_context = new Fall24PharmaceuticalDbContext();
		}

		public static MedicineInformationDAO Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new MedicineInformationDAO();
				}
				return instance;
			}
			private set => instance = value;
		}

		public async Task<List<MedicineInformation>> GetList()
		{
			return await _context.MedicineInformations.Include(x => x.Manufacturer).ToListAsync();
		}

		public class PaintingResponse
		{
			public List<MedicineInformation> MedicineInformation { get; set; }
			public int TotalPages { get; set; }
			public int PageIndex { get; set; }
		}

		public async Task<PaintingResponse> GetList(string searchTerm, int pageIndex, int pagesize)
		{
			var query = _context.MedicineInformations.Include(x => x.Manufacturer).AsQueryable();
			if (!string.IsNullOrEmpty(searchTerm))
			{
				query = query.Where(x => x.ActiveIngredients.ToLower().Contains(searchTerm.ToLower())
				|| x.ExpirationDate.ToLower().Contains(searchTerm.ToLower())
				|| x.WarningsAndPrecautions.ToLower().Contains(searchTerm.ToLower()));
			}
			int count = await query.CountAsync();
			int totalPages = (int)Math.Ceiling(count / (double)pagesize);
			query = query.Skip((pageIndex - 1) * pagesize).Take(pagesize);

			return new PaintingResponse
			{
				MedicineInformation = await query.ToListAsync(),
				TotalPages = totalPages,
				PageIndex = pageIndex
			};
		}
		public async Task<MedicineInformation> GetById(string id)
		{
			return await _context.MedicineInformations.Include(x => x.Manufacturer).FirstOrDefaultAsync(x => x.MedicineId == id);
		}

		public async Task Insert(MedicineInformation medicineInformation)
		{
			try
			{
				var existing = await GetById(medicineInformation.MedicineId);
				if (existing != null)
				{
					throw new Exception("Medicine existing");
				}
				_context.MedicineInformations.Add(medicineInformation);
				await _context.SaveChangesAsync();

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);

			}
		}

		public async Task Update(MedicineInformation medicineInformation)
		{
			try
			{
				var existing = await GetById(medicineInformation.MedicineId);
				if (existing == null)
				{
					throw new Exception("Medicine not existing");
				}
				existing.MedicineName = medicineInformation.MedicineName;
				existing.ActiveIngredients = medicineInformation.ActiveIngredients;
				existing.ExpirationDate = medicineInformation.ExpirationDate;
				existing.DosageForm = medicineInformation.DosageForm;
				existing.WarningsAndPrecautions = medicineInformation.WarningsAndPrecautions;
				existing.ManufacturerId = medicineInformation.ManufacturerId;

				var manu = await _context.Manufacturers.FirstOrDefaultAsync(x => x.ManufacturerId == medicineInformation.ManufacturerId);
				if (manu == null)
				{
					throw new Exception("Manufacturer not existing");
				}
				existing.ManufacturerId = medicineInformation.ManufacturerId;


				_context.MedicineInformations.Update(existing);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task Delete(string id)
		{
			try
			{
				var existing = await GetById(id);
				if (existing == null)
				{
					throw new Exception("Medicine not existing");
				}
				_context.MedicineInformations.Remove(existing);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<List<Manufacturer>> GetManufacturer()
		{
			return await _context.Manufacturers.ToListAsync();
		}
	}
}
