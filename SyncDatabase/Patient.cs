// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Patient
{
    public int Id { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 15 characters.")]
    public string FirstName { get; set; } = null!;

    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 15 characters.")]
    public string LastName { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Region must not exceed 50 characters.")]
    public string Region { get; set; } = string.Empty;

    public int Gender { get; set; }

    public DateTime DateOfBirth { get; set; }

    public bool IsDeleted { get; set; } = false;


}
