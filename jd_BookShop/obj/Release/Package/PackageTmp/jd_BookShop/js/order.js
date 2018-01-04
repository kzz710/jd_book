$(function () {
    loadCommodityCount();
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