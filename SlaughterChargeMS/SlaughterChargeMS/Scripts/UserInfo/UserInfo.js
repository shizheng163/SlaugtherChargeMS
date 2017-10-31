var action = null;//判断当前是添加行为还是修改行为

$(function () {
    InitGrid();
});

//初始化表格数据
function InitGrid() {
    var url = '/UserInfo/QueryUserInfoWithPager';
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
            { title: '客户编号', field: 'UserNo', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '客户姓名', field: 'Name', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '客户公司', field: 'Company', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '联系方式', field: 'Phone', width: '20%', sortable: true, sortable: true, align: 'center' },
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
    ShowShadeUi('信息加载中...');
    $.ajax({
        url: '/UserInfo/GetNextUserNo',
        dataType: 'json',
        success: function (data) {
            if (data.ret == false) {
                $.messager.alert('提示', data.errorMsg, "error");
            }
            else {
                $('#UserNo').textbox('setValue', data.data);
                $('#AddUser').window({
                    title: '添加客户信息'
                }).window('open');
                action = "Add";
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });

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
    $('#UserNo').textbox('setValue', row.UserNo);
    $('#Name').textbox('setValue', row.Name);
    $('#Phone').numberbox('setValue', row.Phone);
    $('#Company').textbox('setValue', row.Company);
    action = 'Edit';
    $('#AddUser').window({
        title: '更新客户信息',
    }).window('open');
}

function Reset(type) {
    if (type == 1)//重置添加批次
    {
        $('#Name').textbox('setValue', '');
        $('#Phone').numberbox('setValue', '');
        $('#Company').textbox('setValue', '');
    }
}
//批次
function SaveEntity() {

    var Name = $('#Name').textbox('getValue');
    if (Name == null || Name == "") {
        $.messager.alert('提示', "用户姓名不允许为空", "info");
        return;
    }
    var Company = $('#Company').textbox('getValue');

    var Phone = $('#Phone').numberbox('getValue');
    if (Phone != null && Phone != "") {
        if (Phone.length != 11)
        {
            $.messager.alert('系统提示', '请输入有效的手机号码!', 'warning', function () {
                $('#Phone3').focus();
            });
            return;
        }
        else if (isPhoneNo(Phone) == false)
        {
            $.messager.alert('系统提示', '请输入有效的手机号码!', 'warning', function () {
                $('#Phone3').focus();
            });
            return;
        }
    }

    
    var PostData = { Name: Name, Company: Company, Phone: Phone };
    var url = '';
    if (action == 'Add') {
        url = '/UserInfo/AddUser';
    }
    else if(action == 'Edit'){
        url = '/UserInfo/Update';
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
        $.messager.alert("提示", "请选择要删除的交易信息", "info");
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
        url: '/UserInfo/Delete',
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

// 验证130-139,150-159,180-189号码段的手机号码
function isPhoneNo(phone) {
    var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/;
    if (!myreg.test(phone)) {
        return false;
    }
    return true;
}
