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
    public class DepartamentosController : Controller
    {
        private readonly GimnasioContext _context;

        public DepartamentosController(GimnasioContext context)
        {
            _context = context;
        }

        // GET: Departamentos

        public async Task<IActionResult> Index()
        {
                var usuStr = this.HttpContext.Session.GetString("dat_usuario");
                if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
                var usu = JsonConvert.DeserializeObject(usuStr);
                if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
                return _context.Departamentos != null ? 
                          View(await _context.Departamentos.ToListAsync()) :
                          Problem("Entity set 'GimnasioContext.Departamentos'  is null.");
        }

        // GET: Departamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // GET: Departamentos/Create
        public IActionResult Create()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            return View();
        }

        // POST: Departamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                var datos = _context.Departamentos.Where(x => x.Nombre == departamento.Nombre).ToList();
                if (datos.Count <= 0)
                {
                    _context.Add(departamento);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Mensaje = "¡¡La descripción del DEPARTAMENTO YA EXISTE!!";
            }
            return View(departamento);
        }


        // GET: Departamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                return NotFound();
            }
            return View(departamento);
        }

        // POST: Departamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Departamento departamento)
        {
            if (id != departamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Id))
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
            return View(departamento);
        }

        // GET: Departamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Departamentos == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departamentos == null)
            {
                return Problem("Entity set 'GimnasioContext.Departamentos'  is null.");
            }
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartamentoExists(int id)
        {
          return (_context.Departamentos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
