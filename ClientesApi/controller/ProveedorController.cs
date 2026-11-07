using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.controller;

[ApiController]
[Route("api/proveedores")]
public class ProveedorController(AppDbContext context) : ControllerBase
{
    //CRUD

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Proveedor>>> GetAllProveedores()
    {
        var proveedors = await context.Proveedores.AsNoTracking().ToListAsync();
        return Ok(proveedors);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Proveedor>> GetProveedorById(int id)
    {
        var proveedor = await context.Proveedores.FindAsync(id);
        if (proveedor == null)
        {
            return NotFound(new { msg = "No encontrado" });
        }

        return Ok(new { msg = "encontrado", Proveedor = proveedor });
    }

    [HttpPost]
    public async Task<ActionResult<Proveedor>> PostProveedor([FromBody] Proveedor proveedor)
    {
        proveedor.Id = 0;

        if (AnyNull([proveedor.Correo, proveedor.Nombre, proveedor.Representante, proveedor.Telefono]))
        {
            return BadRequest(new { msg = "Falta algun dato" });
        }

        context.Proveedores.Add(proveedor);
        await context.SaveChangesAsync();

        return Ok(new { msg = $"Creado con el id {proveedor.Id}" });
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Proveedor>> PutProveedor(int id, [FromBody] Proveedor proveedor)
    {
        proveedor.Id = id;

        if (AnyNull([proveedor.Correo, proveedor.Nombre, proveedor.Representante, proveedor.Telefono]))
        {
            return BadRequest(new { msg = "Falta algun dato" });
        }

        var find = await context.Proveedores.FindAsync(id);

        if (find == null)
        {
            return NotFound(new { msg = "No encontrado" });
        }

        find.Correo = proveedor.Correo;
        find.Nombre = proveedor.Nombre;
        find.Representante = proveedor.Representante;
        find.Telefono = proveedor.Telefono;

        await context.SaveChangesAsync();

        return Ok(new { msg = "Actualizado correctamente" });
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> DeleteProveedor(int id)
    {
        var proveedor = await context.Proveedores.FindAsync(id);

        if (proveedor == null)
        {
            return NotFound(new { msg = "No encontrado" });
        }

        context.Proveedores.Remove(proveedor);
        await context.SaveChangesAsync();

        return Ok(new { msg = "Eliminado correctamente" });
    }

    private bool AnyNull(object?[] obj)
    {
        return obj.All(t => t != null);
    }
}