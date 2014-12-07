/**/
/*global define*/
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
               
                routes.navigate('/stripe/?q=' + token+'&email='+$('.email').val());
                return false;
                
            }
        });

    });
   
    
  

       

}});
