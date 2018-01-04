$(function () {

    $("#btnUpdatePwd").click(function () {
        var pwd = $("#password").val();
        var cpwd = $("#confirmPwd").val();
        var reg = /^(?![a-zA-z]+$)(?!\d+$)(?![!@#$%^&*]+$)[a-zA-Z\d!@#$%^&*]+$/
        pwd = pwd.trim("");
        cpwd = cpwd.trim("");
        if (pwd == "" || cpwd == "") {
            alert("输入不能为空");
        } else if (pwd.length<6||pwd.length>20) {
            alert("长度只能在6-20个字符之间");
        }else if (!reg.test(pwd)) {
            alert("密码格式错误，有被盗号的风险，请使用字母、数字、符号两种及以上的组合");
        } else if (pwd != cpwd) {
            alert("两次输入不一致");
        } else {
            $.ajax({
                url: "/Users/UpdatePwd",
                type: "post",
                data: $("#UpdatePwdForm").serialize(),
                success: function (data) {
                    var dataArr = data.split(":");
                    if (dataArr[0] == "ok") {
                        $("#myModal").modal('show');
                        var i = 5;
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
                        $("#myModal").modal('show');
                        $("#myModalLabel").text("密码更改失败");
                        $(".modal-body").text(dataArr[1]);
                    }
                }
            });
        }

    });
    $("#locationBtn").click(function () {
        window.location.href = "/jd_BookShop/login.html";
    });
});