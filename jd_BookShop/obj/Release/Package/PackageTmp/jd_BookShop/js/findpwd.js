$(function () { 
    $("#btnSendEmail").click(function () {
        var userName = $("#userName").val();
        var email = $("#email").val();
        userName = userName.trim("");
        email = email.trim("");
        if (userName == "" || email == "") {
            alert("用户名和邮箱不能为空");
        } else {
            $.ajax({
                url: "/Users/FindPwd",
                type: "post",
                data: $("#findpwdForm").serialize(),
                success: function (data) {
                    var dataArr = data.split(":");

                    $("#myModal").modal('show');
                    $(".modal-body").text(dataArr[1]);
                }
            });
        }
        
    });
});