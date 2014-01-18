Global = window.Global || {};
Global.Routing = {
    Initialize: function() {
        window.History.Adapter.bind(window, 'statechange', function() {
            var state = window.History.getState();
            crossroads.parse(state.hash);
        });
	    
        History.options.disableSuid = true;
	    
        $('body').on('Routing.Complete', function(e, content) {
        	// Clear the Flatty application-error class in case an error previously occured.
        	$("body").removeClass("application-error");
        	Global.Routing.Linkify(content);
        });
    },
    AddRoute: function (route, title, partialUrl, bindingFunction) {
    	crossroads.addRoute(route, function () {
    		window.document.title = title;
    		var viewFilename = partialUrl;
    		// This adds the contents of arguments to an array because arguments isn't really an array.
    		var routeArgs = [].slice.apply(arguments);

    		$.get(viewFilename, function (data) {
    			var content = $('#content');
    			content.html(data);
    			routeArgs.unshift(content.find('div:first'));

    			if (bindingFunction) {
    				bindingFunction.apply(this, routeArgs);
    				$('body').trigger('Binding.Complete');
    			}

    			$('body').trigger('Routing.Complete', content);
    		});
    	});
    },
    AddExceptionRoute: function (route, title, partialUrl) {
    	crossroads.addRoute(route, function () {
    		window.document.title = title;
    		var viewFilename = partialUrl;

    		$.get(viewFilename, function (data) {
    			var content = $('#content');
    			content.html(data);
    		});
    	});
    },
    Linkify: function (element) {
        $(element)
            .on("click", "a:not(.normalLink):not([href^=#])", function () {
                var href = $(this).attr("href");
                Global.Routing.Redirect(href);
                return false;
            });
    },
    Redirect: function(url) {
        window.History.pushState(null, null, url);
    }
    
};