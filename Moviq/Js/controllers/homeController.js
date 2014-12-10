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
        viewEngine.setView({
            template: 't-empty',
            message: 'hello word!'
        });
    });

    
    routes.get(/^\/#\/addToCart\/?/i, function (context) {
        //alert(2);
        addToCart(context);
    });

    addToCart = function (context) {
        return $.ajax({
            url: '/api/addToCart/?q=' + context.params.id,
            method: 'GET'
        }).done(function (data) {
           // alert("coming up");
        });
    };


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
