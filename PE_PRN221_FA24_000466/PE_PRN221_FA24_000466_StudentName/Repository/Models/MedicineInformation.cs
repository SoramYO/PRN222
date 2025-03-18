using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Repository.Models;

public partial class MedicineInformation
{
    public string MedicineId { get; set; } = null!;

    public string MedicineName { get; set; } = null!;
	[ActiveIngredientsValidation]
	[Display(Name = "Active Ingredients")]
	public string ActiveIngredients { get; set; } = null!;

    public string? ExpirationDate { get; set; }

    public string DosageForm { get; set; } = null!;

    public string WarningsAndPrecautions { get; set; } = null!;

    public string? ManufacturerId { get; set; }

    public virtual Manufacturer? Manufacturer { get; set; }
}
