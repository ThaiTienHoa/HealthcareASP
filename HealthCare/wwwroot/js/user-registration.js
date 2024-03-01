$(function () {
    $("#UserRegisterForm input[name='phoneNumber']").blur(function () {

        var phoneNumber = $("#UserRegisterForm input[name='phoneNumber']").val();

        if (phoneNumber) {
            var url = "/UserAuth/UserNameExists?userName=" + phoneNumber;

            $.ajax({
                type: "GET",
                url: url,
                success: function (data) {
                    if (data == true) {

                        PresentClosableBootstrapAlert("#alert_placeholder_register", "warning", "Số điện thoại không hợp lệ", "Số điện thoại này đã được sử dụng");

                    }
                    else {
                        CloseAlert("#alert_placeholder_register");
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                    PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);

                    console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);

                }
            });
        }

    });


    var registerUserButton = $("#UserRegisterForm button[name='register']").click(onUserRegisterClick);

    function onUserRegisterClick() {
        var url = "/UserAuth/Register";

        var antiForgeryToken = $("#UserRegisterForm input[name='__RequestVerificationToken']").val();
        var firstName = $("#UserRegisterForm input[name='firstName']").val();
        var lastName = $("#UserRegisterForm input[name='lastName']").val();
        var phoneNumber = $("#UserRegisterForm input[name='phoneNumber']").val();
        var password = $("#UserRegisterForm input[name='password']").val();
        var confirmPassword = $("#UserRegisterForm input[name='confirmPassword']").val();

        var user = {
            __RequestVerificationToken: antiForgeryToken,
            firstName: firstName,
            lastName: lastName,
            phoneNumber: phoneNumber,
            password: password,
            confirmPassword: confirmPassword
        };

        $.ajax({
            type: "POST",
            url: url,
            data: user,
            success: function (data) {

                var parsed = $.parseHTML(data);

                var hasErrors = $(parsed).find("input[name='registrationInValid']").val() == 'true';

                if (hasErrors) {

                    $("#RegisterPartial").html(data);
                    var registerUserButton = $("#UserRegisterForm button[name='register']").click(onUserRegisterClick);

                    var form = $("#UserLoginForm");
                    $(form).removeData("validator");
                    $(form).removeData("unobtrusiveValidation");
                    $.validator.unobtrusive.parse(form);
                }
                else {
                    location.href = '/Home/Index';
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                var errorText = "Status: " + xhr.status + " - " + xhr.statusText;

                PresentClosableBootstrapAlert("#alert_placeholder_register", "danger", "Error!", errorText);

                console.error(thrownError + '\r\n' + xhr.statusText + '\r\n' + xhr.responseText);
            }

        });

    }

});