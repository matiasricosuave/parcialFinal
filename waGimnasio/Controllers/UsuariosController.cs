using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using waGimnasio.clases;
using waGimnasio.Models;

namespace waGimnasio.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GimnasioContext _context;

        public UsuariosController(GimnasioContext context)
        {
            _context = context;
        }

        // GET: Usuarios


        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario está autenticado
            var usuStr = this.HttpContext.Session.GetString("dat_usuario");
            if (string.IsNullOrEmpty(usuStr))
            {
                return RedirectToAction("Login", "Usuarios");
            }

            // Obtener los datos del usuario de la sesión
            var cusuario = JsonConvert.DeserializeObject<cDatosPer>(usuStr);

            var gimnasioContext = _context.Usuarios.Include(u => u.IdNavigation);
            var model = await gimnasioContext.ToListAsync();

            // Agregar los datos del usuario al modelo
            foreach (var usuario in model)
            {
                usuario.Email = cusuario.nombre_completo;
            }

            return View(model);
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,Estado")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Personas, "Id", "Id", usuario.Id);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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

        // POST: Usuarios/Edit/5
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

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
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

        // POST: Usuarios/Delete/5
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
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int id, [Bind("Email,Password")] Usuario usuario)
        {
            var parametrores = new SqlParameter("@sw", SqlDbType.Bit);
            parametrores.Direction = ParameterDirection.Output;
            await _context.Database
                     .ExecuteSqlInterpolatedAsync(
                     $@"EXEC validar_pass
                      @email={usuario.Email},@pass={usuario.Password},@sw={parametrores} OUTPUT ");
            var res = (bool)parametrores.Value;
            if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrEmpty(usuario.Password))
            {
                ViewBag.msg = "Debe completar todos los campos.";
                return View(usuario);
            }
            if (res)
            {
                var res_u = _context.Usuarios.Single(c => c.Email == usuario.Email);
                var datPer = _context.Personas.Find(res_u.Id);

                var cusuario = new cDatosPer();
                cusuario.id = datPer.Id;
                cusuario.nombre_completo = datPer.Nombre;
                cusuario.email = usuario.Email;
                HttpContext.Session.SetString("dat_usuario", JsonConvert.SerializeObject(cusuario));

                ViewBag.NombreUsuario = cusuario.nombre_completo; // Agrega esta línea para guardar el nombre en ViewBag

                return RedirectToAction("Index", "Usuarios");
            }

            ViewBag.msg = "Credenciales NO validas!!!";
            return View(usuario);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia todas las variables de sesión
            return RedirectToAction("Login"); // Redirige al método de inicio de sesión
        }
    }
}
