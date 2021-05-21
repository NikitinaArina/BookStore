using System.ComponentModel.DataAnnotations;
namespace BookStore.Models
{
    public class PurchaseForMake
    {
        [Required(ErrorMessage = "Пожалуйста, введите свое ФИО")]
        public string Person { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Вы ввели некорректный email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пожалуйста, введите телефон")]
        public string Phone { get; set; }
    }
}