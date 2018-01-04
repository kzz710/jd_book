$(function () {
    loadCommodityCount();
    $("#saveBtn").click(function () {
        var name = $("#Name").val().trim("");
        var zipCode = $("#ZipCode").val().trim("");
        var telphone = $("#Telphone").val().trim("");
        var address = $("#Address").val().trim("");
        var userId = $("#UserId").val();
        var address = $("#Address").val().trim("");
        if (name==""||zipCode==""||telphone==""||address==""||userId==""||address=="") {
            alert("所有选项都为必填项，不能为空");
        } else {
            $.ajax({
                url: "/AddressInfo/AddAddressInfo",
                type: "post",
                data: $("#AddressForm").serialize(),
                success: function (data) {
                    if (data == "ok") {
                        window.location.reload();
                    } else {
                        alert("操作失败");
                    }
                }
            });
        }
    });

    $("#update").click(function () {
        var id = $(this).parent().parent().find(".addressId").val();
        var name = $(this).parent().parent().find(".addressName").text();
        var tel = $(this).parent().parent().find(".addressTel").text();
        var zc = $(this).parent().parent().find(".addressZC").text();
        var address = $(this).parent().parent().find(".address-Info").text();
        $("#Id").val(id);
        $("#ZipCode").val(zc);
        $("#Name").val(name);
        $("#Telphone").val(tel);
        $("#Address").val(address);
    });

    $("#delete").click(function () {
        if (confirm("确定要删除该收货地址吗？")) {
            var id = $(this).parent().parent().find(".addressId").val();
            $.ajax({
                url: "/AddressInfo/DeleteAddressInfo",
                data: { "Id": id },
                type: "post",
                success: function (data) {
                    if (data == "ok") {
                        window.location.reload();
                    } else {
                        alert("删除失败");
                    }
                }
            });
        }
        
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