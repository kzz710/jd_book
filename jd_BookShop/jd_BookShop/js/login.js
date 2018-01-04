/**
 * Created by kuangzz on 2017/9/5.
 */
$(function () {
    $("#saoma").click(function () {
        $(".content-saoma").show();
        $(".content-zhanghu").hide();
        $(this).addClass("active");
        $("#zhanghu").removeClass("active");
    });
    $("#zhanghu").click(function () {
        $(".content-zhanghu").show();
        $(".content-saoma").hide();
        $(this).addClass("active");
        $("#saoma").removeClass("active");
    });
    $(".maImg").mouseenter(function () {
        $(this).animate({
            left:"20px",
            marginLeft:0
        },300,function () {
            $(".helpImg").show();
        })
    });
    $(".maImg").mouseleave(function () {
        $(".helpImg").hide();
        $(this).animate({
            left:"50%",
            marginLeft:"-73.5px"
        },300)
    });
    //登录
    $("#btnLogin").click(function () {
        var loginName=$("#loginName").val();
        var loginPwd=$("#loginPwd").val();
        loginName=loginName.trim("");
        loginPwd=loginPwd.trim("");
        if(loginName==""&&loginPwd==""){
            $(".errorMsg .text").text("请输入账户名和密码");
            $(".errorMsg").show();
            $("#loginName").focus();

        } else if(loginName==""){
            $(".errorMsg .text").text("请输入账户名");
            $(".errorMsg").show();
            $("#loginName").focus();
        } else if(loginPwd==""){
            $(".errorMsg .text").text("请输入密码");
            $(".errorMsg").show();
            $("#loginPwd").focus();
        }else {
            $.ajax({
                url: "/Users/UserLogin",
                type: "post",
                data: $("#loginForm").serialize(),
                success: function (data) {
                    var dataArr=data.split(":");
                    if (dataArr[0] == "ok") {
                        window.location.href = "/Product/Index";
                    } else {
                        $(".errorMsg .text").text(dataArr[1]);
                        $(".errorMsg").show();
                        $("#loginPwd").focus();
                        $("#loginPwd").val("");
                    }
                }
            });
        }

    });
});