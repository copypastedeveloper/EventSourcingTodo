$(function() {
    ko.mapping.fromJSWithoutTracking = function(objectToMap, options, objectToMapInto) {
    	// Binding null arrays to observableArrays will destroy the observableArray.
	    // Find null arrays and make them empty arrays.
    	for (var n in objectToMap) {
    		if (ko.isObservableArray(objectToMapInto[n]) && objectToMap[n] == null) {
    			objectToMap[n] = [];
    		}
    	}

    	ko.mapping.fromJS(objectToMap, options, objectToMapInto);

        // Remove the information knockout added when mapping from json.
        objectToMapInto.__ko_mapping__ = null;
    };

    ko.isObservableArray = function (obj) {
    	return ko.isObservable(obj) && !(obj.destroyAll === undefined);
    };

    ko.hasValue = function (obj) {
    	return obj() != undefined;
    };

    ko.observableArray.fn.pushAll = function(items) {
        if (!(items instanceof Array)) return this.peek().length;
        this.valueWillMutate();
        ko.utils.arrayPushAll(this.peek(), items);
        this.valueHasMutated();
        return this.peek().length;
    };
    
    ko.observableArray.fn.distinct = function (prop) {
        var target = this;
        target.index = {};
        target.index[prop] = ko.observable({});

        ko.computed(function () {
            //rebuild index
            var propIndex = [];

            ko.utils.arrayForEach(target(), function (item) {
                var key = ko.utils.unwrapObservable(item[prop]);
                if (!key) return;
                
                var match = ko.findByMatchingProperties(propIndex, { propertyValue: key })[0];

                if (!match) {
                    propIndex.push(match = { propertyValue: key, values: [] });
                }
                
                match.values.push(item);
            });

            target.index[prop](propIndex);
        });

        return target;
    };

    ko.findByMatchingProperties = function(set, properties) {
        return set.filter(function(entry) {
            return Object.keys(properties).every(function(key) {
                var val1 = ko.isObservable(entry[key]) ? ko.utils.unwrapObservable(entry[key]) : entry[key];
                var val2 = ko.isObservable(properties[key]) ? ko.utils.unwrapObservable(properties[key]) : properties[key];
                    
                return val1 === val2;
            });
        });
    };

    ko.bindingHandlers.dateString = {
        update: function (element, valueAccessor) {
            var value = valueAccessor();
            var valueUnwrapped = ko.utils.unwrapObservable(value);
            if (valueUnwrapped) {
                $(element).text(moment(valueUnwrapped).format("MM/DD/YYYY"));
            }
        }
    };
	
    ko.bindingHandlers.select2 = {
    	init: function (element, valueAccessor) {
    		jQuery(element).select2(valueAccessor());

    		ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
    			jQuery(element).select2('destroy');
    		});
    	},
    	update: function (element) {
    		jQuery(element).trigger('change');
    	}
    };
    
    ko.bindingHandlers.kendoUpload.options.multiple = false;
});