/*global define*/
define('views/viewEngine', { init: function ($, ko) {
    "use strict";
    
    var mainVw, headerVw, setView;    
    mainVw = {
        viewModel: ko.observable()
    };
    var setCookie = function (cname, cvalue, exdays) {        
        var d = new Date();
        d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
        var expires = "expires=" + d.toUTCString();
        document.cookie = cname + "=" + cvalue + "; " + expires;        
    };
    var getCookie = function (cname) {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1);
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        return "";
    };
    var deleteCookie = function (cname) {
        setCookie(cname,"",-1);
    };
    headerVw = function () {        
        var self = {};        
        self.home = { text: 'Moviq', path: '/' };
        self.links = [];
        self.cartCount = ko.observable();
        self.showCartCount = ko.computed(function () {
            return self.cartCount() > 0;
        }, self);
        
        self.addToCart = function () {            
            self.cartCount((self.cartCount() || 0) + 1);            
        };
        
        self.subtractFromCart = function () {
            var count = (self.cartCount() || 1) - 1;
            
            if (count > 0) {
                self.cartCount(count);
            } else {
                self.cartCount('');
            }
        };
        
        self.links.push({ text: 'BOOKS', href: 'books' });
        self.links.push({ text: 'MUSIC', href: 'music' });
        self.links.push({ text: 'MOVIES', href: 'movies' });
        var cookieList = getCookie("bookCookie");
        if (cookieList != "") {
            var isExists = false;
            var arrCookieList = cookieList.split(",");
            for (var val in arrCookieList) {                
                self.addToCart();
            }
        }
        return self;
    };
    
    setView = function (viewModel) {        
        if (!viewModel) {
            throw new Error('viewModel is undefined. The mainVw cannot be updated.');
        }
        
        if (window.scroll) {
            window.scroll(0, 0);
        }
        

        $('.main').removeClass('in').addClass('out');
        setTimeout(function () {
            mainVw.viewModel(viewModel);
            $('.main').removeClass('out').addClass('in');
            
            if (typeof viewModel.after === 'function') {
                viewModel.after();
            }
        }, 500);
    };
    
    return {
        // mainVw is a singleton - there will only ever be one
        // it is the master view model that is used for refreshing page content
        mainVw: mainVw,
        
        // headerVw is a 
        headerVw: headerVw(),
        setCookie: setCookie,
        getCookie: getCookie,
        deleteCookie: deleteCookie,
        // setView provides a safter approach to udpating the viewModel 
        // property of the mainVw.
        // @param viewModel (object): The active viewModel
        setView: setView
    };

}});