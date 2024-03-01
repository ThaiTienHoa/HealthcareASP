using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class RegistrationModel
    {
        [DisplayName("Số điện thoại")]
        public string? phoneNumber { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string? password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        [DisplayName("Nhập lại mật khẩu")]
        public string? confirmPassword { get; set; }
        [DisplayName("Họ")]
        public string? lastName { get; set; }
        [DisplayName("Tên")]
        public string? firstName { get; set; }
        public string? registrationInValid { get; set; }
    }
}
