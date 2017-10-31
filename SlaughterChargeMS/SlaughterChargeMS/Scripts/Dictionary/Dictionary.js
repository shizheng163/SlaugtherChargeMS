var isAddOrEdit = 'add';//标识是新增还是编辑对话框弹出，用于删除附件的操作
var url;//新增或者更新的连接
var ID;//ID值，新增为空，编辑或者查看为具体ID

//页面加载后
$(function () {
    var queryData = {};//可添加一些预设条件
    InitGrid(queryData);//初始化Datagrid表格数据

    //设置不可更改
    $('#Code').textbox('disable');
});

function InitGrid(queryData) {
    //选中第一条
    $('#dg').datagrid({
        url: '/Dictionary/GetDicCategory',
        rownumbers: false,
        fitColumns: true,
        singleSelect: true,
        remotesort: false,
        nowrap: false,
        fit: true,
        columns: [[
            { title: '编号', field: 'Code', width: '40%', sortable: false, halign: 'center', align: 'center' },
            { title: '类别', field: 'Name', width: '66%', sortable: false, halign: 'center' }
        ]],
        onLoadSuccess: function (data) {
            $('#dg').datagrid('selectRow', 0);
        },
        onSelect: function (rowIndex, rowData) {
            if (rowIndex >= 0) {
                $('#dg2').datagrid('clearSelections');
                $('#dg2').datagrid({
                    url: '/Dictionary/GetDicCategoryInfo?code=' + rowData.Code
                });

            }
        }
    });

    $('#dg2').datagrid({
        iconCls: 'icon-view',
        rownumbers: true,
        width: function () { return document.body.clientWidth*0.7 },//自动宽度
        pagination: true,
        sortName: 'Code',    //根据某个字段给easyUI排序
        pageSize: 20,
        sortOrder: 'asc',
        remoteSort: false,
        nowrap: false,
        idField: 'Id',
        queryParams: queryData,  //异步查询的参数
        columns: [[
            { field: 'ck', width: '1%', checkbox: true },
            { title: '编号', field: 'Code', width: '9%', sortable: true, halign: 'center' },
            { title: '名称', field: 'Name', width: '20%', sortable: true, halign: 'center' },
            { title: '备注', field: 'Remark', width: '55%', sortable: false, halign: 'center' },
        ]],
        toolbar: [{
            id: 'btnAdd',
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                ShowAddDialog();//实现添加记录的页面
            }
        }, '-', {
            id: 'btnEdit',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function () {
                ShowEditDialog();//实现修改记录的方法
            }
        }, '-', {
            id: 'btnDelete',
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                Delete();//实现直接删除数据的方法
            }
        }]
    });

}

function SaveEntity() {
    var categoryCode = $('#dg').datagrid('getSelected').Code;
    if (isAddOrEdit == "add") {
        //增加
        url = '/Dictionary/AddDicCategoryInfo';
        operate(categoryCode);
    }
    else if (isAddOrEdit == "edit") {
        //修改
        url = '/Dictionary/UpdateDicCategoryInfo';
        operate(categoryCode);
    }
}

function operate(categoryCode) {
    var $code = $('#Code');
    var $name = $('#Name');
    var $remark = $('#Remark');

    if ($code.val() == '') {
        $.messager.alert('系统提示', '请输入编号！', 'warning');
        return false;
    }

    if ($name.val() == '') {
        $.messager.alert('系统提示', '请输入名称！', 'warning');
        return false;
    }

    var postdata = { 'FK_DictionaryCategory': categoryCode, 'code': $code.val(), 'name': $name.val(), 'remark': $remark.val()};
    
    ShowShadeUi('操作中,请稍后......');
    $.post(url, postdata, function (msg) {
        closeShadeUi();
        if (msg == "") {
            $.messager.alert('系统提示', '操作成功！', 'info', function () {
                $('#DivAdd').dialog('close');
                //重新加载
                $('#dg2').datagrid('reload');
                //清除所有选中的行
                $('#dg2').datagrid('clearSelections');
                //清除所有选过的行
                $('#dg2').datagrid('clearChecked');
            });
            $code.val('');
            $name.val('');
            $remark.val('');
        }
        else {
            $.messager.alert('系统提示', msg, 'error');
        }
    }, 'text');

}

function ShowAddDialog() {
    isAddOrEdit = 'add';//新增对话框标识

    $('#DivAdd').dialog({
        title: '字典管理-增加',
        iconCls: "icon-add"
    }).dialog('open');

    //设置增加窗口Code的值
    setAddWinCode();
    $('#Name').textbox('setValue', "");//
    $('#Remark').textbox('setValue', "");//
}

function setAddWinCode() {
    var categoryCode = $('#dg').datagrid('getSelected').Code;
    //从服务器获取待添加的字典信息的Code
    $.getJSON("/Dictionary/GetGeneraoteCode?FKCode=" + categoryCode, function (data) {
        //console.log(data);
        $('#Code').textbox('setValue', categoryCode + data.Code);
    });

}

//按钮编辑
function ShowEditDialog() {
    isAddOrEdit = 'edit';//编辑对话框标识
    var selections = $('#dg2').datagrid('getSelections');

    //判断是否有选择
    if (selections.length > 0) {

        //判断是否选择了多项
        if (selections.length > 1) {
            $.messager.alert('系统提示', '每次只能修改一条记录！', 'warning');
            return false;
        } else {
            editData(selections[0])
        }
    }
    else {
        $.messager.alert('系统提示', '请选择需编辑的记录！', 'warning');
        return false;
    }
}

function editData(data) {
    $('#DivAdd').dialog({
        title: '字典管理-编辑[' + data.Code + ']',
        iconCls: "icon-edit"
    }).dialog('open');
    $('#Code').textbox('setValue', data.Code);//设置
    $('#Name').textbox('setValue', data.Name);//设置
    $('#Remark').textbox('setValue', data.Remark);//设置
}

//按钮删除
function Delete() {
    var ids = [];
    var rows = $('#dg2').datagrid('getSelections');
    if (rows.length > 0) {
        for (var i = 0; i < rows.length; i++) {
            ids.push(rows[i].Code);
        }
        ids.join(',');
        removeData(ids)
    }
    else {
        $.messager.alert('系统提示', '还未选择项目！', 'warning');
        return false;
    }
}
function removeData(data) {
    $.messager.confirm('系统提示', '您确定要删除此项目吗?', function (r) {
        if (r) {
            //调用后台删除代码，成功后刷新grid
            ShowShadeUi('删除中,请稍后.....');
            $.post('/Dictionary/removeDicCategoryInfo', { 'ids': data + "" }, function (msg) {
                closeShadeUi();
                if (msg == "") {
                    $.messager.alert('系统提示', '操作成功！', 'info', function () {

                        $('#dg2').datagrid('reload');
                        $('#dg2').datagrid('clearSelections');
                        $('#dg2').datagrid('clearChecked');
                    });
                }
                else {
                    $.messager.alert('系统提示', msg, 'error');
                }
            });
        }
        else {
            return false;
        }
    })
}