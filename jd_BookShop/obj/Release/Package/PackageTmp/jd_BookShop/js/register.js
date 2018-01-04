$(function () {
    var boolUName = false;
    var boolPwd = false;
    var boolCPwd = false;
    var boolTelphone = false;
    var boolMail = false;
    var boolVCode = false
    
    changeStates($("#userName"));
    $("#userName").blur(function () {
        CheckUName();
    });
    function CheckUName() {
        $("#userName").siblings(".info").hide();
        var value = $("#userName").val();
        value = value.trim("");
        if (value == "") {
            $("#userName").val("");
            $("#userName").attr("placeholder", "您的账户名和登录名");
        } else if (value.length < 4 || value.length > 20) {
            $("#userName").siblings(".error").show().find("span").text("长度只能在4-20个字符之间");
            $("#userName").parent().css("border", "1px solid red");
            boolUName = false;
        } else {
            $.post("/Users/CheckUserInfo", { "action": "CheckUserName", "UserName": value }, function (data) {
                if (data == "ok") {
                    $("#userName").siblings(".status").show();
                    $("#userName").siblings(".error").hide();
                    $("#userName").parent().css("border", "1px solid #ddd");
                    boolUName = true;
                } else {
                    $("#userName").siblings(".status").hide();
                    $("#userName").siblings(".error").show().find("span").text("此用户已经存在");
                    $("#userName").parent().css("border", "1px solid red");
                    boolUName = false;
                    
                }
            });
        }
    }
    changeStates($("#password"));
    $("#password").blur(function () {
        CheckPwd();
    });
    function CheckPwd() {
        $("#password").siblings(".info").hide();
        var value = $("#password").val();       
        var reg = /^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&*]+$)[a-zA-Z\d!@#$%^&*]+$/
        if (value == "") {
            $("#password").attr("placeholder", "建议至少使用两种字符组成");
        } else if (value.length < 6 || value.length > 20) {
            $("#password").siblings(".error").show().find("span").text("长度只能在6-20个字符之间");
            $("#password").parent().css("border", "1px solid red");
            boolPwd = false;
            
        } else if (!reg.test(value)) {
            $("#password").siblings(".error").show().find("span").text("有被盗号的风险，建议使用字母、数字、符号两种及以上的组合");
            $("#password").parent().css("border", "1px solid red");
            boolPwd = false;
            
        } else {
            $("#password").siblings(".status").show();
            $("#password").parent().css("border", "1px solid #ddd");
            boolPwd = true;
        }
    }
    changeStates($("#confirmPwd"));
    $("#confirmPwd").blur(function () {
        CheckCPwd();
    });
    function CheckCPwd() {
        $("#confirmPwd").siblings(".info").hide();
        var value = $("#confirmPwd").val();
        if (value == "") {
            $("#confirmPwd").attr("placeholder", "请再次输入密码");
           
        } else if ($("#confirmPwd").val() != $("#password").val()) {
            $("#confirmPwd").siblings(".error").show().find("span").text("两次密码不一致");
            $("#confirmPwd").parent().css("border", "1px solid red");
            boolCPwd = false;
        } else {
            $("#confirmPwd").siblings(".status").show();
            $("#confirmPwd").parent().css("border", "1px solid #ddd");
            boolCPwd = true;
        }
    }
    changeStates($("#telphone"));
    $("#telphone").blur(function () {
        CheckTelphone();
    });
    function CheckTelphone() {
        $("#telphone").siblings(".info").hide();
        var value = $("#telphone").val();
        var reg = /^1[34578]\d{9}$/;
        if (value == "") {
            $("#telphone").attr("placeholder", "建议使用常用手机");
        } else if (!reg.test(value)) {
            $("#telphone").siblings(".error").show().find("span").text("格式错误");
            $("#telphone").parent().css("border", "1px solid red");
            boolTelphone = false;
        } else {
            $.post("/Users/CheckUserInfo", { "action": "CheckTelphone", "Telphone": value }, function (data) {
                if (data == "ok") {
                    $("#telphone").siblings(".status").show();
                    $("#telphone").siblings(".error").hide();
                    $("#telphone").parent().css("border", "1px solid #ddd");
                    boolTelphone = true;
                } else {
                    $("#telphone").siblings(".status").hide();
                    $("#telphone").siblings(".error").show().find("span").text("此手机号码已经注册");
                    $("#telphone").parent().css("border", "1px solid red");
                    boolTelphone = false;
                }
            });
        }
    }
    changeStates($("#email"));
    $("#email").blur(function () {
        CheckEmail();
    });

    function CheckEmail() {
        $("#email").siblings(".info").hide();
        var value = $("#email").val();
        var reg = /^[A-Za-zd]+([-_.][A-Za-zd]+)*@([+@([a-zA-Z0-9_-])+(.[a-zA-Z0-9_-])+/;
        if (value == "") {
            $("#email").attr("placeholder", "建议使用常用邮箱");
        } else if (!reg.test(value)) {
            $("#email").siblings(".error").show().find("span").text("格式错误");
            $("#email").parent().css("border", "1px solid red");
            boolMail = false;
        } else {
            $.post("/Users/CheckUserInfo", { "action": "CheckEmail", "Email": value }, function (data) {
                if (data == "ok") {
                    $("#email").siblings(".status").show();
                    $("#email").siblings(".error").hide();
                    $("#email").parent().css("border", "1px solid #ddd");
                    boolMail = true;
                } else {
                    $("#email").siblings(".status").hide();
                    $("#email").siblings(".error").show().find("span").text("此邮箱已经注册");
                    $("#email").parent().css("border", "1px solid red");
                    boolMail = false;
                }
            });
        }
    }
    changeStates($("#validateCode"));
    $("#validateCode").blur(function () {
        var value = $("#validateCode").val();
        if (value == "") {
            $("#validateCode").attr("placeholder", "请输入验证码");
        }
        $("#validateCode").siblings(".info").hide();

    });
    
    function CheckVCode() {
        $("#validateCode").siblings(".info").hide();
        var value = $("#validateCode").val();
        if (value == "") {
            $("#validateCode").siblings(".error").show();
        } else {
            $.ajax({
                url: "/Users/CheckUserInfo",
                async: false,
                type: "post",
                data: { "action": "CheckValidateCode", "ValidateCode": value },
                success: function (data) {
                    if (data == "ok") {
                        $("#validateCode").siblings(".error").hide();
                        $("#validateCode").parent().css("border", "1px solid #ddd");
                        boolVCode = true;                      
                    } else {
                        $("#validateCode").siblings(".error").show().find("span").text("验证码错误");
                        $("#validateCode").parent().css("border", "1px solid red");
                        boolVCode = false;
                    }
                }
            });
            //$.post("/Users/CheckUserInfo", { "action": "CheckValidateCode", "ValidateCode": value }, function (data) {
            //    if (data == "ok") {
                    
            //        $("#validateCode").siblings(".error").hide();
            //        $("#validateCode").parent().css("border", "1px solid #ddd");
            //        boolVCode = true;
                    
            //        console.log(boolVCode);
            //    } else {                  
            //        $("#validateCode").siblings(".error").show().find("span").text("验证码错误");
            //        $("#validateCode").parent().css("border", "1px solid red");
            //        boolVCode = false;
            //    }
            //});
        }
    }
    $("#codeImg").click(function () {
        $(this).attr("src",$(this).attr("src")+1);
    });
    
    $("#telCode").focus(function () {
        if ($(this).val() == "") {
            $(this).attr("placeholder", "");
        } 
    });
    $("#telCode").blur(function () {
        if ($(this).val() == "") {
            $(this).attr("placeholder", "请输入手机验证码");
        }
    });
    $("#getTelCode").click(function () {
        var str = "";
        for (var i = 0; i < 6; i++) {
            var num = Math.floor(Math.random() * 10);
            str += num;
        }
        $("#telCode").val(str);
        $("#telCode").siblings(".error").hide().siblings(".status").hide();
        $("#telCode").parent().css("border", "1px solid #ddd");
    });

    $("#RegisterBtn").click(function () {
        
        if ($("#userName").val() == "") {
            btnCheckIsNull($("#userName"));
            return false;
        }
        if ($("#password").val() == "") {
            btnCheckIsNull($("#password"));
            return false;
        }
        if ($("#confirmPwd").val() == "") {
            btnCheckIsNull($("#confirmPwd"));
            return false;
        }
        if ($("#telphone").val() == "") {
            btnCheckIsNull($("#telphone"));
            return false;
        }
        if ($("#email").val() == "") {
            btnCheckIsNull($("#email"));
            return false;
        }
        if ($("#validateCode").val() == "") {
            btnCheckIsNull($("#validateCode"));
            return false;
        }
        if ($("#telCode").val() == "") {
            btnCheckIsNull($("#telCode"));
            return false;
        }

        //CheckUName();
        //CheckPwd();
        //CheckCPwd();
        //CheckTelphone();
        //CheckEmail();
        CheckVCode();
        if (!boolUName) {
            $("#userName").focus();
        }
        if (!boolCPwd) {
            $("#confirmPwd").focus();
        }
        if (!boolMail) {
            $("#email").focus();
        }
        if (!boolPwd) {
            $("#password").focus();
        }
        if (!boolTelphone) {
            $("#telphone").focus();
        }
        if (!boolVCode) {
            $("#validateCode").focus();
        }
        
        if (boolUName && boolCPwd && boolMail && boolPwd && boolTelphone && boolVCode) {
            $.ajax({
                url: "/Register/UserRegister",
                type: "post",
                cache: false,
                data: $("#mainForm").serialize(),
                success: function (data) {
                    
                    var dataArr = data.split(":");
                    console.log(dataArr);
                    if (dataArr[0] == "ok") {
                        $("#myModal").modal('show');
                        var i = $("#time").text();
                        var timer = setInterval(function () {
                            i--;
                            if (i == 0) {
                                clearInterval(timer);
                                window.location.href = "/jd_BookShop/login.html";
                                return false;
                            }

                            $("#time").text(i);
                        }, 1000);
                    } else {
                        if (dataArr[1] == "userName") {
                            CheckUName();
                        }
                        if (dataArr[1] == "password") {
                            CheckCPwd();
                        }
                        if (dataArr[1] == "telphone") {
                            CheckTelphone();
                        }
                        if (dataArr[1] == "email") {
                            CheckEmail();
                        }
                    }
                }
            });
        }


    });

    $("#locationBtn").click(function () {
        window.location.href = "/jd_BookShop/index.html";
    });
    
});

function changeStates(ele) {
    ele.focus(function () {
        if ($(this).val() == "") {
            $(this).attr("placeholder", "").siblings(".info").show();
        } else {
            $(this).attr("placeholder", "").siblings(".info").hide();
        }

    });
    ele.keydown(function () {
        $(this).siblings(".info").show().siblings(".error").hide().siblings(".status").hide();
        $(this).parent().css("border", "1px solid #ddd");
    });
}

function btnCheckIsNull(ele) {
    ele.parent().css("border", "1px solid red");
    ele.focus();
    ele.siblings(".error").show();
    ele.attr("placeholder", "").siblings(".info").hide();
}