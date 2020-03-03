function _ArticlesManagementPartial(elem, articlesApiUrl, categoriesApiUrl) {
    //call base constructor
    _BaseManagementPartial.apply(this, arguments);
    this.categoriesApiUrl = categoriesApiUrl;

    //jqGrid
    this.grid = this.elem.find('.grid');
    //pager for jqGrid must be '#id' selector
    this.pagerID = '#' + this.elem.find('.pager').attr("id");

    //+ Methods

    this.onAfterSubmit = function (data, postdata, oper) {
        var responseMessage = data.responseJSON;
        if (responseMessage.isSuccessStatusCode === false) {
            //responseMessage.reasonPhrase
            return [false, responseMessage.reasonPhrase];
        }
        return [true, "", ""];
    };

    this.onBeforeShowForm = function (form, op, sender) {
        this.targetInput = form.find('#Body');
        var articleId = this.grid.jqGrid('getGridParam', 'selrow');
        this.loadArticleBody(articleId);
    };

    this.init = function () {
        // altrows are set with table striped class for Boostrap
        //$.jgrid.styleUI.Bootstrap.base.rowTable = "table table-bordered table-striped";

        this.grid.jqGrid({
            url: this.apiUrl,
            datatype: "json",
            colModel: [
                {
                    label: 'Cover File',
                    name: 'CoverFileId',
                    width: 45,
                    editable: true,
                    formatter: this.formatCoverFileId,
                    unformat: this.unformatCoverFileId
                },
                {
                    label: 'Title',
                    name: 'Title',
                    width: 200,
                    editable: true,
                    formatter: this.formatTitle,
                    unformat: this.unformatTitle
                },
                {
                    label: 'Timestamp',
                    name: 'Timestamp',
                    width: 45,
                    editable: true
                },
                {
                    label: 'Roles',
                    name: 'Roles',
                    width: 45,
                    //word wrap
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'style="white-space: normal;"';
                    },
                    editable: true,
                    formatter: "select",
                    edittype: "select",
                    editoptions: {
                        multiple: true,
                        value: this.getRoles()
                    }
                },
                {
                    label: 'Categories',
                    name: 'Categories',
                    width: 70,
                    //word wrap
                    cellattr: function (rowId, tv, rawObject, cm, rdata) {
                        return 'style="white-space: normal;"';
                    },
                    editable: true,
                    edittype: "select",
                    editoptions: {
                        multiple: true
                    }
                },
                {
                    label: 'Body',
                    name: 'Body',
                    hidden: true,
                    editable: true,
                    editrules: {
                        edithidden: true
                    },
                    edittype: 'textarea',
                    editoptions: { cols: 42, rows: 10 }
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
            caption: 'Управление статьями',
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
                beforeShowForm: utils.bind(this, this.onBeforeShowForm),
                afterSubmit: utils.bind(this, this.onAfterSubmit)
            },
            // options for the Add Dialog
            {
                closeAfterAdd: true,
                reloadAfterSubmit: true,
                recreateForm: true,
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

        this.loadCategories();
    };

    this.loadCategories = function () {
        var data = {};
        var url = this.categoriesApiUrl + "/GetAll";
        utils.getData(data, url, utils.bind(this, this.onLoadCategoriesComplete));
    };

    this.onLoadCategoriesComplete = function (response) {
        var value = this.buildEditOptionsFromKeyValue(response);
        var editoptions = { value: value };
        this.grid.setColProp('Categories', { editoptions: editoptions });
    };

    this.formatTitle = function (cellvalue, options, rowobject) {
        return '<a href="/Article/Index/' + options.rowId + '" target="_blank">'
            + cellvalue + '</a>';
    };

    this.unformatTitle = function (cellvalue, options, elem) {
        return cellvalue;
    };

    this.formatCoverFileId = function (cellvalue, options, rowObject) {
        var fileId = rowObject[0];
        return '<a href="/File/Index/' + fileId + '" target="_blank" class="image-cell">'
            + '<img src="/File/Thumbnail/' + fileId + '" />'
            + '</a>';
    };

    this.unformatCoverFileId = function (cellvalue, options, elem) {
        return cellvalue;
    };

    this.loadArticleBody = function (articleId) {
        var data = { id: articleId };
        var url = this.apiUrl + "/GetArticleBody";
        utils.getData(data, url, utils.bind(this, this.onLoadArticleBodyComplete));
    };

    this.onLoadArticleBodyComplete = function (response) {
        if (this.targetInput.length > 0) {
            this.targetInput.val(response);
        }
    };

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_ArticlesManagementPartial.prototype = new _BaseManagementPartial();