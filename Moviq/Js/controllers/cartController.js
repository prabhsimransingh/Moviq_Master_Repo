﻿/*global define, JSON*/

define('controllers/cartController', { init: function (routes, viewEngine, Products, Product,Books, Book) {
    "use strict";

    var addToCart;
    var proceedToCart;
    var removeFromCart;
    var makePayment;    
    var afterAddRedirect;
    var emptyCart;
    // GET /#/cart/?q=searchterm
    // search for products
    routes.get(/^\/#\/addcart\/?/i, function (context) {
        addToCart(context);
    });
    routes.get(/^\/#\/checkout/, function (context) {
        proceedToCart(context);
    });
    routes.get(/^\/#\/emptycart/, function (context) {
        emptyCart(context);
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
        var cookieList = viewEngine.getCookie("bookCookie");
        var isExists = false;
        if (cookieList != "") {
            var arrCookieList = cookieList.split(",");
            for (var i = 0; i < arrCookieList.length; i++) {
                if (arrCookieList[i] == context.params.bookid) {
                    isExists = true;
                }
            }
            if (isExists) {
                afterAddRedirect(isExists, context.params.bookid);
                return;                
            } 
        }       
       $.ajax({
           url: '/api/addToCart/?q=' + context.params.bookid,
            method: 'GET'
       }).done(function (data) {
           if (cookieList != null && cookieList!=="") {
               viewEngine.setCookie("bookCookie", cookieList + "," + context.params.bookid, 10 * 365 * 24 * 60 * 60);
           } else {               
               viewEngine.setCookie("bookCookie", context.params.bookid, 30);             
           }
           afterAddRedirect(false, context.params.bookid);
        });           
    };

    afterAddRedirect = function (isExists,bookid) {
        $.ajax({
            url: '/api/books/' + bookid
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
    };

    proceedToCart = function (context) {        
        var cookieList = viewEngine.getCookie("bookCookie");
        if (cookieList != null && cookieList !== "") {
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
        alert("remove");
        $.ajax({
            url: '/api/removecart/' + context.params.uid,
            method: 'GET'
        }).done(function (data) {
            var result = data;
            var item = context.params.uid;
            var cookieList = viewEngine.getCookie("bookCookie");
            viewEngine.deleteCookie("bookCookie");
            if (cookieList != "") {
                var arrCookieList = cookieList.split(",");
                var result = "";
                for (var i = 0; i < arrCookieList.length; i++) {
                    if (arrCookieList[i] != item) {
                        result = result + arrCookieList[i] + ",";
                    }
                }
                result = result.substring(0, result.lastIndexOf(","));
                viewEngine.headerVw.subtractFromCart();
                viewEngine.setCookie("bookCookie", result, 10 * 365 * 24 * 60 * 60);
                proceedToCart(context);
            }           
        });
                
    };
    emptyCart = function (context) {        
        $.ajax({
            url: '/api/emptycart/'+"remove",
            method: 'GET'
        }).done(function (data) {            
            viewEngine.deleteCookie("bookCookie");
            viewEngine.headerVw.setCartCount('');
            proceedToCart(context);
        });
    };
   return {
       addToCart: addToCart
   };
}});