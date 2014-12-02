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

    $(document).on('click', ".add", function (event) {
        var cookieList = getCookie("bookCookie");
        if (cookieList != "") {
            var isExists = false;
            var arrCookieList = cookieList.split(",");
            for (var i = 0; i < arrCookieList.length; i++) {
                if (arrCookieList[i] == this.id) {
                    isExists = true;
                }
            }
            if (!isExists) {
                setCookie("bookCookie", cookieList + "," + this.id, 10 * 365 * 24 * 60 * 60);
            }
        }
        else {
            setCookie("bookCookie", this.id, 30);
        }
        return false;
    });

    function setCookie(cname, cvalue, exdays) {
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + "; " + expires;
    }
    function getCookie(cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        return "";
    }

}});
