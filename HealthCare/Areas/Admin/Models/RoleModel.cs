using System.ComponentModel;

namespace HealthCare.Areas.Admin.Models
{
    public class RoleModel
    {
        public string Id { get; set; }
        [DisplayName("Role Name")]
        public string? roleName { get; set; }
    }
}
