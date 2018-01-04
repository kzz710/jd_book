$(function () {
    loadCommodityCount();
    $("#goBuy").click(function () {
        var ck=$("#orderForm").serialize();
        var ra = $("#addressForm").serialize();
        var isLogin = $(".isLogin").val();
        if (isLogin=="No") {
            alert("你尚未登录，请先登录！！！");
            window.location.href = "/jd_BookShop/login.html";
        }else if (ra=="") {
            alert("请选择收货地址");
        } else if (ck == "") {
            alert("请选择要结算的商品");
        } else {
            var ckArr = ck.split("&");
            var bookIds = [];
            for (var i = 0; i < ckArr.length; i++) {
                var bookId = ckArr[i].split("=")[1];
                bookIds.push(bookId);
            }
            var addressId = ra.split("=")[1];
            var booksId = bookIds.toString();
            //console.log(addressId);
            //console.log(booksId);
            $.ajax({
                url: "/Orders/CreateOrder",
                type: "post",
                data: { "addressId": addressId, "booksId": booksId },
                success: function (data) {
                    if(data=="ok"){
                        window.location.href = "/Orders/Index";
                    } else if (data == "noCheck") {
                        window.location.href = "/Cart/Index";
                    } else {
                        alert("购买失败，请稍后再试!!!");
                    }
                }
            });
        }
        
        
    });

    $(".totalCheckBox").click(function () {        
        $(".oneCheckBox").prop("checked",$(this).prop("checked"));
    });
    $(".oneCheckBox").click(function () {
        if ($(".oneCheckBox").length==$(".oneCheckBox:checked").length) {
            $(".totalCheckBox").prop("checked", true);

        } else {
            $(".totalCheckBox").prop("checked", false);
        }
    });

    $(".deleteBtn").click(function () {
        var bookId = $(this).parent().parent().find($(".oneCheckBox")).val();
        if (confirm("确定要移除该商品吗？")) {
            $.ajax({
                url: "/Cart/DeleteCommodity",
                type: "post",
                data: { "bookId": bookId,"action":"totalDelete" },
                success: function (data) {
                    if(data=="ok"){
                        window.location.reload();
                    } else {
                        alert("删除失败，请稍后再试");
                    }
                }
            });
        } 
        
    });
    $(".numBtn").click(function () {
        var action = $(this).text();
        var input = $(this).parent().find($("input"));
        var xiaoji = $(this).parent().parent().find($(".xiaoji"));
        var onePrice = $(this).parent().parent().find($(".onePrice")).text()*1;
        var totalPrice = $(".totalPrice").text()*1;
        var num = input.val();      
        var bookId = $(this).parent().parent().find($(".oneCheckBox")).val();
        if (action == "+") {           
            if (num>=1000) {
                alert("最多添加1000");
                $(this).parent().find($("input")).val(1000);
            } else {
                $.ajax({
                    url: "/Cart/AddCommodity",
                    type: "post",
                    data: { "bookId": bookId,"action":"oneAdd" },
                    success: function (data) {
                        if (data == "ok") {
                            num++;
                            xiaoji.text((onePrice * num).toFixed(2));
                            input.val(num);
                            $(".totalPrice").text((totalPrice+onePrice).toFixed(2));
                        } else {
                            alert("添加失败");
                        }
                    }
                });
            }
            
        } else {
            if (num<=1) {
                if (confirm("确定要移除该商品吗？")) {
                    $.ajax({
                        url: "/Cart/DeleteCommodity",
                        type: "post",
                        data: { "bookId": bookId ,"action":"totalDelete"},
                        success: function (data) {
                            if (data == "ok") {
                                window.location.reload();
                            } else {
                                alert("删除失败，请稍后再试");
                            }
                        }
                    });
                }
            } else {
                $.ajax({
                    url: "/Cart/DeleteCommodity",
                    type: "post",
                    data: { "bookId": bookId,"action":"oneDelete" },
                    success: function (data) {
                        if (data == "ok") {
                            num--;
                            xiaoji.text((onePrice * num).toFixed(2));
                            input.val(num);
                            $(".totalPrice").text((totalPrice - onePrice).toFixed(2));
                        } else {
                            alert("删除失败，请稍后再试");
                        }
                    }
                });
            }
        }

    });

    $(".commodityCount").blur(function () {
        var bookId = $(this).parent().parent().find($(".oneCheckBox")).val();
        var num = $(this).val();
        var xiaoji = $(this).parent().parent().find($(".xiaoji"));
        var onePrice = $(this).parent().parent().find($(".onePrice")).text() * 1;
        var totalPrice = $(".totalPrice").text() * 1;
        if (num=="") {
            alert("商品数量不能为空");
            $(this).val(1);
        } else if (num<1) {
            alert("商品数量不能小于1");
            $(this).val(1);
        } else if (num > 1000) {
            alert("商品最多添加1000件");
            $(this).val(1000);
        }
        num = $(this).val();
        $.ajax({
            url: "/Cart/AddCommodity",
            type: "post",
            data: { "bookId": bookId ,"action":"totalAdd","count":num},
            success: function (data) {
                if (data == "ok") {
                    var xiaoPrice = xiaoji.text() * 1;
                    $(".totalPrice").text((totalPrice - xiaoPrice + onePrice * num).toFixed(2));
                    xiaoji.text((onePrice * num).toFixed(2));                                     
                } else {
                    alert("添加失败");
                }
            }
        });
        
    });
});

function loadCommodityCount() {
    $.ajax({
        url: "/Cart/GetCommodityCount",
        type: "get",
        success: function (data) {
            $(".badge").text(data);
        }
    });
}