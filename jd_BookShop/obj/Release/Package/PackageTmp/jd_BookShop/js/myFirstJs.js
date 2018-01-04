/**
 * Created by kuangz on 2017/8/22.
 */
$(function () {
    checkSession();
    loadCommodityCount();
    
   $(".webnav").hover(
       function () {
           $(".webnav-msg").show();
       },
       function () {
           $(".webnav-msg").hide();
       }
   );
   $(".closeNav").click(function () {
       $(".site-img").hide();
   });


   function myFirstFuc() {
       var index=0;
       var banner=$(".banner");
       var bannerImg=$(".banner-img img");
       var pointLi=$(".img-point li");
       var imgObj={
           imgArr:["image/book1.png","image/book2.png","image/book3.png","image/book4.png","image/book5.png","image/book6.png"],
           bgcArr:["rgb(24, 80, 97)","rgb(52, 152, 219)","rgb(58, 221, 236)","rgb(180, 242, 253)","rgb(249, 190, 0)","rgb(240, 236, 147)"]
       }
       var timer=null;
       return {
           index:index,
           banner:banner,
           bannerImg:bannerImg,
           pointLi:pointLi,
           imgObj:imgObj,
           timer:timer
       }
   }

   var mFF=myFirstFuc();

   function changImg() {
       mFF.banner.css({
           "background":mFF.imgObj.bgcArr[mFF.index],
           "transition":"all 1s"
       });
       mFF.pointLi.removeClass("current");
       mFF.pointLi[mFF.index].className="current";
       mFF.bannerImg.attr("src",mFF.imgObj.imgArr[mFF.index]);
   }
   $(".arrow-right").click(function () {
       mFF.index++;
       mFF.index=mFF.index>=6?0:mFF.index;
       changImg();
   });
   $(".arrow-left").click(function () {
       mFF.index--;
       mFF.index=mFF.index<0?5:mFF.index;
       changImg();
   });
    mFF.timer=setInterval(function () {
       mFF.index++;
       mFF.index=mFF.index>=6?0:mFF.index;
       changImg();
   },3000);
   $(".banner-img").hover(
       function () {
           clearInterval(mFF.timer);
       },
       function () {
           mFF.timer=setInterval(function () {
               mFF.index++;
               mFF.index=mFF.index>=6?0:mFF.index;
               changImg();
           },3000);
   });

    mFF.pointLi.mouseenter(function () {
      $(this).addClass("current").siblings().removeClass("current");
      mFF.index=mFF.pointLi.index($(this));
      changImg();
   });

    function mySecondFun() {
        var todayMsg=$(".today-msg");
        var index=0;
        return {
            todayMsg:todayMsg,
            index:index
        }
    }
    var mSF=mySecondFun();
    function changMsg() {

        mSF.index++;
        mSF.todayMsg.animate({
            top:-mSF.index*60
        },300,function () {
            if(mSF.index>=2){
                mSF.index=0;
                mSF.todayMsg.animate({
                    top:-mSF.index*60
                },0);
            }
        });
    }
    setInterval(function () {
        changMsg();
    },2000);

    
    
    $(".book-tabs li").mouseenter(function () {
        $(this).addClass("active").siblings().removeClass("active");
        var href=$(this).children("a").attr("href");
        href=href.substring(1);
        $(".tab-content div").each(function (index,ele) {
            if($(ele).attr("id")==href){
               $(ele).addClass("active").siblings().removeClass("active");
           }
        });
    });
    
    $(".book-content-pane li").mouseenter(function () {
        $(this).addClass("active").siblings().removeClass("active");
    });


    translateX($(".today-bottom-content img"));
    translateX($(".book-tese .top"),"img");
    translateX($(".book-tese .bottom"),"img");
    translateX($(".book-tese-pannel div:gt(0)"),"img");
    translateX($(".tongshuwenxue .shu img"));
    translateX($(".tongshuwenxue .bottom img"));
    function translateX(ele1,ele2) {
        if(arguments.length==1){
            ele1.hover(
                function () {
                    $(this).css({
                        transform:"translateX(-10px)",
                        transition:"all 0.5s"
                    });
                },
                function () {
                    $(this).css({
                        transform:"translateX(0px)",
                        transition:"all 0.5s"
                    });
                });
        }else if(arguments.length==2) {
            ele1.hover(
                function () {
                    $(this).find(ele2).css({
                        transform:"translateX(-10px)",
                        transition:"all 0.5s"
                    });
                },
                function () {
                    $(this).find(ele2).css({
                        transform:"translateX(0px)",
                        transition:"all 0.5s"
                    });
                });
        }

    }
    $(".find-good-book .content ul li").hover(function () {
        
        $(this).css({
            padding:"15px 0",
            transition:"all 0.5s"
        });
    },function () {
        $(this).css({
            padding:"20px 0",
            transition:"all 0.5s"
        });
    });
    function changContent() {
        $(".editor-recommended-content").each(function (index,ele) {
            if($(ele).hasClass("active")){
                $(ele).removeClass("active");
            }else {
                $(ele).addClass("active");
            }
        });
    }
    $(".editor .arrow-left").click(function () {
        changContent();
    });

    $(".editor .arrow-right").click(function () {
        changContent();
    });

    $(".editor-bottom li").mouseenter(function () {
       $(this).css({
           width:"410px",
           transition:"all 1s"
       }).siblings().css({
           width:"200px",
           transition:"all 1s"
       });
       $(this).find(".p-btn").fadeIn();
       $(this).siblings().find(".p-btn").fadeOut();
       $(this).find(".p-img").css({
           top:"20px",
           right:0,
           transition:"all 1s"
       }).find("img").css({
           width:"204px",
           height:"204px",
           transition:"all 1s"
       });
       $(this).siblings().find(".p-img").css({
           top:"100px",
           right:"15px",
           transition:"all 1s"
       }).find("img").css({
           width:"124px",
           height:"124px",
           transition:"all 1s"
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

function checkSession() {
    $.ajax({
        url: "/Users/CheckSession",
        type: "get",
        success: function (data) {
            if (data == "no") {

            } else {
                $("#loginName").text(data + ",你好").attr("href", "/AddressInfo/Index");
                $("#action").text("退出登录").attr("href", "/Users/LoginOut");
            }
        }
    });
}