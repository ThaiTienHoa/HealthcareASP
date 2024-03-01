$(function () {

    var userLoginButton = $("#UserLoginForm button[name='login']").click(onUserLoginClick);

    function onUserLoginClick() {

        var url = "/UserAuth/Login";

        // Lấy giá trị của token chống tấn công CSRF từ input ẩn
        var antiForgeryToken = $("#UserLoginForm input[name='__RequestVerificationToken']").val();

        var phoneNumber = $("#UserLoginForm input[name='phoneNumber']").val();
        var password = $("#UserLoginForm input[name='password']").val();

        var userInput = {
            __RequestVerificationToken: antiForgeryToken,
            phoneNumber: phoneNumber,
            password: password,
        };

        // Gửi request AJAX đến UserAuth/Login
        $.ajax({
            type: "POST",
            url: url,
            data: userInput,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='loginInValid']").val() == "true";

                if (hasErrors == true) {
                    $("#LoginPartial").html(data);

                    userLoginButton = $("#UserLoginForm button[name='login']").click(onUserLoginClick);

                    var form = $("#UserLoginForm");

                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);

                }
                else {
/*                    // Lấy URL hiện tại
                    var currentUrl = window.location.href;

                    // Sử dụng regular expression để tìm và trích xuất giá trị của tham số 'returnUrl'
                    var match = currentUrl.match(/[?&]ReturnUrl=([^&]*)/);

                    // Kiểm tra xem có tìm thấy kết quả không
                    if (match) {
                        // Giá trị của tham số 'returnUrl'
                        var returnUrl = decodeURIComponent(match[1]);
                        if (returnUrl != null && returnUrl !== "") {
                            location.href = returnUrl;
                        }
                    }*/
                    
                    location.href = '/Home/Index';     
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                PresentClosableBootstrapAlert("#alert_placeholder_login", "danger", "Error!", errorText);

                console.error(thrownError + "\r\n" + xhr.statusText + "\r\n" + xhr.responseText);
            }
        });
    }
});