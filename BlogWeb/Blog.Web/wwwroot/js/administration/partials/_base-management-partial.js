function _BaseManagementPartial(elem, apiUrl) {
    //call base constructor
    _BasePartial.apply(this, arguments);

    //urls
    this.apiUrl = apiUrl;

    //+ Methods

    this.getRoles = function () {
        return {
            "": "",
            "PrivateReader": "PrivateReader",
            "ImageViewer": "ImageViewer",
            "Administrator": "Administrator"
        };
    };

    this.buildEditOptionsFromKeyValue = function (items) {
        var result = "";
        if (items) {
            for (var i = 0; i < items.length; i++) {
                var item = items[i];
                result += item.Key + ":" + item.Value;
                if (i < items.length - 1) {
                    result += ";";
                }
            }
        }

        return result;
    }

    //- Methods
};

_BaseManagementPartial.prototype = new _BasePartial();