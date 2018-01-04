/**
 * Created by kuangz on 2017/8/31.
 */
$(function () {
    $.get("/Books/GetBooks", function (data) {
        
        var dataObj = { item: data };
        
        for (var i = 0; i < dataObj.item.length; i++) {
            var s = dataObj.item[i].PublishDate;
            var b = s.indexOf("T");
            var c = s.substring(0, b);
            var a = new Date(Date.parse(c.replace(/-/g, "/")));
            var y = a.getFullYear();
            var m = a.getMonth()+1;
            var d = a.getDate();
            var publishDate = y + "/" + m + "/" + d ;
            dataObj.item[i].PublishDate = publishDate;
        }         
        var str = template("template", dataObj);
        $(".find-good-book .content ul").append(str);
        //for (var i = 0; i < data.length; i++) {
        //    console.log(1);
        //    $("<li>"+
        //                    "<a href='#'>"+
        //                        "<div class='f-img'><img src='image/BookCovers/"+data[i].ISBN+".jpg' alt=''></div>"+
        //                        "<div class='f-title'>"+data[i].Title+"</div>"+
        //                        "<div class='f-des'>"+data[i].ContentDescription+"</div>"+
        //                        "<div class='f-num'>"+
        //                            "<i></i>"+
        //                            "<em>"+data[i].Clicks+"</em>"+
        //                        "</div>"+
        //                    "</a>"+
        //             "</li>").appendTo($(".find-good-book .content ul"));
            
        //}
    }, "json");
        
});