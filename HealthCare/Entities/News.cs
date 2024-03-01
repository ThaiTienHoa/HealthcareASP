using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HealthCare.Entities
{
    public class News
    {
        public int newsId { get; set; }
        [DisplayName("Tiêu đề")]
        public string? title { get; set;}
        [DisplayName("Mô tả")]
        public string? description { get; set; }
        [DisplayName("Nội dung")]
        public string? htmlContent { get; set; }
        [DisplayName("Ảnh bìa")]
        public string? img { get; set; }
        public string userId { get; set; }
        [DisplayName("Thứ tự")]
        public Nullable<int> order { get; set; }
        public string? meta { get; set; }
        [DisplayName("Trạng thái")]  
        
        public Nullable<bool> status { get; set;}
        [DisplayName("Ngày đăng")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayName("Ngày cập nhật")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
    }
}
