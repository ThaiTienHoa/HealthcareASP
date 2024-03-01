using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Entities
{
    public class Specialization
    {
        public int specializationId { get; set; }
        [DisplayName("Chuyên khoa")]
        public string? specializationName { get; set; }
        public int specialtyId { get; set; }
        [DisplayName("Thứ tự")]
        public Nullable<int> order { get; set; }
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
