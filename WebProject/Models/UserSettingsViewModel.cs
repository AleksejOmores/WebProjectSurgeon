using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
    public class UserSettingsViewModel : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Логин обязателен")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string? Email { get; set; }

        [Display(Name = "Тема оформления")]
        public string ThemePreference { get; set; } = "light";

        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name = "Подтвердите пароль")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }
    }
}
