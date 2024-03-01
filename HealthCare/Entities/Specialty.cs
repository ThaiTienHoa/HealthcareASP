using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare.Entities
{
    public class Specialty
    {
        public int specialtyId { get; set; }
        [DisplayName("Khoa")]
        public string? specialtyName { get; set; }
        [DisplayName("Thứ tự")]
        public Nullable<int> order { get; set; }
        public string? meta { get; set;}
        [DisplayName("Tình trạng")]
        public Nullable<bool> status { get; set;}
        [DisplayName("Ngày tạo")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
        [ForeignKey("specialtyId")]
        public virtual ICollection<DoctorInfo>? DoctorInfos { get; set; }
        [ForeignKey("specialtyId")]
        public virtual ICollection<Specialization>? Specializations { get; set; }
    }
}
