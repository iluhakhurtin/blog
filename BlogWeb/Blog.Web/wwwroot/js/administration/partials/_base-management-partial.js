function _BaseManagementPartial(elem, apiUrl) {
    //call base constructor
    _BasePartial.apply(this, arguments);

    //urls
    this.apiUrl = apiUrl;

    //+ Methods

    this.getRoles = function () {
        return {
            "PrivateReader": "PrivateReader",
            "ImageViewer": "ImageViewer",
            "Administrator": "Administrator"
        };
    };


    //- Methods
};

_BaseManagementPartial.prototype = new _BasePartial();