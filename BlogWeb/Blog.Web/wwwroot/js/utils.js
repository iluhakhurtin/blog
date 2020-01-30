//all global functions should be encapsulated in this object
var utils = {
    //Binds method to the object
    bind: function (object, method) {
        return function () {
            var args = $.merge([], arguments);
            //Pass current context as last parameter to the callee function
            args.push(this);
            return method.apply(object, args);
        };
    },

    postJSON: function (data, url, successCallback) {
        $.ajax({
            url: url,
            data: JSON.stringify(data),
            type: 'POST',
            contentType: "application/json",
            dataType: "json",
            success: function (response, status, XmlHttpRequest) {
                if (successCallback)
                    successCallback(response);
            }
        });
    },

    postFormData: function (data, url, successCallback) {
        $.ajax({
            url: url,
            data: data,
            type: 'POST',
            dataType: "json",
            success: function (response, status, XmlHttpRequest) {
                if (successCallback)
                    successCallback(response);
            }
        });
    },

    getData: function (data, url, successCallback) {
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            cache: false,
            success: function (response, status, XmlHttpRequest) {
                if (successCallback)
                    successCallback(response);
            }
        });
    },

    showMessage: function (msg) {
        alert(msg);
    }
};