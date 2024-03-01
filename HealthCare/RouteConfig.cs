using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

public static class RouteConfig
{
    public static void Configure(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapControllerRoute(
        name: "Admin",
        pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
    );

        endpoints.MapControllerRoute(
        name: "Ve-chung-toi",
        pattern: "ve-chung-toi",
        defaults: new { controller = "Home", action = "About" }
        );

        endpoints.MapControllerRoute(
        name: "Dich-vu",
        pattern: "dich-vu",
        defaults: new { controller = "Home", action = "Service" }
        );

        endpoints.MapControllerRoute(
        name: "Lien-he",
        pattern: "lien-he",
        defaults: new { controller = "Home", action = "Contact" }
        );

        endpoints.MapControllerRoute(
        name: "Trang-chu",
        pattern: "trang-chu/{action=Index}/{id?}",
        defaults: new { controller = "Home", action = "Index" }
        );

        endpoints.MapControllerRoute(
        name: "Dang-nhap",
        pattern: "dang-nhap",
        defaults: new { controller = "UserAuth", action = "Login" }
        );

        endpoints.MapControllerRoute(
        name: "Dang-ky",
        pattern: "dang-ky",
        defaults: new { controller = "UserAuth", action = "Register" }
        );

        endpoints.MapControllerRoute(
        name: "Thong-tin-tai-khoan",
        pattern: "thong-tin-tai-khoan",
        defaults: new { controller = "UserAuth", action = "UpdateProfile" }
        );

        endpoints.MapControllerRoute(
        name: "Tao-ho-so",
        pattern: "ho-so-benh-nhan/tao-ho-so-moi/{id?}",
        defaults: new { controller = "PatientInfo", action = "Create" }
        );

        endpoints.MapControllerRoute(
        name: "Cap-nhat-ho-so",
        pattern: "ho-so-benh-nhan/cap-nhat-ho-so/{id?}",
        defaults: new { controller = "PatientInfo", action = "Edit" }
        );

        endpoints.MapControllerRoute(
        name: "Ho-so-benh-nhan",
        pattern: "ho-so-benh-nhan/{action=Index}/{id?}",
        defaults: new { controller = "PatientInfo", action = "Index" }
        );

        endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );
    }
}
