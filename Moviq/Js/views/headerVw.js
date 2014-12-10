/**/

/*global define*/
<<<<<<< HEAD
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
        $('span.close').click(function () { $(hiddenSection).fadeOut(); });
    }
});
=======
define('views/headerVw', { init: function ($, routes) {
    "use strict";
    
    var selector = 'form.search-form',
        inputSelector = selector + ' input';
    
    // add singleton events
    $(document).on('submit', selector, function (event) {
        routes.navigate('/search/?q=' + $(inputSelector).val());
        return false;
    });

    $(document).on('submit', 'form.exampleform', function (event) {
        Stripe.setPublishableKey('pk_test_RPfyHYbopF6CfB00Wj2IokwY');

        Stripe.createToken({
            number: $('.card-number').val(),
            cvc: $('.card-cvc').val(),
            exp_month: $('.card-expiry-month').val(),
            exp_year: $('.card-expiry-year').val()
        }, function (status, response) {
            if (response.error) {
                // re-enable the submit button
                // $(form['submit-button']).removeAttr("disabled")
                // show the error
                alert(response.error.message);
                $(".payment-errors").html(response.error.message);



            } else {
                // token contains id, last4, and card type
                var token = response['id'];

                routes.navigate('/stripe/?q=' + token + '&email=' + $('.email').val());
                return false;

            }
        });

    });
}});
>>>>>>> develop_mohit
