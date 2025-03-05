using System.ComponentModel.DataAnnotations;

namespace BOs;

public partial class MedicineInformation
{
	[Required(ErrorMessage = "MedicineId is required")]
	public string MedicineId { get; set; } = null!;


	[Required(ErrorMessage = "MedicineName is required")]
	public string MedicineName { get; set; } = null!;

	[RegularExpression(@"^(?:[A-Z0-9][a-zA-Z0-9]*\s?)+$", ErrorMessage = "Each word must begin with a capital letter or a number, and no special characters are allowed.")]
	[MinLength(11, ErrorMessage = "ActiveIngredients must be greater than 10 characters.")]
	public string ActiveIngredients { get; set; } = null!;

	[Required(ErrorMessage = "ExpirationDate is required")]
	public string? ExpirationDate { get; set; }

	[Required(ErrorMessage = "DosageForm is required")]
	public string DosageForm { get; set; } = null!;

	[Required(ErrorMessage = "WarningsAndPrecautions is required")]
	public string WarningsAndPrecautions { get; set; } = null!;

	[Required(ErrorMessage = "ManufacturerId is required")]
	public string? ManufacturerId { get; set; }

	public virtual Manufacturer? Manufacturer { get; set; }
}
