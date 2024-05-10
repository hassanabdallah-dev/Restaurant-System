/*================================================

* Template Name: Foodkuy - Mobile HTML Template
* Version: 1.0
* Author: HidraTheme 
* Developed By: HidraTheme  
* Author URL: https://themeforest.net/user/hidratheme

NOTE: This is the custom js file for the template

================================================*/
let deferred;
window.addEventListener("beforeinstallprompt", (e) => {
    e.preventDefault();
    deferred = e;
    var dialog = bootbox.dialog({
        title: 'Installer l&#39;application',
        message: "<p>Installer l&#39;application pour une bonne experience<br><b>Note:</b> Vous faites rien, l&#39;application s&#39;installe automatiquement.</p>",
        size: 'large',
        buttons: {
            cancel: {
                label: "Non Merci",
                className: 'btn primaryBGColor',
                callback: function () {
                    console.log('Custom cancel clicked');
                }
            },
            ok: {
                label: "Installer!",
                className: 'btn btn-install primaryBGColor',
                callback: function () {
                    console.log('Custom OK clicked');
                    deferred.prompt();
                    deferred.userChoice.then((choiceResult) => {
                        if (choiceResult === "accepted") {
                            console.log("user accepted app install");
                        }
                    });
                }
            }
        }
    });

});
window.addEventListener("appinstalled", function () {
    console.log("a2hs", "installed");
});
$(document).ready(function () {
    $(".btn").addClass("primaryBGColor");
    $(".theme-button").addClass("primaryBGColor");
    $(".theme-button-line").addClass("primaryColor primaryBorder");
    $("ul.slick-dots li button").addClass("primaryBGColor");
    $(".share-button li a").addClass("primaryBGColor");
    $("body .form-control:focus, .add-cart").addClass("primaryBorderColor");
    $(".wrap-pagination .page-item.active .page-link").addClass("primaryBGColor primaryBorderColor");
    $(".preloading .wrap-preload").addClass("primaryBGColor");
    $(".bg-header, .footer, body.preview .here, .cart-total").addClass("primaryBGColor");
    $(".plus, .minus, .footer .socmed .socmed-line .socmed-item, .footer .socmed .socmed-line .socmed-item .si-icon").addClass("primaryColor");
    $(".add-cart:active, .item-number, .section-news-list .home-news-wrap .news-item .news-content .hnw-desc .more").addClass("primaryColor");
    $("body.preview .pr-title, .item-remove-btn, .shopping-cart-item-qty").addClass("primaryColor");
    $("#sidebarright ul .svg-inline--fa").addClass("primaryColor");
    $("#sidebarright .sidebar-header,  body.preview .buy-now .bn").addClass("primaryBGColor");
    $("#sidebarleft ul .svg-inline--fa").addClass("primaryColor");
    $("#sidebarleft ul li .svg-inline--fa").addClass("primaryColor");
    $(".wrap-list-rc .lrc-item .lrc-content .lrc-desc .more, .gallery-section .filter-button").addClass("primaryColor");
    $(".section-home.home-news .home-news-wrap .news-item .news-content .hnw-desc .more").addClass("primaryColor");
    $(".section-subscribe .subcontainer .submitsub").addClass("primaryBGColor primaryBorder");
    $(".section-subscribe .mail-subscribe-box i").addClass("primaryColor primaryBorder");
    $(".section-subscribe .mail-subscribe-box input").addClass("primaryBorder");

});
$(document).bind("contextmenu", function (e) { return false; });
/*$("#sidebarright").on("scroll",function(){
    var t = $(this);
    t.css({
        "bottom":0,
        "top":0,
    })
});*/
$(window).on("navigate", function (event, data) {

    if (data.state.direction == "back") {
        if (window.location.href == window.location.origin + window.location.pathname) {
            $('#sidebarleft').removeClass('active');
            $('#sidebarright').removeClass('active');
            $(".cart-total").hide();
            $('.overlay').removeClass('active');
            $('body').removeClass('noscroll');
        }
    }
    else if (data.state.direction == "forward") {
        if (window.location.href == window.location.origin + window.location.pathname + "#sidebarright") {
            $('.overlay').addClass('active');
            $('body').addClass('noscroll');
            var sidebarright = $('#sidebarright');
            sidebarright.addClass('active');
            if ($(".cart-empty-message").css('display') != 'block')
                $(".cart-total").show(200);

        }
        else if (window.location.href == window.location.origin + window.location.pathname + "#sidebarleft") {
            $('body').addClass('noscroll');
            $('.overlay').addClass('active');
            $('#sidebarleft').addClass('active');
        }
    }

    // reset the content based on the url

});

(function ($) {

    "use strict";

    if ('serviceWorker' in navigator) {
        window.addEventListener('load', () => {
            /*navigator.serviceWorker.register('/sw.js')
                .then(reg => {
                    console.log("registered ", reg);
                }).catch(err => {
                    console.log("registration failed ", err);
                });*/
        });
    }
    $(window).on("back", function () {
        console.log("new");
    });
    /*=================== PRELOADER ===================*/
    $(window).on('load', function () {
        var str = window.location.href;
        str = str.split("#");
        if (str.length == 2) {
            this.window.location.href = str[0];
        }
        $(".preloading").fadeOut("slow");
    });
    /*=================== SIDE NAVIGATION  ===================*/
    $('#dismiss, .overlay').on('click', function () {
        window.history.back(-1);
    });
    $('#sidebarleftbutton,#sidebarrightbutton').on('click', function () {
        $('body').addClass('noscroll');
    });

    $('#sidebarleftbutton').on('click', function () {
        if (window.location.href != window.location.origin + window.location.pathname + "#sidebarleft")
            window.location.href += "#sidebarleft";
        $('.overlay').addClass('active');
        $('#sidebarleft').addClass('active');
    });

    $('#sidebarrightbutton').on('click', function () {
        if (window.location.href != window.location.origin + window.location.pathname + "#sidebarright")
            window.location.href += "#sidebarright";
        console.log(window.location.href);
        var sidebarright = $('#sidebarright');
        sidebarright.addClass('active');
        sidebarright.show();
        if ($(".cart-empty-message").css('display') != 'block')
            $(".cart-total").show(200);
    });

    /*=================== HOMEPAGE - CAROUSEL SLIDER  ===================*/
    $('.img-hero').slick({
        autoplay: true,
        dots: true,
        infinite: true,
        arrows: false,
        speed: 300,
        slidesToShow: 1,
        adaptiveHeight: false
    });


    /*=================== HOMEPAGE - RECIPES YOU MIGHT LIKE CAROUSEL  ===================*/
    $('.yml-carousel').slick({
        slidesToShow: 5,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 2000,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3
                }
            }
        ]
    });


    /*=================== HOMEPAGE - AUTHOR ===================*/
    $('.home-author').slick({
        dots: true,
        infinite: false,
        speed: 300,
        slidesToShow: 6,
        slidesToScroll: 6,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    slidesToShow: 6,
                    slidesToScroll: 6
                }
            },
            {
                breakpoint: 380,
                settings: {
                    slidesToShow: 4,
                    slidesToScroll: 4
                }
            }
        ]
    });


    /*=================== RECIPE PAGE -  SLICK CAROUSEL FOOD IMAGE ===================*/
    $("#food-recipe-image").slick({
        arrows: false,
        dots: true
    });

    $(".awl-btn").on("click", function () {
        $(this).toggleClass("highlight");
    });


    /*=================== GALLERY FILTERING FUCTION  ===================*/
    $(".filter-button").on("click", function () {
        var value = $(this).attr('data-filter');

        if (value == "gallery-no-filter") {
            $('.gallery-img-box').removeClass("gallery-hidden");
        }
        else {
            $(".gallery-img-box").not('.' + value).addClass("gallery-hidden");
            $('.gallery-img-box').filter('.' + value).removeClass("gallery-hidden");

        }
    });

    $('.filter-gallery .filter-button').on("click", function () {
        $('.filter-gallery').find('.filter-button.active').removeClass('active');
        $(this).addClass('active');
    });


    /*=================== MAGNIFICPOPUP GALLERY  ===================*/
    $(".gallery-list").magnificPopup({
        type: "image",
        removalDelay: 300
    });


})(jQuery);


