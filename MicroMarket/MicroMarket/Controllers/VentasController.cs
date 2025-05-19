using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicroMarket.Contexto;
using MicroMarket.Models;

namespace MicroMarket.Controllers
{
    public class VentasController : Controller
    {
        private readonly MyContext _context;

        public VentasController(MyContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index(string searchString)
        {
            // Prepara la consulta base, incluyendo al cliente
            var ventas = _context.Ventas
                                 .Include(v => v.Cliente)
                                 .Include(v => v.Producto)
                                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                ventas = ventas.Where(v =>
                    // Filtrar por número de venta (convertido a texto)
                    v.NroVenta.ToString().Contains(searchString) ||
                    // Filtrar por nombre de cliente usando LIKE en SQL
                    EF.Functions.Like(v.Cliente.Nombre, $"%{searchString}%")
                );
            }

            // Ejecuta la consulta
            var listaFiltrada = await ventas.ToListAsync();
            return View(listaFiltrada);
        }



        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.Producto)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Info");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Info");

            // Enviar precios al frontend
            var productosConPrecios = _context.Productos
                .Select(p => new { p.ProductoId, p.Precio })
                .ToDictionary(p => p.ProductoId, p => p.Precio);
            ViewBag.PreciosProductos = productosConPrecios;
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VentaId,FechaVenta,ProductoId,Cantidad,PrecioUnidad,Total,ClienteId")] Venta venta)
        {
            if (ModelState.IsValid)
            {
                // Obtener número automático
                venta.NroVenta = GetNumero();

                // Buscar producto
                var producto = await _context.Productos.FindAsync(venta.ProductoId);

                if (producto == null)
                {
                    ModelState.AddModelError("", "Producto no encontrado.");
                    return View(venta);
                }

                // Verificar stock
                if (producto.Stock < venta.Cantidad)
                {
                    ModelState.AddModelError("", "Stock insuficiente.");
                    ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Info", venta.ClienteId);
                    ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Info", venta.ProductoId);
                    return View(venta);
                }

                // Descontar stock
                producto.Stock -= venta.Cantidad;
                _context.Update(producto);

                // Guardar venta
                _context.Add(venta);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Info", venta.ClienteId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "Info", venta.ProductoId);
            return View(venta);
        }



        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Direccion", venta.ClienteId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,NroVenta,FechaVenta,Mes,Anio,Detalle,Cantidad,PrecioUnidad,Total,ClienteId,TelevisorId")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Direccion", venta.ClienteId);
            return View(venta);
        }

        private int GetNumero()
        {
            if (_context.Ventas.ToList().Count > 0)
                return _context.Ventas.Max(i => i.NroVenta + 1);
            return 1;
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.VentaId == id);
        }
    }
}
