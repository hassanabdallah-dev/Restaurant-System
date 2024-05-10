
const asyncLocalStorage = {
    setItem: async function (key, value) {
        await null;
        return localStorage.setItem(key, value);
    },
    getItem: async function (key) {
        await null;
        return localStorage.getItem(key);
    }
};
function changeNumber(number, object) {
    var numberElement = object.parent().find(".item-number");
    var card = object.parents(".card");
    var quantity = card.find(".quantityForm");
    var temp = parseInt(numberElement.html()) + number;
    if (temp >= 0 && temp < 100) {
        numberElement.html(temp);
        quantity.val(temp);
        return temp;
    }
    return -1;
}
function incrementNumber(object) {
    return changeNumber(1, object);
}
function decrementNumber(object) {
    return changeNumber(-1, object);
}
function removeMainItem(object) {
    var n = $(".cart-count-num");
    var x = parseInt(n.html());
    n.html(x - 1);
    if (x == 1) {
        $(".cart-count").hide();
        $(".cart-empty-message").show(100);
        $(".SendButton").empty();
        $(".Comment").empty();
        $(".SendButton").remove();
        $(".Comment").remove();


        $(".cart-total").hide(200);
    }
    var container = object.siblings(".inner-card").find(".item-quantity-container");
    container.find(".btn-add").show(200);
    object.hide(200);
    container.find(".item-quantity").css("display", "none");
}
function addItem(card, object, num) {
    if (card != null || object != null) {
        var n = $(".cart-count-num");
        var x = parseInt(n.html());
        n.html(x + 1);
        if (x == 0) {
            $(".cart-count").show();
            $(".cart-empty-message").hide();
            $(".SendButton").show();
            $(".Comment").show();
        }
    }
    var cardd, btn;
    if (num == false)
        num = 1;
    if (card == null) {
        cardd = object.parents(".card");
        btn = object;
    }
    else {
        cardd = card;
        btn = cardd.find(".btn-add");
    }
    btn.hide(200);
    cardd.find(".item-remove-btn").show(200);
    var qty = btn.siblings(".item-quantity");
    qty.find(".item-number").html(num);
    qty.css("display", "flex");
}
async function saveToLocal(object) {
    var id = "cart-"+object.Id;
    var stringified = JSON.stringify(object);
    await asyncLocalStorage.setItem(id, stringified);
}
function getFromLocal(Id, bln) {
    var key;
    if (bln == false) {
        key = Id;
    }
    else {
        key = "cart-" + Id;
    }
    var ob = JSON.parse(localStorage.getItem(key));
    Object.assign(new CartItem, ob);
    Object.setPrototypeOf(ob, CartItem.prototype);
    return ob;
}
async function updateCount(Id, count, op=0) {
    var old = getFromLocal(Id,true);
    if (op == 1)
        count = old.Count - 1;
    else if (op == 2)
        count = old.Count + 1;
    var temp = parseFloat(localStorage.getItem("cart-total-price"))-(old.Count*old.Price);
    if (count <= 0) {
        localStorage.removeItem("cart-" + Id);
        temp = temp.toString();
    }
    else {
        old.Count = count;
        await saveToLocal(old);
        temp = (temp + (old.Price * count)).toString();
    }
    localStorage.setItem("cart-total-price", temp);
    $(".cart-total-num").html(temp + "$");
}
async function addToCart(id, name, price, image) {
    var newitem = new CartItem();
    newitem.Id = id;
    newitem.Count = 1;
    newitem.Name = name;
    newitem.Price = price;
    newitem.Image = image;
    await saveToLocal(newitem);
    var initial = localStorage.getItem("cart-total-price");
    if (initial == "0" || (initial != null && initial != false && initial != "")) {
        initial = (parseFloat(initial) + parseFloat(price)).toString();
    }
    else 
        initial = price.toString();
    localStorage.setItem("cart-total-price", initial);
    $(".cart-total-num").html(initial + "$");
}
function getAllItems() {
    var i, results = [];
    for (i = 0; i < localStorage.length;i++) {
        var key = localStorage.key(i);
        if (key == "cart-undefined")
            localStorage.removeItem(key);
        else if (key != "cart-total-price" && key.startsWith("cart-")) 
            results.push(getFromLocal(key, false));
    }
    return results;
}
async function addItemToCart(card,cart_list_view) {
    var id = card.attr('id');
    var name = card.find(".item-name").html();
    var image = card.find(".item-photo").attr('src');
    var price = card.find(".item-pprice").html();
    var desc = card.find(".item-description").html();
    price = price.substr(0, price.length - 1);
    await addToCart(id, name, price, image);
    var matches = id.match(/(\d+)/); 
    $(".SendButton").empty();
    $(".Comment").empty();
    $(".SendButton").remove();
    $(".Comment").remove();
    cart_list_view.append(addToCartFromList(id + "-cart", name, price, 1, image, desc, matches));
    cart_list_view.append('<div class="SendButton"><div Style="Color:red;"><B>Comment</B><textarea type="text" class="Comment" name="remarque"/></div><div> <input class="SendButton btn btn-primary" type="button" onclick="send()" value="send"></input></div></div>');
}
async function incItem(card, id = null) {
    var itemnumber,idd,op=0,num;
    if (card != null && card.length) {
        itemnumber = parseInt(card.find(".item-number").html());
        idd = card.attr('id');
        num = itemnumber + 1;
    }
    else {
        idd = id;
        op = 2;
        num = 0;
    }
    await updateCount(idd, num, op);
}
async function decItem(card, id = null) {
    var itemnumber, idd, op = 0, num;
    if (card != null && card.length) {
        itemnumber = parseInt(card.find(".item-number").html());
        idd = card.attr('id');
        num = itemnumber - 1;
    }
    else {
        idd = id;
        op = 1;
        num = 0;
    }
    await updateCount(idd, num, op);
}
async function removeItem(card, id) {
    var idd;
    if (card != null && card.length)
        idd = card.attr('id');
    else
        idd = id;
    await updateCount(idd, 0);
}
function changeItem(id, img, name, description, price, allergant) {
    res = "";
    if (allergant.allergants.length != 0) {
        res = '<div style="position:relative;" class="primaryColor mt-3 item-description text - justify"><div><p class="primaryColor"><b>Allergant:</b></p></div><div class="inglink" style="cursor:pointer;overflow: hidden;white-space: nowrap;text-overflow: ellipsis;width: 90%;position: absolute;">';
        for (var i = 0; i < allergant.allergants.length; i++) {
            res += allergant.allergants[i]["name"];
            if (i != allergant.allergants.length - 1)
                res += ", ";
        }
        res += '</div></div>';
        console.log(res);
    }
    ing = "";
    var btn = "";
    if (allergant.ingredients.length != 0) {
        for (var i = 0; i < allergant.ingredients.length; i++) {
            ing += allergant.ingredients[i]["name"];
            if (i != allergant.ingredients.length - 1)
                ing += ", ";
        }
        btn = '<button type="button" data-toggle="modal" data-target="#ingredientModal" data-ing="' + ing +'" class="btn btn-link btn-inf primaryColor"><i class="fas fa-info-circle"></i></button>';
    }



   return '<div id="item-'+id+'" class="card shadow-lg menu-item">'+
         '<div class="item-remove-btn item-remove-btn-main primaryColor"><i class="fa fa-trash"></i></div>'+
          '<div class="inner-card">'+
            '<div class="item-main">'+
                '<div class="item-photo-container">'+
                    '<img src="/'+img+'" class="item-photo">'+
                               ' </div>'+
                    '<div class="item-text-container">'+
                        '<div class="item-text">'+
                            '<div class="item-detail">'+
                                '<div class="item-name">'+name+' '+btn+'</div>'+
       '<div class="item-description text-justify">' + description + '</div>' +
                             res+
                            '</div>'+
                            '<div class="item-price">'+
                                '<div class="item-quantity-outer">'+
        '<span style="font-size:17px;" class="item-pprice">'+price+'€</span>' +
            '<div class="item-quantity-container">' +
            '<div class="item-quantity">' +
            '<span class="minus minus-main primaryColor"><i class="minus fa fa-minus-square"></i></span>' +
            '<div class="item-quantity-component item-number primaryColor">0</div>' +
            '<span class="plus plus-main primaryColor"><i class="fa fa-plus-square"></i></span>' +
            '</div>' +
       '<button  class="btn btn-add primaryBGColor">ajouter</button>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>' +
            '</div>';

   
}
var Boolean = 0;

function loadAllItems() {
    $("#all").addClass("active");
    //$("#cat").removeClass("active");
    var str = window.location.href;
    var array = str.split("/");
    var product = array[array.length - 1]
    var path;
    if (product.charAt(0) != "M")
        path = "../getAllItems";
    else
        path = "getAllItems";
    
    $(".menu-cards").empty();
    $.ajax({
        url: path,
        dataType: "json",
        method: 'post',
        success: function (data) {
            getAllergant(data);
            /*var i = 0;
            for (i = 0; i < data.length; i++)
                $(".menu-cards").append(changeItem(data[i]["itemId"], data[i]["imagePath"], data[i]["name"], data[i]["description"], data[i]["price"]));
        */
        },
        error: function (err) {
           // alert(err);
        }
    });
    if (Boolean == 0) {
        Boolean = 1;
        handling();
    }

    setTimeout(function () {
        console.log("getting objects from local storage");
        cart_list = getAllItems();
        $(".cart-total-num").html(localStorage.getItem("cart-total-price") + "$");
        if (cart_list.length == 0)
            $(".cart-count").css("display", "none");
        else
            $(".cart-count").css("display", "block");
        console.log(cart_list);
        for (var counter = 0; counter < cart_list.length; counter++) {
            var card = $("#" + cart_list[counter].Id);
            addItem(card, null, cart_list[counter].Count);

            var n = $(".cart-count-num");
            var x = parseInt(n.html());
            n.html(x - 1);
        }
    }, 500);
}

function getAllergant(data1) {
    var str = window.location.href;
    var array = str.split("/");
    var product = array[array.length - 1]
    var path;
    if (product != "Menu")
        path = "../getAllergantItems";
    else
        path = "getAllergantItems";
    var k;
    var i = 0;
    for (i = 0; i < data1.length; i++) {
        id = data1[i]["itemId"];
        $.ajax({
            url: path,
            dataType: "json",
            data: { "id": id },
            index: i,
            array:data1,
            method: 'post',
            success: function (data) {

         
                $(".menu-cards").append(changeItem(this.array[this.index]["itemId"], this.array[this.index]["imagePath"], this.array[this.index]["name"], this.array[this.index]["description"], this.array[this.index]["price"], data));
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
}
function loadCategory(id) {
    //$("#all").removeClass("active");
    $("#cat-" + id).addClass("active");
    //id="btn-' + id +'" onclick="btnAdd('+id+')"
    var str = window.location.href;
    var array = str.split("/");
    var product = array[array.length - 1]
    var path;
    if (product != "Menu")
        path = "../getCatItems";
    else
        path = "getCatItems";


    $(".menu-cards").empty();
    $.ajax({
        url: path,
        dataType: "json",
        data: { "id": id },
        method: 'post',
        success: function (data) {
            getAllergant(data);
            /* var i = 0;
           for (i = 0; i < data.length; i++) {
                var a = getAllergant(data[i]["itemId"]);
                $(".menu-cards").append(changeItem(data[i]["itemId"], data[i]["imagePath"], data[i]["name"], data[i]["description"], data[i]["price"], a));
            }*/
            },
        error: function (err) {
        }
    });
    if (Boolean == 0) {
        Boolean = 1;
        handling();
    }


 
    setTimeout(function () {
        console.log("getting objects from local storage");
        cart_list = getAllItems();
        $(".cart-total-num").html(localStorage.getItem("cart-total-price") + "$");
        if (cart_list.length == 0)
            $(".cart-count").css("display", "none");
        else
            $(".cart-count").css("display", "block");
        console.log(cart_list);
        for (var counter = 0; counter < cart_list.length; counter++) {
            var card = $("#" + cart_list[counter].Id);
            addItem(card, null, cart_list[counter].Count);

            var n = $(".cart-count-num");
            var x = parseInt(n.html());
            n.html(x - 1);
        } }, 500);
   
    /*$(document).ready(function () {
        initializtion();
    });*/
}

 function testParametreValue() {
      /*const queryString = window.location.search;
      const urlParams = new URLSearchParams(queryString);
      if (urlParams.has('cat')) {
          const product = urlParams.get('cat');
          content = "https://localhost:44376/User/Landing/Menu?cat=" + product;
        //  window.history.back(-1);
          loadCategory(product);
          //window.location.href = content;
      }*/

     var str = window.location.href; 
     var array = str.split("/");
     var product = array[array.length - 1]

     if (product != "" && product != "Menu")
         loadCategory(product);
     else
         loadAllItems();

  }



function handling() {


    $(document).on('click', '.btn-add', function () {
        
        $(this).siblings(".item-quantity").find(".item-number").html("1");
        var th = $(this);
        addItem(null, th, false);
        addItemToCart(th.parents(".card"), $(".shopping-cart-list"));
    });

    $(document).on('click', '.item-remove-btn', function () {
        var th = $(this);
        th.siblings(".inner-card").find(".item-number").html("0");
        removeMainItem(th);
        var card = th.parents(".card");
        $("#" + card.attr("id") + "-cart").remove();
         removeItem(card, null);
    });


    $(document).on('click', '.minus-main', function () {
        var th = $(this);
        var card = th.parents(".card");
        var cart_card = $("#" + card.attr("id") + "-cart");
         decItem(card);
        var number = decrementNumber(th);
        decrementNumber(cart_card.find(".minus-cart"));
        if (number == 0) {
            removeMainItem(th.parents(".menu-item").find(".item-remove-btn"));
            cart_card.remove();
        }
    });

    $(document).on('click', '.plus-main', function () {
        var th = $(this);
        var card = th.parents(".card");
        var plus_cart = $("#" + card.attr("id") + "-cart").find(".plus-cart");
         incItem(card, null);
        incrementNumber(th);
        incrementNumber(plus_cart);
    });

    $(document).on('click', '.nav-item', function () {
        $(".nav-link").removeClass("active");
        $(this).find(".nav-link").addClass("active");
    });
}

function initializtion() {
    console.log("getting objects from local storage");
    cart_list = getAllItems();
    $(".cart-total-num").html(localStorage.getItem("cart-total-price") + "$");
    if (cart_list.length == 0)
        $(".cart-count").css("display", "none");
    else
        $(".cart-count").css("display", "block");
    console.log(cart_list);
    for (var counter = 0; counter < cart_list.length; counter++) {
        var card = $("#" + cart_list[counter].Id);
        addItem(card, null, cart_list[counter].Count);
    }
    $(".btn-add").click(function () {
        $(this).siblings(".item-quantity").find(".item-number").html("1");
        var th = $(this);
        addItem(null, th, false);
        addItemToCart(th.parents(".card"), $(".shopping-cart-list"));
    });
    $(".item-remove-btn").click(async function () {
        var th = $(this);
        th.siblings(".inner-card").find(".item-number").html("0");
        removeMainItem(th);
        var card = th.parents(".card");
        $("#" + card.attr("id") + "-cart").remove();
        await removeItem(card, null);
    });
    $(".minus-main").click(async function () {
        var th = $(this);
        var card = th.parents(".card");
        var cart_card = $("#" + card.attr("id") + "-cart");
        await decItem(card);
        var number = decrementNumber(th);
        decrementNumber(cart_card.find(".minus-cart"));
        if (number == 0) {
            removeMainItem(th.parents(".menu-item").find(".item-remove-btn"));
            cart_card.remove();
        }
    });
    $(".plus-main").click(async function () {
        var th = $(this);
        var card = th.parents(".card");
        var plus_cart = $("#" + card.attr("id") + "-cart").find(".plus-cart");
        await incItem(card, null);
        incrementNumber(th);
        incrementNumber(plus_cart);
    });
    $(".nav-item").click(function () {

        $(".nav-link").removeClass("active");
        $(this).find(".nav-link").addClass("active");
    });
   // testParametreValue();


}

function search() {
    $('#search').bind('keyup', function () {

        var searchString = $(this).val();

        $("ul li").each(function (index, value) {

            currentName = $(value).text()
            if (currentName.toUpperCase().indexOf(searchString.toUpperCase()) > -1) {
                $(value).show();
            } else {
                $(value).hide();
            }

        });

    });
}
$(document).ready(function () {
    initializtion();
    testParametreValue();
    /*const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    if (urlParams.has('cat')) {
        const product = urlParams.get('cat');
        content = "https://localhost:44376/User/Landing/Menu?cat=" + product;
        loadCategory(product);
        //window.location.href = content;
    }*/
    search();
});
