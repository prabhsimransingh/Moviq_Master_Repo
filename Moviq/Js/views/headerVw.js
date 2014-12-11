/**/

/*global define*/

define('views/headerVw', {
    init: function ($, routes) {
        "use strict";

        var selector = 'form.search-form',
            inputSelector = selector + ' input';

        // add singleton events
        $(document).on('submit', selector, function (event) {
            routes.navigate('/search/?q=' + $(inputSelector).val());
            return false;
        });
        $(document).on('click', '.submit-button', function (event) {
          

            
            $(".payform").validate({
                rules: {
                    "username": {
                        username: true,
                        required: true
                    },
                    "email": {
                        email: true,
                        required: true
                    },
                    "card-cvc": {
                        cardCVC: true,
                        required: true
                    },
                    "card-number": {
                        cardNumber: true,
                        required: true
                    },
                    "card-expiry-year": "cardExpiry" // we don't validate month separately
                }
            });
            if (!$(".payform").valid())
            {
                return false;
            }      
            routes.navigate('/stripe/?totalcost=' + document.getElementById("totalcost").innerHTML);
            return false;
        });
        $(document).on('click', ".anchor", function (event) {
            routes.navigate('/addcart/?bookid=' + this.id);
            return false;
        });
        $(document).on('click', ".btn-add-to-cart .btn", function (event) {
            routes.navigate('/addcart/?bookid=' + this.id);
            return false;
        });

        $(document).on('click', ".proceed-to-cart .btn", function (event) {
            routes.navigate('/checkout');
            return false;
        });

        $(document).on('click', ".btn-remove-from-cart .btn", function (event) {
            routes.navigate('/remove/?uid=' + this.id);
            return false;
        });
        $(document).on('click', ".make-payment-btn .btn", function (event) {
            routes.navigate('/makepayment/?total=' + document.getElementById("totalCosts").innerHTML);
            return false;
        });
        $(document).on('click', ".empty-cart-btn .btn", function (event) {            
            routes.navigate('/emptycart');
            return false;
        });
    }
});

