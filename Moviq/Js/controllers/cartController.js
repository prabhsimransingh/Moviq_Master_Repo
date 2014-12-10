/*global define, JSON*/

define('controllers/cartController', { init: function (routes, viewEngine, Products, Product,Books, Book) {
    "use strict";

    var addToCart;
    var proceedToCart;
    var removeFromCart;
    var makePayment;
    var isUserLogged;
    // GET /#/cart/?q=searchterm
    // search for products
    routes.get(/^\/#\/addcart\/?/i, function (context) {
        addToCart(context);
    });
    routes.get(/^\/#\/checkout/, function (context) {
        proceedToCart(context);
    });
    routes.get(/^\/#\/remove\/?/i, function (context) {
        removeFromCart(context);
    });
    routes.get(/^\/#\/makepayment/, function (context) {        
        makePayment(context);
    });
    routes.get('/', function (context) {       
    });

    addToCart = function (context) {        
       $.ajax({
            url: '/api/addToCart/?q=' + context.params.id,
            method: 'GET'
       }).done(function (data) {
           alert("data"+JSON.parse(data));

            var cookieList = viewEngine.getCookie("bookCookie");
            var isExists = false;
            if (cookieList != "") {
                var arrCookieList = cookieList.split(",");
                for (var i = 0; i < arrCookieList.length; i++) {
                    if (arrCookieList[i] == context.params.bookid) {
                        isExists = true;
                    }
                }
                if (!isExists) {
                    viewEngine.setCookie("bookCookie", cookieList + "," + context.params.bookid, 10 * 365 * 24 * 60 * 60);
                }
            }
            else {
                viewEngine.setCookie("bookCookie", context.params.bookid, 30);
            }
            $.ajax({
                url: '/api/books/' + context.params.bookid
            }).done(function (data) {
                var book = new Book(JSON.parse(data));
                if (!isExists) {
                    viewEngine.headerVw.addToCart();
                    viewEngine.setView({
                        template: 't-book-added',
                        data: { book: book, message: "1 item added to Cart" }
                    });
                } else {
                    viewEngine.setView({
                        template: 't-book-added',
                        data: { book: book, message: "Item already added in the Cart" }
                    });
                }

            });
        });       
    };

    isUserLogged = function () {
        $.ajax({
            url: '/users/islogged',
            method: 'GET'
        }).done(function (data) {
            var user = JSON.parse(data);
            alert(user);
            return true;
        });
    };

    proceedToCart = function (context) {
        var cookieList = viewEngine.getCookie("bookCookie");
        if (cookieList != "") {
            var arrCookieList = cookieList.split(",");

            var result = [];
            var results;
            var flag = true;
            for (var i = 0; i < arrCookieList.length; i++) {
                $.ajax({
                    url: '/api/cart/' + arrCookieList[i],
                    method: 'GET'
                }).done(function (data) {
                    result.push(JSON.parse(data));
                    results = new Products(result);
                    viewEngine.setView({
                        template: 't-cart-grid',
                        data: results
                    });
                });
            }
        } else {
            viewEngine.setView({
                template: 't-cart-empty'
            });
        }
    };

    removeFromCart = function (context) {        
        var cookieList = viewEngine.getCookie("bookCookie");
        viewEngine.deleteCookie("bookCookie");
        if (cookieList != "") {
            var arrCookieList = cookieList.split(",");
            var result="";
            for (var i = 0; i < arrCookieList.length; i++) {
                if (arrCookieList[i] != context.params.uid) {
                    result = result + arrCookieList[i] + ",";
                }
            }            
            result = result.substring(0, result.lastIndexOf(","));
            viewEngine.headerVw.subtractFromCart();
            viewEngine.setCookie("bookCookie", result, 10 * 365 * 24 * 60 * 60);
            proceedToCart(context);
        }        
    };
    makePayment = function (context) {      
        
    };
   return {
       addToCart: addToCart
   };
}});