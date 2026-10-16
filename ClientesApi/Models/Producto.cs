using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models;


/**
 * create table productos(
	id int identity(100,1) primary key,
	nombre nvarchar(100)  not null,
	descripcion nvarchar(500) not null,
	precio decimal(7,2) not null,
	garantia tinyint not null,
	numserie nvarchar(40) null
)
 */
[Table("productos")]
public class Producto
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required,StringLength(100)]
    //[Column("nombre")]
    public string Nombre { get; set; }
    
    [Required,StringLength(500)]
    [Column("Descripcion")]
    public string Descripcion { get; set; }
    
    [Required]
    [Column("precio")]
    public decimal Precio { get; set; }
    
    [Required]
    [Column("garantia")]
    public byte Garantia { get; set; }
    
    [Column("numSerie")]
    public string? NumSerie { get; set; }
    
}