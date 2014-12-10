/*global define, JSON*/
define('controllers/paymentController', {
    init: function ($, routes, viewEngine) {
        "use strict";

        var submitPayment;
        var onPayment;

        // GET /pay
        // Payment 
        routes.get(/^\/#\/pay\/?/i, function (context) {
            viewEngine.setView({
                template: 't-pay',
                data: {}
            });
        });

        routes.get(/^\/#\/stripe\/?/i, function (context) {
            onPayment(context);
        });

        onPayment = function (context) {
            return $.ajax({
                url: '/api/payment/?q=' + context.params.q+'&email='+context.params.email,
                method: 'GET'
            }).done(function (data) {
                
                 var amountValue = JSON.parse(JSON.parse(data).content).amount;
                 var txnid = JSON.parse(JSON.parse(data).content).balance_transaction;
                
                if (JSON.parse(JSON.parse(data).content).paid) {
                    viewEngine.setView({
                        template: 't-pay-success',
                        data: { payvalue: amountValue,transid: txnid }
                    });
                } else {
                    viewEngine.setView({
                        template: 't-no-results',
                        data: { searchterm: context.params.q }
                    });
                }
            });
        };
    }
});
