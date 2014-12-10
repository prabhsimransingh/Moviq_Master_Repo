/*global define, JSON*/
define('controllers/paymentController', {
    init: function ($, routes, viewEngine) {
        "use strict";

        var submitPayment;
        var onPayment;

        // GET /pay
        // Payment 
        //routes.get(/^\/#\/pay\/?/i, function (context) {
        //    viewEngine.setView({
        //        template: 't-pay',
        //        data: {}
        //    });
        //});
        routes.get(/^\/#\/makepayment\/?/i, function (context) {
            var cost=context.params.total;
            viewEngine.setView({
                template: 't-pay',
                data: { totalcost: cost}
            });
        });

        routes.get(/^\/#\/stripe\/?/i, function (context) {           
            $('.submit-button').attr("disabled", "disabled");
            Stripe.setPublishableKey('pk_test_RPfyHYbopF6CfB00Wj2IokwY');
         
            Stripe.createToken({
                number: $('.card-number').val(),
                cvc: $('.card-cvc').val(),
                exp_month: $('.card-expiry-month').val(),
                exp_year: $('.card-expiry-year').val()
            }, function (status, response) {
                if (response.error) {
                   //re-enable the submit button
                    $('.submit-button').removeAttr("disabled")
                    // show the error
                    alert(response.error.message);
                    $(".payment-errors").html(response.error.message);
                
                } else {
                    // token contains id, last4, and card type
                    var token = response['id'];
                    
                    onPayment(token,context.params.totalcost);
                    false;
                }
            });
            

            // adding the input field names is the last step, in case an earlier step errors
            addInputNames();
        });

        onPayment = function (token, totalcost) {
           
            return $.ajax({
                url: '/api/payment/?q=' + token+ '&totalcost=' +totalcost*100,
                method: 'GET'
            }).done(function (data) {

                var amountValue = JSON.parse(JSON.parse(data).content).amount;
                var txnid = JSON.parse(JSON.parse(data).content).balance_transaction;

                if (JSON.parse(JSON.parse(data).content).paid) {
                    viewEngine.setView({
                        template: 't-pay-success',
                        data: { payvalue: amountValue/100, transid: txnid }
                    });
                } else {
                    viewEngine.setView({
                        template: 't-pay-fail'
                        
                    });
                }
            });
        };
    }
});
