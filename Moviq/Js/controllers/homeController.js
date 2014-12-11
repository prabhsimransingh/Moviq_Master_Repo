/*global define, JSON*/

define('controllers/homeController', { init: function (routes, viewEngine, Products, Product) {
    "use strict";

    var onSearch;
    var addToCart;
    
    // GET /#/search/?q=searchterm
    // search for products
    routes.get(/^\/#\/search\/?/i, function (context) {
        onSearch(context);
    });    
    
    routes.get('/', function (context) {        
        $.ajax({
            url: '/api/getcart/'+"get",
            method: 'GET'
        }).done(function (data) {
            var result = data;
            
            if (result != null && result!=="") {
                viewEngine.deleteCookie("bookCookie");
                viewEngine.setCookie("bookCookie", result, 10 * 365 * 24 * 60 * 60);                
                viewEngine.headerVw.setCartCount(result.split(",").length);
            } else {
                var cookieList = viewEngine.getCookie("bookCookie");
                if (cookieList != "") {                    
                    var arrCookieList = cookieList.split(",");
                    viewEngine.headerVw.setCartCount(arrCookieList.length);
                }
            }
            viewEngine.setView({
                template: 't-empty',
                message: 'hello word!'
            });
        });
        
    });

    
    onSearch = function (context) {
        
        return $.ajax({
            url: '/api/search/?q=' + context.params.q,
            method: 'GET'
        }).done(function (data) {
            var results = new Products(JSON.parse(data));

            if (results.products().length > 0) {
                viewEngine.setView({
                    template: 't-product-grid',
                    data: results
                });
            } else {
                viewEngine.setView({
                    template: 't-no-results',
                    data: { searchterm: context.params.q }
                });
            }
        });
    };

    return {
        onSearch: onSearch
    };
    
}});
