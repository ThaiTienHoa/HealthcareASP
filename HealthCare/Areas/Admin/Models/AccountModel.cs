using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Areas.Admin.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [DisplayName("Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [DisplayName("Role")]
        public string? role { get; set; }
        [DisplayName("Tên")]
        public string? firstName { get; set; }
        [DisplayName("Họ")]
        public string? lastName { get; set; }
        [DisplayName("Avatar")]
        public string? avatar { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<bool> status { get; set; }
        [DisplayName("Ngày tham gia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
    }

    public class CreateAccountModel
    {        
        [DisplayName("Họ")]
        public string? lastName { get; set; }
        [DisplayName("Tên")]
        public string? firstName { get; set; }
        [DisplayName("Số điện thoại")]
        public string? phoneNumber { get; set; }
        [DisplayName("Role")]
        public string ? role { get; set; }
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu")]
        public string? password { get; set; }
        [Compare("password")]
        [DataType(DataType.Password)]
        [DisplayName("Nhập lại mật khẩu")]
        public string? confirmPassword { get; set; }

    }
}
