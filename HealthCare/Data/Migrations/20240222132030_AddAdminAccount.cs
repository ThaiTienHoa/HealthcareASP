using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

#nullable disable

namespace HealthCare.Data.Migrations
{
    public partial class AddAdminAccount : Migration
    {
        const string ADMIN_USER_GUID = "90394e7d-76a9-49fd-9163-7fb32bb22ed3";
        const string ADMIN_ROLE_GUID = "153d050b-b451-4712-b202-d583ea00265b";
        const string DOCTOR_ROLE_GUID = "7736a798-1d93-4fc4-a35e-a6c67d6e5bf3";
        const string PATIENT_ROLE_GUID = "9a494cac-5cd5-4553-917f-9ecb65e57364";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var passwordHash = hasher.HashPassword(null, "healthcare");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetUsers(Id, firstName, lastName, avatar, meta, status, createAt, updateAt, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)");
            sb.AppendLine("VALUES(");
            sb.AppendLine($"'{ADMIN_USER_GUID}'");
            sb.AppendLine(", 'Admin'");
            sb.AppendLine(", 'HealthCare'");
            sb.AppendLine(", ''");
            sb.AppendLine(", ''");
            sb.AppendLine(", 1");
            sb.AppendLine(", '2024-02-18 00:00:00'");
            sb.AppendLine(", '2024-02-18 00:00:00'");
            sb.AppendLine(", 'admin@healthcare.com'");
            sb.AppendLine(", 'ADMIN@HEALTHCARE.COM'");
            sb.AppendLine(", 'healthcare@gmail.com'");
            sb.AppendLine(", 'healthcare@gmail.com'");
            sb.AppendLine(", 0");
            sb.AppendLine($", '{passwordHash}'");
            sb.AppendLine(", ''");
            sb.AppendLine(", '0338650639'");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(", 0");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());

            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}', 'Admin', 'ADMIN')");
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{DOCTOR_ROLE_GUID}', 'Doctor', 'DOCTOR')");
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) VALUES ('{PATIENT_ROLE_GUID}', 'Patient', 'PATIENT')");

            migrationBuilder.Sql($"INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES ('{ADMIN_USER_GUID}', '{ADMIN_ROLE_GUID}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRoles WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetUsers WHERE Id = '{ADMIN_USER_GUID}'");

            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{ADMIN_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{DOCTOR_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = '{PATIENT_ROLE_GUID}'");
        }
    }
}
