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

            if (TempData.TryGetValue("UserName", out var userName))
            {
                model.UserName = userName.ToString();
            }
            else
            {
                model.UserName = "�����";
            }

            return View(model);
        }

        public async Task<IActionResult> IndexEmergency()
        {
            var surgeons = await _context.Surgeons.ToListAsync();
            var operationRooms = await _context.OperatingRooms.ToListAsync();
            ViewBag.Surgeons = surgeons;
            ViewBag.OperatingRooms = operationRooms;
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
                    TempData["SuccessMessage"] = "��������� ������� ���������";
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
                ModelState.AddModelError("", "�������� ��� ��� ������");
            }
            return View("Index");
        }

        [HttpPost]
        public IActionResult AddProcedure([FromBody] OperationSchedule model)
        {
            // �������� ��������� ������, ������� ������������ ����
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "������������ ������ ��������"
                });
            }

            // 1. ��������: ���� ������ �� ������ ���� � �������
            if (model.StartDateTime <= DateTime.Now)
            {
                return Json(new
                {
                    success = false,
                    message = "���������� ������������� �������� �� ��������� ���� � �����"
                });
            }

            // 2. ���� �������� � ���� ���� � ����� ��������� ������ ���� ����� ������� ������
            if (model.StartDateTime.Date == model.EndDateTime.Date && model.EndDateTime <= model.StartDateTime)
            {
                return Json(new
                {
                    success = false,
                    message = "����� ��������� �������� ������ ���� ����� ������� ������"
                });
            }

            // 3. �������� �� ����������� � ��� ���������������� ���������� � ��������� ������������
            bool conflict = _context.OperationSchedules
                .Where(op => op.OperatingRoomId == model.OperatingRoomId)
                .Any(op =>
                    model.StartDateTime < op.EndDateTime &&
                    model.EndDateTime > op.StartDateTime
                );

            if (conflict)
            {
                return Json(new
                {
                    success = false,
                    message = "�������� �������: � ��������� ������ ��� ���� ��������"
                });
            }

            model.CreatedAt = DateTime.Now;
            _context.OperationSchedules.Add(model);
            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "�������� ������� �������������"
            });
        }

        [HttpGet]
        public IActionResult GetOperations()
        {
            // �������� �������� � ������������� ������������� � ���������
            var operations = _context.OperationSchedules
                .Include(op => op.OperatingRoom)
                .Include(op => op.Surgeon)
                .OrderBy(op => op.StartDateTime)
                .ToList();

            var operationsDto = operations.Select(op => new {
                operatingRoom = op.OperatingRoom != null ? op.OperatingRoom.RoomName : "�� �������",
                surgeon = op.Surgeon != null ? op.Surgeon.FullName : "�� ������",
                startDateTime = op.StartDateTime.ToString("dd.MM HH:mm"),
                endDateTime = op.EndDateTime.ToString("dd.MM HH:mm"),
                status = op.Status ?? "�� ������"
            });

            return Json(operationsDto);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
