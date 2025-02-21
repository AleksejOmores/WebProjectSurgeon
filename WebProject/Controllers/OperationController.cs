using Microsoft.AspNetCore.Mvc;
using WebProject.Models;
using System.Linq;

namespace WebProject.Controllers
{
    public class OperationController : Controller
    {
        private readonly HospitalSurgeryContext _context;

        public OperationController(HospitalSurgeryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Получаем список всех операций
            var operations = _context.OperationSchedules.ToList();
            return View(operations);
        }

        [HttpGet]
        public IActionResult AddOperation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddOperation(OperationSchedule operation)
        {
            if (ModelState.IsValid)
            {
                _context.OperationSchedules.Add(operation);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(operation);
        }
    }
}
