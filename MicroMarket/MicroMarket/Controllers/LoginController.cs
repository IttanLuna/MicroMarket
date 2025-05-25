using MicroMarket.Contexto;
using Microsoft.AspNetCore.Mvc;

namespace MicroMarket.Controllers
{
    public class LoginController : Controller
    {
        MyContext _context;
        public LoginController(MyContext context)
        {
            //Inyeccion de dependencias
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            const int MAX_ATTEMPTS = 3;
            const int LOCKOUT_MINUTES = 5;

            // Obtener valores de sesión
            int attempts = HttpContext.Session.GetInt32("LoginAttempts") ?? 0;
            string? lockoutTimeStr = HttpContext.Session.GetString("LockoutTime");
            DateTime? lockoutTime = lockoutTimeStr != null ? DateTime.Parse(lockoutTimeStr) : (DateTime?)null;

            // Verificar si está bloqueado
            if (lockoutTime.HasValue && lockoutTime > DateTime.Now)
            {
                var remaining = lockoutTime.Value - DateTime.Now;
                TempData["LoginError"] = $"Cuenta bloqueada. Intenta nuevamente en {remaining.Minutes} minutos y {remaining.Seconds} segundos.";
                return RedirectToAction("Index", "Login");
            }

            // Intentar autenticación
            var vendedor = _context.Vendedores
                .FirstOrDefault(x => x.Email == email && x.Contraseña == password);

            if (vendedor != null)
            {
                // Éxito: limpiar intentos
                HttpContext.Session.Remove("LoginAttempts");
                HttpContext.Session.Remove("LockoutTime");

                HttpContext.Session.SetInt32("Rol", (int)vendedor.Rol);
                HttpContext.Session.SetString("Nombre", vendedor.Nombre);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                attempts++;
                HttpContext.Session.SetInt32("LoginAttempts", attempts);

                if (attempts >= MAX_ATTEMPTS)
                {
                    DateTime newLockout = DateTime.Now.AddMinutes(LOCKOUT_MINUTES);
                    HttpContext.Session.SetString("LockoutTime", newLockout.ToString());

                    TempData["LoginError"] = $"Demasiados intentos. Cuenta bloqueada por {LOCKOUT_MINUTES} minutos.";
                }
                else
                {
                    TempData["LoginError"] = $"Credenciales incorrectas. Intento {attempts} de {MAX_ATTEMPTS}.";
                }

                return RedirectToAction("Index", "Login");
            }
        }


        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear(); // Limpia todos los datos de sesión
            return RedirectToAction("Index", "Login");
        }

    }
}

