$(function () {
    loadCommodityCount();

    $(".find-good-book .content ul li").hover(function () {

        $(this).css({
            padding: "15px 0",
            transition: "all 0.5s"
        });
    }, function () {
        $(this).css({
            padding: "20px 0",
            transition: "all 0.5s"
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