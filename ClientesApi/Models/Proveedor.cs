using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models;

[Table("proveedores")]
public class Proveedor
{
    [Column("id")] [Key] public int Id { get; set; }

    [Column("representante"), MaxLength(255)]
    [Required]
    public string Representante { get; set; } = "";

    [Column("nombre"), MaxLength(255)]
    [Required]
    public string Nombre { get; set; } = "";

    [Column("correo"), MaxLength(255)]
    [Required]
    public string Correo { get; set; } = "";

    [Column("telefono"), MaxLength(15)] public string Telefono { get; set; } = "";
}