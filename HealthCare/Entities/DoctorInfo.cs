using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Entities
{
    public class DoctorInfo
    {
        public int doctorInfoId { get; set; }
        public string? userId { get; set; }
        [DisplayName("Tên bác sĩ")]
        public string? fullName { get; set; }
        [DisplayName("Email")]
        public string? email { get; set; }
        [DisplayName("Số điện thoại")]
        public string? phoneNumber { get; set; }
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> birthday { get; set; }
        [DisplayName("Giới tính")]
        public Nullable<bool> gender { get; set; }
        [DisplayName("Địa chỉ")]
        public string? address { get; set; }

        [DisplayName("Học hàm/ Học vị")]
        public string? qualification { get; set; }
        [DisplayName("Khoa")]
        public int specialtyId { get; set; }
        [DisplayName("Giới thiệu")]
        public string? about { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<bool> status { get; set; }
        public string? meta { get; set; }
        [DisplayName("Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
        [ForeignKey("doctorId")]
        public virtual ICollection<Appoiment>? Appoiments { get; set; }
    }
}
