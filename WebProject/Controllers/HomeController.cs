using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebProject.Models;
using System.Linq;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly HospitalSurgeryContext _context;  
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, HospitalSurgeryContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexMain()
        {
            var model = new MainPage();
            if (TempData.ContainsKey("UserName"))
            {
                model.UserName = TempData["UserName"].ToString();
            }
            return View(model);
        }

        public IActionResult IndexEmergency()
        {
            return View();
        }

        public async Task<IActionResult> IndexScheduled()
        {
            var surgeons = await _context.Surgeons.ToListAsync();
            var operationRooms = await _context.OperatingRooms.ToListAsync();
            ViewBag.Surgeons = surgeons;
            ViewBag.OperatingRooms = operationRooms;
            return View();
        }

        [Authorize]
        public IActionResult UserSettings()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

            var model = new UserSettingsViewModel
            {
                Username = user.Username
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UserSettings(UserSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    user.Username = model.Username;

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        user.Password = model.NewPassword;
                    }

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Настройки успешно сохранены";
                    return RedirectToAction("UserSettings");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Home home)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == home.Name && u.Password == home.Password);
                if (user != null)
                {
                    TempData["UserName"] = user.Username;
                    return RedirectToAction("IndexMain");
                }
                ModelState.AddModelError("", "Неверные имя или пароль");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult AddProcedure([FromBody] OperationSchedule model)
        {
            // Проверка, что дата окончания не раньше даты начала
            if (model.EndDateTime.Date < model.StartDateTime.Date)
            {
                return Json(new { success = false, message = "Дата окончания операции не может быть раньше даты начала." });
            }

            // Если операции в один день - проверяем время
            if (model.StartDateTime.Date == model.EndDateTime.Date
                && model.EndDateTime <= model.StartDateTime)
            {
                return Json(new
                {
                    success = false,
                    message = "Для операций в рамках одного дня время окончания должно быть позже времени начала."
                });
            }

            // Проверка пересечения с существующими операциями в этой операционной
            bool conflict = _context.OperationSchedules
                .Where(op => op.OperatingRoomId == model.OperatingRoomId)
                .Any(op => model.StartDateTime < op.EndDateTime
                         && model.EndDateTime > op.StartDateTime);

            if (conflict)
            {
                return Json(new
                {
                    success = false,
                    message = "Конфликт времени: в этой операционной уже есть операции в выбранный интервал."
                });
            }

            model.CreatedAt = DateTime.Now;
            _context.OperationSchedules.Add(model);
            _context.SaveChanges();

            return Json(new { success = true, message = "Операция успешно добавлена." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
