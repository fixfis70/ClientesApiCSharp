using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientesApi.controller;

[ApiController]
[Route("api/productos")]
public class ProductoController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductoController(AppDbContext context)
    {
        _context = context;
    }
    
    //CRUD

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetAllProductos()
    {
        var prodcutos = await _context.Productos.AsNoTracking().ToListAsync();
        return Ok(prodcutos);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Producto>> GetProductoById(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound(new {msg = "No encontrado"});
        }
        return Ok(new {msg = "encontrado", producto});
    }
    
    [HttpPost]
    public async Task<ActionResult<Producto>> GetProductoById([FromBody] Producto producto)
    {
        producto.Id = 0;

        var productoNumSerie = producto.NumSerie;

        if (productoNumSerie == null)
        {
            return BadRequest(new { msg = "Falta el numero de serie" });
        }

        bool existente = await _context.Productos.AnyAsync(p=>p.NumSerie == productoNumSerie);
        
        if (existente)
        {
            return Conflict(new { msg = $"El numero de serie {productoNumSerie} ya está en uso"});
        }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        
        return Ok(new {msg=$"Creado con el id {producto.Id}"});
    }
    
    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Producto>> GetProductoById(int id,[FromBody] Producto producto)
    {
        producto.Id = id;
        var productoNumSerie = producto.NumSerie;

        if (productoNumSerie == null)
        {
            return BadRequest(new { msg = "Falta el numero de serie" });
        }

        bool existente = await _context.Productos.AnyAsync(p=>p.NumSerie == productoNumSerie && p.Id!=id);
        
        if (existente)
        {
            return Conflict(new { msg = $"El numero de serie {productoNumSerie} ya está en uso"});
        }
        
        
        var find = await _context.Productos.FindAsync(id);

        if (find == null)
        {
            return NotFound(new {msg = "No encontrado"});
        }
        
        find.Descripcion = producto.Descripcion;
        find.Garantia = producto.Garantia;
        find.Nombre = producto.Nombre;
        find.NumSerie = producto.NumSerie;
        find.Precio = producto.Precio;

        await _context.SaveChangesAsync();
        
        return Ok(new {msg="Actualizado correctamente"});
    }



}