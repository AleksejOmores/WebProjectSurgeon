using System.ComponentModel.DataAnnotations;

namespace WebProject.Models
{
    public class Home
    {
        [Required(ErrorMessage = "Введите ваше имя!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите ваш пароль!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите ваш пароль ещё раз!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ReplayPassword { get; set; }
    }
}
