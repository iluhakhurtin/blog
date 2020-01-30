function _BasePartial(elem) {
    this.elem = $(elem);

    this.show = function (show) {
        if (show)
            this.elem.show();
        else
            this.elem.hide();
    };

    this.isVisible = function () {
        return this.elem.is(":visible");
    };

    this.detach = function () {
        this.elem.detach();
    };

    this.appendTo = function (elem) {
        this.elem.appendTo(elem);
    };
}