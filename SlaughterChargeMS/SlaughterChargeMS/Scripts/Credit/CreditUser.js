var curSelectUserId = null;//判断当前是添加行为还是修改行为
var CurAddtype;
$(function () {
    InitGrid();
    InitUserGrid();
});

//初始化表格数据
function InitGrid() {
    var url = '/Credit/QueryArrearsUserInfo';
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
            { title: '客户编号', field: 'UserNo', width: '20%', sortable: true, sortable: true, align: 'center' },
            { title: '客户姓名', field: 'Name', width: '20%', sortable: true, sortable: true, align: 'center' },
            { title: '拖欠客户金额', field: 'ArrearsNum', width: '20%', sortable: true, sortable: true, align: 'center' },
            { title: '客户公司', field: 'Company', width: '20%', sortable: true, sortable: true, align: 'center' },
            { title: '联系方式', field: 'Phone', width: '15%', sortable: true, sortable: true, align: 'center' },

        ]],
        toolbar: [{
            text: '添加欠款信息',
            iconCls: 'icon-add',
            handler: function () {
                ShowAddDialog(1);
            }
        }, '-', {
            text: '添加还款信息',
            iconCls: 'icon-ok',
            handler: function () {
                ShowAddDialog(2);
            }
        }, '-', {
            text: '查看详情',
            iconCls: 'icon-edit',
            handler: function () {
                var rows = $('#grid').datagrid('getSelections');
                if (rows.length == 0) {
                    $.messager.alert('提示', "请选择要查看的详细条目", "info");
                    return;
                }
                else if (rows.length > 1) {
                    $.messager.alert('提示', "您只能选择一条信息来查看", "info");
                    return;
                }
                parent.addTab('客户赊欠项目款查看-' + rows[0].Name + '(' + rows[0].UserNo + ')', '/Credit/Detail?UserId=' + rows[0].Id, 'icon icon-nav');
            }
        }],
        onDblClickRow: function (index, row) {
            parent.addTab('客户赊欠项目款查看-' + row.Name + '(' + row.UserNo + ')', '/Credit/Detail?UserId=' + row.Id, 'icon icon-nav');
        }
    });
    $('#grid').datagrid('clearSelections');
}
//初始化选择用户表格
function InitUserGrid()
{
    var url = '/UserInfo/QueryUserInfoWithPager';
    $('#gridUser').datagrid({
        url: url,
        pagination: true,
        pageSize: 20,
        rownumbers: true,
        singleSelect: true,
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
            { title: '联系方式', field: 'Phone', width: '18%', sortable: true, sortable: true, align: 'center' },
        ]],
        toolbar: [{
            text: '确定选择',
            iconCls: 'icon-add',
            handler: function () {
                SelectUser();
            }
        }]
    });
    var SelectUser = function () {
        var row = $('#gridUser').datagrid('getSelected');
        if (row == null || row == undefined) {
            $.messager.alert('提示', "请选择要选定的客户", "info");
            return;
        }
        curSelectUserId = row.Id;
        $('#UserInfoTextBox').textbox('setValue', row.Name + '(' + row.UserNo + ')');
        $('#gridUser').datagrid('clearSelections');
        $('#SelectUserDialog').dialog('close');
    };
    $('#gridUser').datagrid('clearSelections');
}

//打开添加客户的对话框
function ShowAddDialog(type) {
    Reset(type);
    if (type == 2)
    {
        var rows = $('#grid').datagrid('getSelections');
        if (rows.length == 0) {
            $.messager.alert('提示', "请选择还款客户信息", "info");
            return;
        }
        else if(rows.length>1){
            $.messager.alert('提示', "您只能选择一条客户信息进行还款", "info");
            return;
        }
        $('#UserInfoTextBox').textbox('setValue', rows[0].Name + '(' + rows[0].UserNo + ')');
        curSelectUserId = rows[0].Id;
    }
    $('#AddArrearsInfo').dialog('open');
}

function Reset(type) {
    if (type == 1)//重置添加批次
    {
        CurAddtype = 0;
        curSelectUserId = null;
        $('#UserInfoTextBox').textbox('setValue', '');
        $('#remark').textbox('setValue', '');
        $('#TransTimn').datebox('setValue', GetCurDateTime());
        $('#Money').numberbox('setValue', '');
        $('#moneyLab').html('欠款金额');
        $('#dateLab').html('赊欠日期');
        $('#selectUserA').show();
        InitUserGrid();
    }
    else if (type == 2) {
        CurAddtype = 1;
        $('#moneyLab').html('还款金额');
        $('#dateLab').html('还款日期');
        $('#UserInfoTextBox').css('width','180px');
        $('#remark').textbox('setValue', '');
        $('#TransTimn').datebox('setValue', GetCurDateTime());
        $('#Money').numberbox('setValue', '');
        $('#selectUserA').hide();
    }
}
//批次
function SaveEntity() {

    var UserInfo = $('#UserInfoTextBox').textbox('getValue');
    if (UserInfo == null || UserInfo == ""  ||curSelectUserId == null) {
        $.messager.alert('提示', "请选择用户信息", "info");
        return;
    }
    var TransTimn = $('#TransTimn').datebox('getValue');
    if (TransTimn == "")
    {
        $.messager.alert('提示', CurAddtype == 0? "请选择赊欠日期":"请选择还款日期", "info");
        return;
    }
    var Money = $('#Money').numberbox('getValue');
    if (Money == null || Money == "") {
        $.messager.alert('提示', CurAddtype == 0 ? "请输入欠款金额" : "请输入还款金额", "info");
        return;
    }

    
    var PostData = { Type: CurAddtype, FK_UserInfo: curSelectUserId, Money: Money, TransTime: TransTimn, Remark: $('#remark').textbox('getValue') };
    var url = '/Credit/AddModel';
   
    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: url,
        data: PostData,
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "操作成功", "info");
                $('#AddArrearsInfo').window('close');
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


function OpenSelectUserDialog()
{
    $('#SelectUserDialog').dialog('open');
    if (curSelectUserId != null)
    {
        $('#gridUser').datagrid('selectRecord', curSelectUserId);
    }
}