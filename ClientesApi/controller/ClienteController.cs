using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.controller;

[Authorize]
[ApiController]
[Route("api/clientes")]
public class ClienteController(AppDbContext _context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cliente>>> Listar()
    {
        var clients = await _context.Clientes.AsNoTracking().ToListAsync();
        return Ok(clients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> FindById(int? id)
    {
        var client = await _context.Clientes.FindAsync(id);

        if (client == null)
        {
            return NotFound(new { mensaje = $"No existe el cliente con Id {id}" });
        }

        return Ok(client);
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Registrar([FromBody] Cliente cliente)
    {
        cliente.Id = 0;

        if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni))
        {
            return Conflict(new { mensaje = "Ya Existe un cliente con ese Dni" });
        }

        _context.Clientes.Add(cliente);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(FindById), new { id = cliente.Id }, cliente);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<Cliente>> Update(int? id, [FromBody] Cliente cliente)
    {
        if (id == null)
        {
            return BadRequest(new {mensaje = "Falta un Id en la ruta"});
        }

        cliente.Id = id.Value;

        var existente = await _context.Clientes.FindAsync(cliente.Id);

        if (existente == null)
        {
            return NotFound(new {mensaje = $"No existe el cliente con Id {id}" });
        }

        if (await _context.Clientes.AnyAsync(c => c.Dni == cliente.Dni && c.Id != cliente.Id))
        {
            return Conflict(new { mensaje = "Ya Existe un cliente con ese Dni" });
        }
        
        
        existente.Nombre = cliente.Nombre;
        existente.Apellidos = cliente.Apellidos;
        existente.Telefono = cliente.Telefono;
        existente.Dni = cliente.Dni;
        existente.Direccion = cliente.Direccion;
        
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int? id)
    {
        var cliente = await _context.Clientes.FindAsync(id);

        if (cliente == null)
        {
            return NotFound(new { msg = $"No se encontró un cliente con el id {id}" });
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return Ok(new {msg ="Eliminado correctamente"});
    }
}