/**
 * Created by kuangz on 2017/8/31.
 */
$(function () {
    $("#btnComment").click(function () {
        addBookComment();
    });
    loadBookComment();
    
    checkSession();
    loadCommodityCount();
    addCount();
    loadUBBEditor();

    $("#buyBtn").click(function () {
        var bookId = $("#BookId").text();
        $.ajax({
            url: "/Cart/AddCommodity",
            type: "post",
            data: { "bookId": bookId,"action":"oneAdd" },
            success: function (data) {
                if (data == "ok") {                    
                    loadCommodityCount();
                    alert("已加入购物车");
                } else {
                    alert("加入购物车失败,请稍后再试");
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
function addBookComment() {
    
    var oEditor = CKEDITOR.instances.txtContent;
    var Msg = oEditor.getData();
    var bookId = $("#BookId").text();
    $.ajax({
        url: "/BookComment/AddBookComment",
        type: "post",
        data: { "bookComment": Msg, "bookId": bookId },
        
        success: function (data) {
            var dataArr = data.split(":");
            if (dataArr[0] == "no") {
                alert(dataArr[1]);
            } else {
                $("#commentList").empty();
                
                loadBookComment();
                oEditor.setData("");
                $("#txtContent").focus();
            }
           
        }
    });
}

function loadBookComment(){
    var bookId = $("#BookId").text();
    $.ajax({
        url: "/BookComment/GetBookComment",
        type: "post",
        data: { "bookId": bookId },
        dataType: "json",
        success: function (data) {
            if (data == "no") {

            } else {
                for (var i = 0; i < data.length; i++) {
                    $("<li>" + data[i].CreateDateTime + ":"+data[i].Msg+"</li>").appendTo($("#commentList"));
                }
                
            }
        }
    });
}
function addCount() {
    var bookId = $("#BookId").text();
    
    $.ajax({
        url: "/Books/AddClickCount",
        type: "post",
        data: { "bookId": bookId },
        success: function (data) {
            if (data == "ok") {

            } else {

            }
        }
    });
}

function checkSession() {
    $.ajax({
        url: "/Users/CheckSession",
        type: "get",
        success: function (data) {
            if (data == "no") {
                
            } else {
                $("#loginName").text(data + ",你好").attr("href", "/AddressInfo/Index");
                $("#action").text("退出登录").attr("href","/Users/LoginOut");
            }
        }
    });
}
function loadUBBEditor() {
    CKEDITOR.replace('txtContent',
        {
            extraPlugins: 'bbcode',
            removePlugins: 'bidi,button,dialogadvtab,div,filebrowser,flash,format,forms,horizontalrule,iframe,indent,justify,liststyle,pagebreak,showborders,stylescombo,table,tabletools,templates',
            toolbar:
                [
                    ['Source', '-', 'Save', 'NewPage', '-', 'Undo', 'Redo'],
                    ['Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
                    ['Link', 'Unlink', 'Image'],
                    '/',
                    ['FontSize', 'Bold', 'Italic', 'Underline'],
                    ['NumberedList', 'BulletedList', '-', 'Blockquote'],
                    ['TextColor', '-', 'Smiley', 'SpecialChar', '-', 'Maximize']
                ],
            smiley_images:
                [
                    'regular_smile.gif', 'sad_smile.gif', 'wink_smile.gif', 'teeth_smile.gif', 'tounge_smile.gif',
                    'embaressed_smile.gif', 'omg_smile.gif', 'whatchutalkingabout_smile.gif', 'angel_smile.gif', 'shades_smile.gif',
                    'cry_smile.gif', 'kiss.gif'
                ],
            smiley_descriptions:
                [
                    'smiley', 'sad', 'wink', 'laugh', 'cheeky', 'blush', 'surprise',
                    'indecision', 'angel', 'cool', 'crying', 'kiss'
                ]
        });
}