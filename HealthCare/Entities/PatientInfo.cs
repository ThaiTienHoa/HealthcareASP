using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Entities
{
    public class PatientInfo
    {
        public int patientInfoId { get; set; }
        [DisplayName("Họ và tên")]
        public string? fullName { get; set;}
        [DisplayName("Số điện thoại")]
        public string? phoneNumber { get; set;}
        [DisplayName("Ngày sinh")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<DateTime> birthday { get; set;}
        [DisplayName("Giới tính")]
        public Nullable<bool> gender { get; set;}
        [DisplayName("BHYT")]
        public string? insurance { get; set;}
        [DisplayName("Địa chỉ")]
        public string? address { get; set;}
        [DisplayName("CMND/CCCD")]
        public string? nationalId { get; set;}
        [DisplayName("Nghề nghiệp")]
        public string? job { get; set; }
        public string? userId { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<bool> status { get; set;}
        public string? meta { get; set;}
        [DisplayName("Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set;}
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set;} = DateTime.Now;

        [ForeignKey("patientId")]
        public virtual ICollection<Appoiment>? Appoiments { get; set; }
    }
}
