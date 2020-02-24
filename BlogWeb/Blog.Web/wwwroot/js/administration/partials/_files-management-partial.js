function _FilesManagementPartial(elem, filesApiUrl) {
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
                    label: 'Name',
                    name: 'Name',
                    width: 250,
                    editable: true,
                    formatter: this.formatName,
                    unformat: this.unformatName
                },
                {
                    label: 'Extension',
                    name: 'Extension',
                    width: 50,
                    editable: true
                },
                {
                    label: 'Mime Type',
                    name: 'MimeType',
                    width: 50,
                    editable: true
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
            caption: 'Управление файлами',
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

    this.formatName = function (cellvalue, options, rowobject) {
        return '<a href="/File/Index/' + options.rowId + '" target="_blank">'
            + cellvalue + '</a>';
    };

    this.unformatTitle = function (cellvalue, options, elem) {
        return cellvalue;
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_FilesManagementPartial.prototype = new _BaseManagementPartial();