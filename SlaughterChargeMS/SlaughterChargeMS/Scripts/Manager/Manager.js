var action = null;//判断当前是添加行为还是修改行为

$(function () {
    InitGrid();
});

//初始化表格数据
function InitGrid() {
    var url = '/Manager/QueryUserInfoWithPager';
    $('#grid').datagrid({
        url: url,
        pagination: true,
        pageSize: 20,
        rownumbers: true,
        singleSelect: false,
        remoteSort: false,
        sortOrder: 'asc',
        idField: 'Id',
        striped: true,
        nowrap: false,
        columns: [[
            { field: 'ck', width: '1%', checkbox: true, align: 'center' },
            { field: 'Id', hidden: true },
            { title: '登录名', field: 'LoginName', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '姓名', field: 'Name', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '添加/修改人员', field: 'OperatorName', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '添加/修改时间', field: 'OperatorTime', width: '20%', sortable: true, sortable: true, align: 'center' },
        ]],
        toolbar: [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                ShowAddUserDialog();
            }
        }, '-', {
            text: '修改',
            iconCls: 'icon-ok',
            handler: function () {
                ShowEditUserDialog();
            }
        }, '-', {
            text: '重置密码',
            iconCls: 'icon-ok',
            handler: function () {
                $.messager.confirm('提示', '您确定要重置选中的用户的密码？重置后无法恢复!', function (action1) {
                    if (action1)
                        ResetPassword();
                });
            }
        }, '-', {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                $.messager.confirm('提示', '您确定要删除选中的信息？删除后无法恢复!', function (action1) {
                    if(action1)
                        Delete();
                });
            }
        }]
    });

    $('#grid').datagrid('clearSelections');
}

//打开添加客户的对话框
function ShowAddUserDialog() {
    Reset(1);
    $('#AddUser').window({
        title: '添加管理员信息'
    }).window('open');
    action = "Add";
}
//打开修改客户的对话框
function ShowEditUserDialog()
{
    var getSelections = $('#grid').datagrid('getSelections');
    if (getSelections.length == 0)
    {
        $.messager.alert('提示', '请选择要修改的信息', 'info');
        return;
    }
    else if (getSelections.length > 1)
    {
        $.messager.alert('提示', '您只能选中一条信息来进行修改', 'info');
        return;
    }
    var row = getSelections[0];
    $('#LoginName').textbox('setValue', row.LoginName);
    $('#Name').textbox('setValue', row.Name);
    action = 'Edit';
    $('#AddUser').window({
        title: '更新管理员信息',
    }).window('open');
}

function Reset(type) {
    if (type == 1)//重置添加批次
    {
        $('#Name').textbox('setValue', '');
        $('#LoginName').textbox('setValue', '');
    }
}
//批次
function SaveEntity() {

    var LoginName = $('#LoginName').textbox('getValue');
    if (LoginName == null || LoginName == "") {
        $.messager.alert('提示', "登录名不允许为空", "info");
        return;
    }

    var Name = $('#Name').textbox('getValue');
    if (Name == null || Name == "") {
        $.messager.alert('提示', "用户姓名不允许为空", "info");
        return;
    }

    var PostData = { Name: Name, LoginName: LoginName };
    var url = '';
    if (action == 'Add') {
        url = '/Manager/AddUser';
    }
    else if(action == 'Edit'){
        url = '/Manager/Update';
        var id = $('#grid').datagrid('getSelections')[0].Id;
        PostData["Id"] = id;
    }

    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: url,
        data: PostData,
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "操作成功", "info");
                $('#AddUser').window('close');
                InitGrid();
            }
            else {
                $.messager.alert('提示', data, "error");
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });
}

function Delete()
{
    var rows = $('#grid').datagrid('getSelections');
    if (rows.length == 0) {
        $.messager.alert("提示", "请选择要删除的信息", "info");
        return;
    }
    var IdList = "";
    for (var i = 0; i < rows.length; i++)
    {
        IdList = IdList + rows[i].Id + ",";
    }
    IdList = IdList.substr(0, IdList.length - 1);
    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/Manager/Delete',
        data: { IdList: IdList },
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "删除成功", "info");
                InitGrid();
            }
            else {
                $.messager.alert('提示', data, "error");
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });
}

function ResetPassword()
{
    var getSelections = $('#grid').datagrid('getSelections');
    if (getSelections.length == 0) {
        $.messager.alert('提示', '请选择要重置密码的用户', 'info');
        return;
    }
    else if (getSelections.length > 1) {
        $.messager.alert('提示', '您只能选择一个用户', 'info');
        return;
    }

    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/Manager/ResetPassword',
        data: { UserId: getSelections[0].Id },
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "重置密码成功!(与登录名相同!)", "info");
                InitGrid();
            }
            else {
                $.messager.alert('提示', data, "error");
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });
}