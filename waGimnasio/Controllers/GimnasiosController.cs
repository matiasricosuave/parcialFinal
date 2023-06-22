using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using waGimnasio.clases;
using waGimnasio.Models;

namespace waGimnasio.Controllers
{
    public class GimnasiosController : Controller
    {
        private readonly GimnasioContext _context;

        public GimnasiosController(GimnasioContext context)
        {
            _context = context;
        }

        // GET: Gimnasios
        public async Task<IActionResult> Index()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            var gimnasioContext = _context.Gimnasios
            .Include(g => g.IdDepartamentoNavigation)
            .Where(g => g.Estado == true); // Filtrar solo los gimnasios con Estado = true

            return View(await gimnasioContext.ToListAsync());
        }

        // GET: Gimnasios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Gimnasios == null)
            {
                return NotFound();
            }

            var gimnasio = await _context.Gimnasios
                .Include(g => g.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (gimnasio == null)
            {
                return NotFound();
            }

            return View(gimnasio);
        }

        // GET: Gimnasios/Create
        public IActionResult Create()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "Id", "Nombre");
            return View();
        }


        // POST: Gimnasios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Telefono,Direccion,IdDepartamento,Estado")] Gimnasio gimnasio)
        {
            if (ModelState.IsValid)
            {
                // Cargar el departamento correspondiente al IdDepartamento seleccionado
                gimnasio.IdDepartamentoNavigation = await _context.Departamentos.FindAsync(gimnasio.IdDepartamento);

                _context.Add(gimnasio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "Id", "Nombre", gimnasio.IdDepartamento);
            return View(gimnasio);
        }


        // GET: Gimnasios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Gimnasios == null)
            {
                return NotFound();
            }

            var gimnasio = await _context.Gimnasios.FindAsync(id);
            if (gimnasio == null)
            {
                return NotFound();
            }
            ViewData["IdDepartamento"] = new SelectList(_context.Departamentos, "Id", "Nombre", gimnasio.IdDepartamento);
            return View(gimnasio);
        }

        // POST: Gimnasios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nombre,Telefono,Direccion")] Gimnasio gimnasio)
        {
            if (id != gimnasio.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingGimnasio = await _context.Gimnasios.FindAsync(id);
                    if (existingGimnasio == null)
                    {
                        return NotFound();
                    }

                    // Copiar los valores de las propiedades editables en la entidad existente
                    existingGimnasio.Nombre = gimnasio.Nombre;
                    existingGimnasio.Telefono = gimnasio.Telefono;
                    existingGimnasio.Direccion = gimnasio.Direccion;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GimnasioExists(gimnasio.Codigo))
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
            return View(gimnasio);
        }

        // GET: Gimnasios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Gimnasios == null)
            {
                return NotFound();
            }

            var gimnasio = await _context.Gimnasios
                .Include(g => g.IdDepartamentoNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (gimnasio == null)
            {
                return NotFound();
            }

            return View(gimnasio);
        }

        // POST: Gimnasios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gimnasios == null)
            {
                return Problem("Entity set 'GimnasioContext.Gimnasios' is null.");
            }

            var gimnasio = await _context.Gimnasios.FindAsync(id);
            if (gimnasio != null)
            {
                gimnasio.Estado = false; // Actualizar el estado a false en lugar de eliminarlo
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GimnasioExists(int id)
        {
          return (_context.Gimnasios?.Any(e => e.Codigo == id)).GetValueOrDefault();
        }
    }
}
