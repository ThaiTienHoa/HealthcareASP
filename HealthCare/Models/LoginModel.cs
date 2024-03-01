using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Models
{
    public class LoginModel
    {
        [DisplayName("Số điện thoại")]
        public string? phoneNumber { get; set; }
        [DisplayName("Mật khẩu")]
        [DataType(DataType.Password)]
        public string? password { get; set; }
        public string? loginInValid { get; set; }
    }
}
