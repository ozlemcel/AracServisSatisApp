using System.ComponentModel.DataAnnotations;

namespace AracServisSatis.Models
{
    public class EditProfileViewModel
    {
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telefon")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre (Tekrar)")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}

