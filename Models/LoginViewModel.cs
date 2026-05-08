using System.ComponentModel.DataAnnotations;

namespace PharmaSphere.Models
{
    /// <summary>
    /// ViewModel for user login credentials.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>Gets or sets the username.</summary>
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }

        /// <summary>Gets or sets the password.</summary>
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        /// <summary>Gets or sets a value indicating whether to remember the login session.</summary>
        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool RememberMe { get; set; }
    }
}
