using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Entities
{
    public class Appoiment
    {
        public int appoimentId { get; set; }
        [DisplayName("Ngày hẹn")]
        public Nullable<DateTime> appDate { get; set; }
        [DisplayName("Lý do")]
        public string? reason { get; set; }
        [DisplayName("Mô tả")]
        public string? description { get; set; }
        public int doctorId { get; set; }
        public int patientId { get; set; }
        public string? meta { get; set; }
        [DisplayName("Trạng thái")]
        public Nullable<bool> status { get; set; }
        [DisplayName("Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
    }
}
