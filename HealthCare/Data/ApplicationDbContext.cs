using HealthCare.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HealthCare.Areas.Admin.Models;

namespace HealthCare.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? avatar { get; set; }
        public string? meta { get; set; }
        public Nullable<bool> status { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> createAt { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public Nullable<DateTime> updateAt { get; set; } = DateTime.Now;
        public Nullable<int> doctorInfoId { get; set; }
        [ForeignKey("userId")]
        public virtual ICollection<PatientInfo>? PatientInfos { get; set; }
        [ForeignKey("userId")]
        public virtual ICollection<News>? News { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DoctorInfo> DoctorInfo { get; set; }
        public DbSet<PatientInfo> PatientInfo { get; set; }
        public DbSet<Appoiment> Appoiment { get; set; }
        public DbSet<Specialty> Specialty { get; set; }
        public DbSet<Specialization> Specialization { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<HealthCare.Areas.Admin.Models.UserModel>? UserModel { get; set; }
        public DbSet<HealthCare.Areas.Admin.Models.RoleModel>? RoleModel { get; set; }
    }
}
