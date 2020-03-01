function _FilesManagementPartial(elem, filesApiUrl) {
    //call base constructor
    _BaseManagementPartial.apply(this, arguments);

    //jqGrid
    this.grid = this.elem.find('.grid');
    //pager for jqGrid must be '#id' selector
    this.pagerID = '#' + this.elem.find('.pager').attr("id");

    //+ Methods
    this.onBeforeShowAddForm = function (formid) {
        $("#tr_File", formid).show();
    };

    this.onBeforeShowEditForm = function (formid) {
        $("#tr_File", formid).hide();
    };

    this.onBeforeSubmitAdd = function (postdata, form, oper) {
        var $this = this;

        var data = new FormData();
        var file = form.find('input[type="file"]').get(0).files[0];
        data.append('File', file);
        data.append('Name', postdata.Name);
        data.append('Extension', postdata.Extension);
        data.append('MimeType', postdata.MimeType);
        data.append('oper', "add");

        $.ajax({
            url: this.apiUrl,
            processData: false,
            contentType: false,
            data: data,
            type: 'POST'
        }).done(function (response) {
            if (response.isSuccessStatusCode) {
                // Updating the jqGrid
                $this.grid.jqGrid('setGridParam', { datatype: 'json' }).trigger('reloadGrid');
                // Close the form
                $("#cData").trigger("click");
            } else {
                //TODO: use jqGrid dialog for the error
                utils.showMessage(response.reasonPhrase);
            }
        }).fail(function () {

        });

        return [false, ""];
    };

    this.onAfterSubmit = function (data, postdata, oper) {
        var responseMessage = data.responseJSON;
        if (responseMessage.isSuccessStatusCode === false) {
            //responseMessage.reasonPhrase
            return [false, responseMessage.reasonPhrase];
        }
        return [true, "", ""];
    };

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
                    width: 150,
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
                },
                {
                    label: 'Timestamp',
                    name: 'Timestamp',
                    width: 50,
                    editable: false
                },
                {
                    label: 'File',
                    name: 'File',
                    width: 80,
                    editable: true,
                    editoptions: {
                        enctype: "multipart/form-data"
                    },
                    edittype: 'file',
                    editrules: { edithidden: true },
                    hidden: true
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
                recreateForm: true,
                beforeShowForm: utils.bind(this, this.onBeforeShowEditForm),
                afterSubmit: utils.bind(this, this.onAfterSubmit)
            },
            // options for the Add Dialog
            {
                closeAfterAdd: true,
                reloadAfterSubmit: true,
                recreateForm: true,
                beforeShowForm: utils.bind(this, this.onBeforeShowAddForm),
                beforeSubmit: utils.bind(this, this.onBeforeSubmitAdd),
                afterSubmit: utils.bind(this, this.onAfterSubmit)
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
        var mimeType = rowobject[2];
        if (mimeType && mimeType.match(/image/g)) {
            return '<a href="/File/Index/' + options.rowId + '" target="_blank" class="image-cell">'
                + '<img src="/File/Thumbnail/' + options.rowId + '" />'
                + '<span>' + cellvalue + '</span>'
                + '</a>';
        }
        return '<a href="/File/Index/' + options.rowId + '" target="_blank" class="image-cell">'
            + cellvalue
            + '</a>';
    };

    this.unformatName = function (cellvalue, options, elem) {
        return cellvalue;
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_FilesManagementPartial.prototype = new _BaseManagementPartial();