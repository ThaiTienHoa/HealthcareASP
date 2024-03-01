using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HealthCare.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [DisplayName("Tên")]
        public string? firstName { get; set; }
        [DisplayName("Họ")]
        public string? lastName { get; set; }
        [DisplayName("Avatar")]
        public string? avatar { get; set; }
    }
}
