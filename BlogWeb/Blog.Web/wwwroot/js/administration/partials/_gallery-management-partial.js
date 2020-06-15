function _GalleryManagementPartial(elem, filesApiUrl) {
    //call base constructor
    _BaseManagementPartial.apply(this, arguments);

    //jqGrid
    this.grid = this.elem.find('.grid');
    //pager for jqGrid must be '#id' selector
    this.pagerID = '#' + this.elem.find('.pager').attr("id");

    //+ Methods
    
    this.init = function () {
        // altrows are set with table striped class for Boostrap
        //$.jgrid.styleUI.Bootstrap.base.rowTable = "table table-bordered table-striped";

        this.grid.jqGrid({
            url: this.apiUrl,
            datatype: "json",
            colModel: [
                {
                    label: 'SmallFileId',
                    name: 'SmallFileId',
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Small File',
                    name: 'SmallFileName',
                    width: 50,
                    editable: true,
                    formatter: utils.bind(this, this.formatSmallFile),
                    unformat: utils.bind(this, this.unformatImage)
                },
                {
                    label: 'ImageId',
                    name: 'ImageId',
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'PreviewFileId',
                    name: 'PreviewFileId',
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Preview File',
                    name: 'PreviewFileName',
                    width: 50,
                    editable: true,
                    formatter: utils.bind(this, this.formatPreviewFile),
                    unformat: utils.bind(this, this.unformatImage)
                },
                {
                    label: 'OriginalFileId',
                    name: 'OriginalFileId',
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Original File',
                    name: 'OriginalFileName',
                    width: 50,
                    editable: true,
                    formatter: utils.bind(this, this.formatOriginalFile),
                    unformat: utils.bind(this, this.unformatImage)
                },
                {
                    label: 'ArticleId',
                    name: 'ArticleId',
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Article',
                    name: 'ArticleTitle',
                    width: 80,
                    editable: true,
                    //word wrap
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'style="white-space: normal;"';
                    }
                },
                {
                    label: 'Description',
                    name: 'Description',
                    width: 80,
                    editable: true,
                    //word wrap
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'style="white-space: normal;"';
                    }
                },
                {
                    label: 'Timestamp',
                    name: 'Timestamp',
                    width: 35,
                    editable: false
                }
            ],
            autowidth: true,
            height: 'auto',
            loadonce: false,
            altRows: true,
            //rownumbers : true,
            //multiselect : true,
            colMenu: true,
            menubar: true,
            viewrecords: true,
            hoverrows: true,
            rowNum: 10,
            caption: 'Управление фотогалереей',
            sortable: true,
            pager: this.pagerID,
            editurl: this.apiUrl
        });

        this.grid.navGrid(this.pagerID,
            // the buttons to appear on the toolbar of the grid
            {
                edit: true,
                add: true,
                del: true,
                search: true,
                refresh: true,
                view: true,
                position: "left",
                cloneToTop: false
            },
            // options for the Edit Dialog
            {
                editCaption: "Фото",
                closeAfterEdit: true,
                reloadAfterSubmit: true,
                recreateForm: true
            },
            // options for the Add Dialog
            {
                closeAfterAdd: true,
                reloadAfterSubmit: true,
                recreateForm: true
            },
            // options for the Delete Dailog
            {
                errorTextFormat: function (data) {
                    return 'Error: ' + data.responseText;
                }
            },
            // search options - define multiple search
            {
                multipleSearch: true,
                showQuery: true
            }
        );
    };

    this.formatSmallFile = function (cellvalue, options, rowobject) {
        var fileId = rowobject[0];
        var fileName = rowobject[1];
        var value = this.formatImageCell(fileId, fileName);
        return value;
    };

    this.formatPreviewFile = function (cellvalue, options, rowobject) {
        var fileId = rowobject[3];
        var fileName = rowobject[4];
        var value = this.formatImageCell(fileId, fileName);
        return value;
    };

    this.formatOriginalFile = function (cellvalue, options, rowobject) {
        var fileId = rowobject[5];
        var fileName = rowobject[6];
        var value = this.formatImageCell(fileId, fileName);
        return value;
    };

    this.unformatImage = function (cellvalue, options, elem) {
        return cellvalue;
    };

    this.formatImageCell = function (fileId, fileName) {
        return '<a href="/File/Index/' + fileId + '" target="_blank" class="image-cell">'
            + '<img src="/File/Thumbnail/' + fileId + '" />'
            + '<span>' + fileName + '</span>'
            + '</a>';
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_GalleryManagementPartial.prototype = new _BaseManagementPartial();