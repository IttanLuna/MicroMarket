using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MicroMarket.Contexto;
using MicroMarket.Models;

// Asume que ya agregaste el enum TipoRol en el modelo

namespace MicroMarket.Controllers
{
    public class VendedorsController : Controller
    {
        private readonly MyContext _context;

        public VendedorsController(MyContext context)
        {
            _context = context;
        }

        // GET: Vendedors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vendedores.ToListAsync());
        }

        // GET: Vendedors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(m => m.VendedorId == id);
            if (vendedor == null) return NotFound();

            return View(vendedor);
        }

        // GET: Vendedors/Create
        public IActionResult Create()
        {
            ViewData["Roles"] = GetRolesSelectList();
            return View();
        }

        // POST: Vendedors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VendedorId,Nombre,Email,Contraseña,Telefono,Direccion,Rol")] Vendedor vendedor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = GetRolesSelectList();
            return View(vendedor);
        }

        // GET: Vendedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var vendedor = await _context.Vendedores.FindAsync(id);
            if (vendedor == null) return NotFound();

            ViewData["Roles"] = GetRolesSelectList();
            return View(vendedor);
        }

        // POST: Vendedors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VendedorId,Nombre,Email,Contraseña,Telefono,Direccion,Rol")] Vendedor vendedor)
        {
            if (id != vendedor.VendedorId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendedorExists(vendedor.VendedorId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Roles"] = GetRolesSelectList();
            return View(vendedor);
        }

        // GET: Vendedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var vendedor = await _context.Vendedores
                .FirstOrDefaultAsync(m => m.VendedorId == id);
            if (vendedor == null) return NotFound();

            return View(vendedor);
        }

        // POST: Vendedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendedor = await _context.Vendedores.FindAsync(id);
            if (vendedor != null)
            {
                _context.Vendedores.Remove(vendedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendedorExists(int id)
        {
            return _context.Vendedores.Any(e => e.VendedorId == id);
        }

        // Para pasar la lista de roles a la vista
        private List<SelectListItem> GetRolesSelectList()
        {
            return Enum.GetValues(typeof(TipoRol))
                .Cast<TipoRol>()
                .Select(r => new SelectListItem
                {
                    Value = ((int)r).ToString(),
                    Text = r.ToString()
                }).ToList();
        }
    }
}
