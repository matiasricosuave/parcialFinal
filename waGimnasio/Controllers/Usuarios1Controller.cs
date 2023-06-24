using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using waGimnasio.Models;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace waGimnasio.Controllers
{
    public class Usuarios1Controller : Controller
    {
        private readonly GimnasioContext _context;

        public Usuarios1Controller(GimnasioContext context)
        {
            _context = context;
        }

        // GET: Usuarios1
        public async Task<IActionResult> Index()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            var gimnasioContext = _context.Usuarios.Include(u => u.IdNavigation);
            return View(await gimnasioContext.ToListAsync());
        }

        // GET: Usuarios1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios1/Create
        public IActionResult Create()
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id");
            return View();
        }

        // POST: Usuarios1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo electrónico ya está en uso
                if (_context.Usuarios.Any(u => u.Email == usuario.Email))
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                    ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
                    return View(usuario);
                }

                // Verificar si la persona ya tiene un usuario existente
                if (_context.Usuarios.Any(u => u.Id == usuario.Id))
                {
                    ModelState.AddModelError("", "La persona ya tiene un usuario existente.");
                    ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
                    return View(usuario);
                }

                // Hashear la contraseña con MD5
                using (MD5 md5 = MD5.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(usuario.Password);
                    byte[] hashBytes = md5.ComputeHash(passwordBytes);
                    string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                    usuario.Password = hashedPassword;
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
            return View(usuario);
        }

        // GET: Usuarios1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
            return View(usuario);
        }

        // POST: Usuarios1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,Estado")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Verificar si el correo electrónico ya está en uso por otro usuario
                var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email && u.Id != usuario.Id);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "El correo electrónico ya está en uso.");
                    ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
                    return View(usuario);
                }
                // Hashear la contraseña con MD5
                using (MD5 md5 = MD5.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(usuario.Password);
                    byte[] hashBytes = md5.ComputeHash(passwordBytes);
                    string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
                    usuario.Password = hashedPassword;
                }

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
            return View(usuario);
        }

        // GET: Usuarios1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr)) { return RedirectToAction("Login", "Usuarios"); }
            var usu = JsonConvert.DeserializeObject(usuStr);
            if (usu == null) return RedirectToAction("Login", "Usuarios"); ;
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'GimnasioContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
