function _ImagesManagementPartial(elem, filesApiUrl) {
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
                    label: 'Id',
                    name: 'Id',
                    width: 50,
                    editable: false,
                    formatter: this.formatId
                },
                {
                    label: 'Preview File',
                    name: 'PreviewFileId',
                    width: 50,
                    editable: true,
                    formatter: utils.bind(this, this.formatPreviewFile),
                    unformat: utils.bind(this, this.unformatPreviewFile)
                },
                {
                    label: 'Original File',
                    name: 'OriginalFileId',
                    width: 50,
                    editable: true,
                    formatter: utils.bind(this, this.formatOriginalFile),
                    unformat: utils.bind(this, this.unformatOriginalFile)
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
            caption: 'Управление изображениями',
            sortable: true,
            //altRows: true, This does not work in boostrarap
            //altclass: '....'
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
                editCaption: "Статьи",
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

    this.formatId = function (cellvalue, options, rowobject) {
        return options.rowId;
    }

    this.formatPreviewFile = function (cellvalue, options, rowobject) {
        var fileId = rowobject[0];
        var fileName = rowobject[1];
        var value = this.formatImageCell(fileId, fileName);
        return value;
    };

    this.formatOriginalFile = function (cellvalue, options, rowobject) {
        var fileId = rowobject[2];
        var fileName = rowobject[3];
        var value = this.formatImageCell(fileId, fileName);
        return value;
    };

    this.formatImageCell = function (fileId, fileName) {
        return '<a href="/File/Index/' + fileId + '" target="_blank" class="image-cell">'
            + '<img src="/File/Thumbnail/' + fileId + '" />'
            + '<span>' + fileName + '</span>'
            + '</a>';
    };

    this.unformatPreviewFile = function (cellvalue, options, elem) {
        //var rowData = this.grid.jqGrid('getRowData', options.rowId);
        //return rowData[0];
        return "";
    };

    this.unformatOriginalFile = function (cellvalue, options, elem) {
        //var rowData = this.grid.jqGrid('getRowData', options.rowId);
        //return rowData[2];
        return "";
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_ImagesManagementPartial.prototype = new _BaseManagementPartial();