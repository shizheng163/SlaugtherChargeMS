
var IncomeReason = null;//收入原因
var PayReason = null;//支出原因
$(function () {
    //加载全部数据
    InitGrid(SearchData);//SearchData由页面加载完成后 页面本身JS生成。
    GetReasonData();
});

//初始化表格数据
function InitGrid(queryParams) {
    var url = '/ButcherProcess/QueryButcherDetailWithPager';
    $('#grid').datagrid({
        url: url,
        queryParams: queryParams,
        pagination: true,
        pageSize: 20,
        rownumbers: true,
        remoteSort: false,
        idField: 'Id',
        striped: true,
        nowrap: false,
        columns: [[
            { field: 'ck', width: '1%', checkbox: true, align: 'center' },
            { field: 'Id', hidden: true },
            { title: '交易时间', field: 'TransTime', width: '12%', sortable: true, sortable: true, align: 'center' },
            { title: '交易类型', field: 'Type', width: '12%', sortable: true, sortable: true, align: 'center' },
            { title: '收入来源/支出原因', field: 'Reason', width: '12%', sortable: true, sortable: true, align: 'center' },
            { title: '交易对象', field: 'TranscationObject', width: '12%', sortable: true, sortable: true, align: 'center' },
            { title: '变更数量', field: 'UpdateNum', width: '12%', align: 'center' },
            { title: '单只金额/支出金额', field: 'SingleAmount', width: '12%', sortable: true, sortable: true, align: 'center' },
            { title: '收入/支出金额', field: 'TotalSpend', width: '12%', align: 'center' },
            { title: '录入时间', field: 'EnterTime', width: '12%', align: 'center' },
        ]],
        toolbar: [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                showAddDialog();
            }
        }, '-', {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                DeleteDetail();
            }
        }],
    });
}

//打开添加详情的对话框
function showAddDialog() {
    if (IncomeReason == null || PayReason == null)
    {
        $.messager.alert('提示', '收支原因获取失败,已重新请求获取收支原因,请稍后再打开对话框', 'info');
        GetReasonData();
        return;
    }
    Reset(1);
    $('#AddButcherDetail').window({
        title: '添加进货批次'
    }).window('open');
}

function Reset(type) {
    if (type == 1)//重置添加详情
    {
        $('input[name="Type"]:eq(0)').attr('checked', 'true');
        SetReasonCombox(0);
        $('#ButcherTime').datebox('setValue', GetCurDateTime());
        $('#TranscationObject').textbox('setValue', '');
        $('#SellCategory').combobox('select', '0101');
        $('#UpdateNum').numberbox('setValue', '1');
        $('#SignleAmount').numberbox('setValue', '');
    }
}

//批次
function SaveEntity() {
    var Type = $('input[name="Type"]:checked').val();
    var ReasonCode = $('#Reason').combobox('getValue');
    if (ReasonCode == '' || ReasonCode == null) {
        $.messager.alert('提示', "请选择收支原因", "info");
        return;
    }
    var ButcherTime = $('#ButcherTime').datebox('getValue');
    if (ButcherTime == null || ButcherTime == "") {
        $.messager.alert('提示', "交易时间未填写", "info");
        return;
    }
    var TranscationObject = $('#TranscationObject').textbox('getValue');
    if (TranscationObject == null || TranscationObject == "") {
        $.messager.alert('提示', "交易对象不允许为空", "info");
        return;
    }
    var UpdateNum = $('#UpdateNum').numberbox('getValue');
    if (UpdateNum == null || UpdateNum == "") {
        $.messager.alert('提示', "货物数量不允许为空", "info");
        return;
    }

    var SignleAmount = $('#SignleAmount').numberbox('getValue');
    if (SignleAmount == null || SignleAmount == "") {
        $.messager.alert('提示', "单个金额/支出金额不允许为空", "info");
        return;
    }


    var PostData = {
        Type: Type,
        FK_Reason: ReasonCode, TransactionObject: TranscationObject, TransTime: ButcherTime,
        UpdateNum: UpdateNum, SingleAmount: SignleAmount
    };

    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/ButcherProcess/AddButcherProcess',
        data: PostData,
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                InitGrid(SearchData);
                $.messager.alert('提示', "添加成功", "info");
                $('#AddButcherDetail').window('close');
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

function DeleteDetail()
{
    var rows = $('#grid').datagrid('getSelections');
    if (rows.length == 0) {
        $.messager.alert("提示", "请选择要删除的交易信息", "info");
        return;
    }
    $.messager.confirm('提示', '您确定要删除选中的信息？删除后无法恢复！', function (action) {
        if (!action)
            return;
        var IdList = "";
        for (var i = 0; i < rows.length; i++) {
            IdList = IdList + rows[i].Id + ",";
        }
        IdList = IdList.substr(0, IdList.length - 1);
        ShowShadeUi('提交处理中...');
        $.ajax({
            type: 'POST',
            url: '/ButcherProcess/DeleteButcherDetailInfo',
            data: { IdList: IdList },
            dataType: 'text',
            success: function (data) {
                if (data == "") {
                    $.messager.alert('提示', "删除成功", "info");
                    InitGrid(SearchData);
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
    });
}
//得到收支原因的数据
function GetReasonData()
{
    ShowShadeUi("请稍后......");
    $.ajax({
        url: '/Common/GetComboBoxValue',
        dataType: 'json',
        data: { name: "屠宰加工收入原因" },
        success: function (data) {
            IncomeReason = data;
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', XMLHttpRequest.status + "," + XMLHttpRequest.readyState + "," + textStatus, 'error');
        },
        complete: function () {
            closeShadeUi();
        }
    });
    ShowShadeUi("请稍后......");
    $.ajax({
        url: '/Common/GetComboBoxValue',
        dataType: 'json',
        data: { name: "屠宰加工支出原因" },
        success: function (data) {
            PayReason = data;
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', XMLHttpRequest.status + "," + XMLHttpRequest.readyState + "," + textStatus, 'error');
        },
        complete: function () {
            closeShadeUi();
        }
    });

}
//设置收支原因
function SetReasonCombox(type)
{
    var data = null;
    if (type == 0) {
        data = IncomeReason.concat();
        $('#SingnleAmountLab').html('单个金额');
        $('#UpdateNum').numberbox('enable');
    }
    else if (type == 1) {
        data = PayReason.concat();
        $('#SingnleAmountLab').html('支出金额');
        $('#UpdateNum').numberbox('disable');
    }
    data.unshift({ "Name": unescape(escape("-请选择-")), "Code": "" });
    $('#Reason').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 300,
        required: true,
        editable: false,//不可编辑，只能选择
        data: data
    });
    $('#Reason').combobox('select', '');
}
