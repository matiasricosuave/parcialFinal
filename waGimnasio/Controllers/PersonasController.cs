using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using waGimnasio.Models;

namespace waGimnasio.Controllers
{
    public class PersonasController : Controller
    {
        private readonly GimnasioContext _context;

        public PersonasController(GimnasioContext context)
        {
            _context = context;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            var gimnasioContext = _context.Personas.Include(p => p.CodigoGimnasioNavigation);
            return View(await gimnasioContext.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.CodigoGimnasioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Nombre");
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ci,Nombre,Direccion,Telefono,Email,FechaNacimiento,Sexo,CodigoGimnasio")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el CI ya existe en la base de datos
                if (_context.Personas.Any(p => p.Ci == persona.Ci))
                {
                    ModelState.AddModelError("", "El CI ya existe en la base de datos.");
                    ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Codigo", persona.CodigoGimnasio);
                    return View(persona);
                }

                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Codigo", persona.CodigoGimnasio);
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Nombre", persona.CodigoGimnasio);
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ci,Nombre,Direccion,Telefono,Email,FechaNacimiento,Sexo,CodigoGimnasio")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Verificar si el CI ya existe en la base de datos (excluyendo al usuario actual)
                if (_context.Personas.Any(p => p.Ci == persona.Ci && p.Id != id))
                {
                    ModelState.AddModelError("", "El CI ya existe en la base de datos.");
                    ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Codigo", persona.CodigoGimnasio);
                    return View(persona);
                }

                try
                {
                    _context.Update(persona);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            ViewData["CodigoGimnasio"] = new SelectList(_context.Gimnasios, "Codigo", "Codigo", persona.CodigoGimnasio);
            return View(persona);
        }

        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Personas == null)
            {
                return NotFound();
            }

            var persona = await _context.Personas
                .Include(p => p.CodigoGimnasioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            var usuarioAsociado = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuarioAsociado != null)
            {
                TempData["ErrorMessage"] = "Primero debe eliminar el usuario asociado a esta persona.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return (_context.Personas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
