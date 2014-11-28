/**/
/*global define*/
define('views/headerVw', { init: function ($, routes) {
    "use strict";
    
    var selector = 'form.search-form',
        inputSelector = selector + ' input';
    
    // add singleton events
    $(document).on('submit', selector, function (event) {
        routes.navigate('/search/?q=prab');
        return false;
    });

    $(document).on('click', selector, function (event) {
        alert(1);
    });

}});
