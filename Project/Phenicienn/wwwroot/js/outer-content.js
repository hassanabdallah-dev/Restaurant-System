﻿function addToCartFromList(id, name, price, count, image, desc, idonly) {
     return '<div id="' + id + '" class="card shopping-cart-card shadow-lg">' +//<!--ID-->
         '<input name="B" type="checkbox" style="display:none;" checked  value="' + parseInt(idonly) + '" />' +
         '<input class="quantityForm" name="A" type="checkbox" style="display:none;" checked  value="' + parseInt(count) + '" />' +
                    '<div class="item-remove-btn item-remove-btn-cart primaryColor">'+
                        '<span class="item-remove-btn-cart">'+
                            '<i class="fa fa-trash"></i>'+
         '</span>' +

                    '</div>'+
                    '<div class="shopping-cart-card-content">'+
                        '<div class="shopping-cart-item-photo">'+
                            '<img src="'+image+'">'+//<!--IMAGE-->
                        '</div>'+
                        '<div class="shopping-cart-item-details">'+
                            '<div class="item-detail">'+
                            '<div class="shopping-cart-item-name">'+name+
                                //'Hors Mixte   <!--NAME-->'+//<!--NAME-->
                                '</div>' +
                                '<div class="item-description text-justify">' + desc+
                                //'Hors Mixte   <!--NAME-->'+//<!--NAME-->
                            '</div>'+
                            '</div>'+
                            '<div class="shopping-cart-item-action">'+
                                '<div class="shopping-cart-item-description">'+
                                    '<span class="shopping-cart-item-price">'+price+'€</span>'+//<!--PRICE-->
                                '</div>'+
                                '<div class="shopping-cart-item-qty primaryColor">'+
                                    '<span class="minus minus-cart"><i class="minus fa fa-minus-square"></i></span>'+
                                    '<b>'+
                                        '<span class="item-quantity-component item-number cart-item-number">'+count+
                                            //'0       <!--COUNT-->'+//<!--COUNT-->
                                        '</span>'+
                                    '</b>'+
                                    '<span class="plus plus-cart"><i class="plus fa fa-plus-square"></i></span>'+
         '</div>' +
                            '</div>'+
                        '</div>'+
                    '</div>'+
         '</div>' +
         '</form>';
}
function loadCart(list) {
    $(".SendButton").empty();
    $(".Comment").empty();
    $(".SendButton").remove();
    $(".Comment").remove();
    for (var i = 0; i < cart_list.length; i++) {
        var item = cart_list[i];
        var matches = item.Id.match(/(\d+)/); 
       // alert(matches[0]);
        list.append(addToCartFromList(item.Id+"-cart", item.Name, item.Price, item.Count, item.Image, item.Description,matches[0]));
    }
    list.append('<div class="SendButton"><div Style="Color:red;"><B>Comment</B><textarea type="text" class="Comment" name="remarque"/></div><div><input class="SendButton btn btn-primary" type="button" onclick="send()" value="send"></input></div></div>');
    if (i == 0) {
        $(".cart-empty-message").show();
        $(".SendButton").empty();
        $(".Comment").empty();
        $(".SendButton").remove();
        $(".Comment").remove();
    }
    else {
        $(".cart-empty-message").hide();
        $(".SendButton").show();
        $(".Comment").show();
    }
}

/*function loadCategory() {
 
        $.ajax({
            url: 'User/Landing/getAllCat',
            dataType: "json",
            method: 'post',
            success: function (data) {

                $(data).each(function (index, emp) {
                    $("#cat-" + emp.CategoryId).click(function () {
                        changeItem(emp.CategoryId);
                    });
                });
            },
            error: function (err) {
                alert(err);
            }
        });
    }*/
        
      /* function changeItem(id) {

        }*/

function send() {
    var str = window.location.href;
    var array = str.split("/");
    var product = array[array.length - 1];
    var remarque = $(".Comment").val();
    var A = [];
    $.each($("input[name='A']:checked"), function () {
        A.push($(this).val());
    });

    var B = [];
    $.each($("input[name='B']:checked"), function () {
        B.push($(this).val());
    });
    id = 1;
    $.ajax({
        url: "/User/Orders/Send",
        method: 'post',
        //dataType: "json",
        data: {
            B:B,
            A: A,
            remarque: remarque,
        },
       
        success: function (data) {
            $(".SendButton").empty();
            $(".Comment").empty();
            $(".SendButton").remove();
            $(".Comment").remove();
            $(".shopping-cart-card").remove();
            
            localStorage.clear(); 
            localStorage.setItem("cart-total-price", 0);
            $(".cart-count-num").html("0");
            $(".cart-total-num").html("0$");
            $(".cart-empty-message").show(100);
           // $(".cart-count-num").hide();
            
            window.history.back(-1);
            loadAllItems();
            
           
        },
        error: function (err) {
            
            console.log(err);
        }
    });
}
$(document).ready(function () {
    var list = $(".shopping-cart-list");
    loadCart(list);
    var sh_cart_list = $(".shopping-cart-list");
    sh_cart_list.on("click", ".item-remove-btn-cart",function (event) {
        var par = $(this).parents(".shopping-cart-card");
        var th = par.attr("id");
        par.hide(100, function () { par.remove(); });
        /*if ($(".shopping-cart-list").children().length == 1)
            $(".cart-empty-message").show();*/
        var id = th.substr(0, th.length - 5);
        th = $("#" + th.substr(0, th.length - 5));
        if (th != null && th.length) {
            th = th.find(".item-remove-btn");
            th.siblings(".inner-card").find(".item-number").html("0");
            removeMainItem(th);
            removeItem(th.parents(".card"));
        }
        else {
            removeItem(null, id);
            var n = $(".cart-count-num");
            if (n.html() == "1") {
                $(".cart-count").hide();
                $(".cart-empty-message").show(100);
                $(".SendButton").empty();
                $(".Comment").empty();
                $(".SendButton").remove();
                $(".Comment").remove();
                $(".cart-total").hide(200);
            }
        }
        event.stopPropagation();
    });
    sh_cart_list.on("click", ".plus-cart", async function (event) {
        var tht = $(this);
        var th = tht.parents(".shopping-cart-card").attr("id");
        var id = th.substr(0, th.length - 5);
        th = $("#" + id);
        await incItem(th,id);
        if (th != null && th.length) {
            incrementNumber(th.find(".plus-main"));
        }
        incrementNumber(tht);
        event.stopPropagation();
    });
    sh_cart_list.on("click", ".minus-cart", async function (event) {
        var tht = $(this);
        var th = tht.parents(".shopping-cart-card");
        var id = th.attr("id");
        id = id.substr(0, id.length - 5);
        var th_id = $("#" + id);
        await decItem(th_id, id);
        var number = decrementNumber(tht);
        if (th_id != null && th_id.length) {
            var minus_main = th_id.find(".minus-main"); 
            decrementNumber(minus_main);
            if (number == 0) {
                removeMainItem(minus_main.parents(".menu-item").find(".item-remove-btn"));
            }
        }
        if (number == 0)
            th.hide(100, function () { th.remove(); });
        event.stopPropagation();
    });

    //loadCategory();
    
});