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
                    width: 50,
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Small',
                    name: 'Small',
                    width: 50,
                    editable: true,
                    formatter: this.formatImage,
                    unformat: this.unformatImage
                },
                {
                    label: 'ImageId',
                    name: 'ImageId',
                    width: 50,
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Preview',
                    name: 'Preview',
                    width: 50,
                    editable: true,
                    formatter: this.formatImage,
                    unformat: this.unformatImage
                },
                {
                    label: 'Original',
                    name: 'Original',
                    width: 50,
                    editable: true,
                    formatter: this.formatImage,
                    unformat: this.unformatImage
                },
                {
                    label: 'ArticleId',
                    name: 'ArticleId',
                    width: 50,
                    editable: true,
                    editrules: { edithidden: true },
                    hidden: true
                },
                {
                    label: 'Article',
                    name: 'Article',
                    width: 35,
                    editable: true
                },
                {
                    label: 'Description',
                    name: 'Description',
                    width: 50,
                    editable: true
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
            caption: 'Управление фотогаллереей',
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

    this.formatImage = function (cellvalue, options, rowobject) {
        return '<a href="/File/Index/' + cellvalue + '" target="_blank" class="image-cell">'
            + '<img src="/File/Thumbnail/' + cellvalue + '" />'
            + '<span>' + cellvalue + '</span>'
            + '</a>';
    };

    this.unformatImage = function (cellvalue, options, elem) {
        return cellvalue;
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_GalleryManagementPartial.prototype = new _BaseManagementPartial();