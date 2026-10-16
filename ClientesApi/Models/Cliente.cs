using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClientesApi.Models;


/**
 * Create table clientes(
	id int identity(1,1) primary key,
	apellidos nvarchar(70) not null,
	nombres nvarchar(70) not null,
	telefono nvarchar(9) not null,
	correo nvarchar(150) not null,
	dni char(8) not null,
	direccion nvarchar(150) not null,

	constraint uk_dni_cli unique (dni)
	)
 */
[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    
    [Required,StringLength(70)]
    [Column("apellidos")]
    public string Apellidos { get; set; }
    
    [Required,StringLength(70)]
    [Column("nombres")]
    public string Nombre { get; set; }
    
    [Required,StringLength(9)]
    [Column("telefono")]
    public string Telefono { get; set; }
    

    [Required,StringLength(8)]
    [Column("dni")]
    public string Dni { get; set; }

    [Required, StringLength(150)]
    [Column("direccion")]
    public string Direccion { get; set; }

    [Required, StringLength(150)]
    [Column("correo")]
    public string Correo { get; set; }
}