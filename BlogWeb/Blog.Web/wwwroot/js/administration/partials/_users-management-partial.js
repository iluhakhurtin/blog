﻿function _UsersManagementPartial(elem, apiUrl) {
    //call base constructor
    _BaseManagementPartial.apply(this, arguments);

    //jqGrid
    this.grid = this.elem.find('.grid');
    //pager for jqGrid must be '#id' selector
    this.pagerID = '#' + this.elem.find('.pager').attr("id");

    //+ Methods

    this.onBeforeShowEditForm = function (formid) {
        $("#Email", formid).attr("readonly", "readonly");
    };

    this.onBeforeShowAddForm = function (formid) {
        $("#Email", formid).removeAttr("readonly", "readonly");
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
                    label: 'Email',
                    name: 'Email',
                    width: 100,
                    editable: true
                },
                {
                    label: 'Roles',
                    name: 'Roles',
                    width: 100,
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
                    label: 'Password',
                    name: 'Password',
                    hidden: true,
                    editable: true,
                    editrules: { edithidden: true },
                    hidedlg: true
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
            caption: 'Управление пользователями',
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
                editCaption: "Роли",
                closeAfterEdit: true,
                reloadAfterSubmit: true,
                recreateForm: true,
                afterSubmit: utils.bind(this, this.onAfterSubmit),
                beforeShowForm: utils.bind(this, this.onBeforeShowEditForm)
            },
            // options for the Add Dialog
            {
                closeAfterAdd: true,
                reloadAfterSubmit: true,
                recreateForm: true,
                afterSubmit: utils.bind(this, this.onAfterSubmit),
                beforeShowForm: utils.bind(this, this.onBeforeShowAddForm)
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

    //- Methods

    //+ Methods calls

    this.init();

    //- Methods calls
};

_UsersManagementPartial.prototype = new _BaseManagementPartial();