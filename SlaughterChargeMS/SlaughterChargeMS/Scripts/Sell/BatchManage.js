//双击后的行Id或者查看详情后的行Id
var CurSellGridId;

$(function () {
    InitGrid();
    InitCombobox();
    InitGridDetail();
    $('#gridDetail').datagrid('options').url = '/Sell/QuerySellBatchDetailInfo';
});

//初始化表格数据
function InitGrid(queryParams) {
    var url = '/Sell/QuerySellBatchInfo';
    $('#grid').datagrid({
        url: url,
        queryParams: queryParams,
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
            { field: 'Id', width: '20%', hidden: true },
            { title: '批次编号', field: 'BatchNumber', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '进货日期', field: 'QuantityDate', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '交易对象', field: 'TranscationObject', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '贩卖类型', field: 'SellCategory', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '进货数量', field: 'QuantityNum', width: '8%', align: 'center' },
            { title: '剩余数量', field: 'RemainderNum', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '进货支出', field: 'QuantitySpend', width: '8%', align: 'center' },
            { title: '其他支出', field: 'OtherSpend', width: '8%', align: 'center' },
            { title: '当前收入', field: 'CurIncome', width: '8%',  align: 'center' },
            { title: '盈利金额', field: 'CurProfit', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '录入时间', field: 'EnterTime', width: '8%', sortable: true, sortable: true, align: 'center' },
            { title: '经办人', field: 'Operator', width: '8%', align: 'center' },


        ]],
        toolbar: [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                showAddSellDialog();
            }
        }, '-', {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                $.messager.confirm('提示', '您确定要删除选中的信息(连同该批次下的交易细节)？删除后无法恢复!', function (action1) {
                    if (action1)
                        Delete();
                });
            }
        }, '-', {
            text: '查看详情',
            iconCls: 'icon-edit',
            handler: function () {
                showDetailDialog();
            }
        }],
        onDblClickRow: function (index, row)
        {
            showDetailDialog(row.Id);
        }
    });
}

function InitCombobox()
{
    BindDictItem("SellCategory", "01", 3, "0101");//货物类型
    BindDictItem("TransReason", "02", 3, "0202");//货物类型
    var spendTypeData = [{ Code: '1', Name: '收入' }, { Code: '2', Name: '支出' }];
    $('#SpendType').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 300,
        required: true,
        editable: false,//不可编辑，只能选择
        data: spendTypeData
    });
    $('#SpendType').combobox('select', '1');
}

//打开添加课程的对话框
function showAddSellDialog() {
    Reset(1);
    ShowShadeUi('信息加载中...');
    $.ajax({
        url: '/Sell/GetNextSellBatchNumber',
        dataType: 'json',
        success: function (data) {
            if (data.ret == false) {
                $.messager.alert('提示', data.errorMsg, "error");
            }
            else {
                $('#BatchNumber').textbox('setValue', data.data);
                $('#AddSellBach').window({
                    title: '添加进货批次'
                }).window('open');
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });

}

function Reset(type) {
    if (type == 1)//重置添加批次
    {
        $('#SellDate').datebox('setValue', GetCurDateTime());
        $('#TranscationObject').textbox('setValue', '');
        $('#SellCategory').combobox('select', '0101');
        $('#QuantityNum').numberbox('setValue', '');
        $('#SellSpend').numberbox('setValue', '');
    }
    else if (type == 2) //重置添加交易
    {
        $('#TransTime').datebox('setValue', GetCurDateTime());
        $('#DetailTranscationObject').textbox('setValue', '');
        $('#TransReason').combobox('select', '0202');
        $('#TransNum').numberbox('setValue', '0');
        $('#TransCost').numberbox('setValue', '');
        $('#SpendType').combobox('select', '1');
    }
}

//批次
function SaveEntity() {
    var SellDate = $('#SellDate').datebox('getValue');
    if (SellDate == null || SellDate == "") {
        $.messager.alert('提示', "采购日期未填写", "info");
        return;
    }
    var TranscationObject = $('#TranscationObject').textbox('getValue');
    if (TranscationObject == null || TranscationObject == "") {
        $.messager.alert('提示', "交易对象不允许为空", "info");
        return;
    }
    var SellCategory = $('#SellCategory').combobox('getValue');
    var QuantityNum = $('#QuantityNum').numberbox('getValue');
    if (QuantityNum == null || QuantityNum == "") {
        $.messager.alert('提示', "货物数量不允许为空", "info");
        return;
    }

    var SellSpend = $('#SellSpend').numberbox('getValue');
    if (SellSpend == null || SellSpend == "") {
        $.messager.alert('提示', "进货支出不允许为空", "info");
        return;
    }

    var PostData = {
        QuantityDate: SellDate, TransactionObject: TranscationObject, FK_SellCategory: SellCategory,
        QuantityNum: QuantityNum,RemainderNum: 0, QuantitySpend: SellSpend
        };

    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/Sell/AddSellBatchInfo',
        data: PostData,
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "添加成功", "info");
                $('#AddSellBach').window('close');
                InitGrid(null);
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

//显示当前批次详细交易信息表格
function showDetailDialog(RowId)
{
    if (RowId == null) {
        var rows = $('#grid').datagrid('getSelections');
        if (rows.length == 0) {
            $.messager.alert("提示", "请选择要查看的批次", "info");
            return;
        }
        else if (rows.length > 1) {
            $.messager.alert("提示", "您只能选择一条记录来查看", "info");
            return;
        }
        CurSellGridId = rows[0].Id;
    }
    else
        CurSellGridId = RowId;
    
    AjaxGetDetail(CurSellGridId, function () {
        $('#gridDetail').datagrid('load', { SellId: CurSellGridId });
        $('#SellDetailManageDialog').window('open');
    });
}

function AjaxGetDetail(id, callback)
{
    $.ajax({
        url: '/Sell/QuerySignleSellBatchInfo',
        data: { SellId: id },
        dataType: 'json',
        success: function (data) {
            if (data.ret == false) {
                $.messager.alert('提示', data.errorMsg, "error");
            }
            else {
                var ret = data.data;
                $('#CurBatchNumber').html(ret.BatchNumber);
                $('#CurQuantityDate').html(ret.QuantityDate);
                $('#CurTranscationObject').html(ret.TranscationObject);
                $('#CurSellCategory').html(ret.SellCategory);
                $('#CurQuantityNum').html(ret.QuantityNum);
                $('#CurRemainderNum').html(ret.RemainderNum);
                $('#CurQuantitySpend').html(ret.QuantitySpend);
                $('#CurOtherSpend').html(ret.OtherSpend);
                $('#CurIncome').html(ret.CurIncome);
                $('#CurProfit').html(ret.CurProfit);
                $('#CurEnterTime').html(ret.EnterTime);
                $('#CurOperator').html(ret.Operator);
                callback();
            }

        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', "错误编码:" + XMLHttpRequest.status, "error");
        },
        complete: function () {
            closeShadeUi();
        }
    });
}

function InitGridDetail() {
    $('#gridDetail').datagrid({
        url: '',
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
            { title: '所在批次', field: 'BatchNumber', width: '6%',align: 'center' },
            { title: '交易时间', field: 'TransTime', width: '11%', sortable: true, sortable: true, align: 'center' },
            { title: '交易对象', field: 'TranscationObject', width: '11%', sortable: true, sortable: true, align: 'center' },
            { title: '交易原因', field: 'TransReason', width: '6%', sortable: true, sortable: true, align: 'center' },
            { title: '交易货物数量', field: 'TransNum', width: '10%', align: 'center' },
            { title: '交易后剩余数量', field: 'RemainderNum', width: '10%', align: 'center' },
            { title: '交易金额', field: 'TrnasCost', width: '6%', align: 'center' },
            { title: '收支类型', field: 'SpendType', sortable: true, sortable: true, width: '6%', align: 'center' },
            { title: '录入时间', field: 'EnterTime', width: '18%', sortable: true, sortable: true, align: 'center' },
        ]],
        toolbar: [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                Reset(2);
                $('#AddSellDetailInfo').window('open');
            }
        }, '-', {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                DeleteDetail();
            }
        }]
    });
}

function SaveDetailEntity() {
    var TransTime = $('#TransTime').datebox('getValue');
    if (SellDate == null || SellDate == "") {
        $.messager.alert('提示', "交易时间未填写", "info");
        return;
    }
    var TranscationObject = $('#DetailTranscationObject').textbox('getValue');
    if (TranscationObject == null || TranscationObject == "") {
        $.messager.alert('提示', "交易对象不允许为空", "info");
        return;
    }
    var TransReason = $('#TransReason').combobox('getValue');
    var TransNum = $('#TransNum').numberbox('getValue');
    if (TransNum == null || TransNum == "") {
        $.messager.alert('提示', "交易货物数量不允许为空", "info");
        return;
    }

    var TransCost = $('#TransCost').numberbox('getValue');
    if (SellSpend == null || SellSpend == "") {
        $.messager.alert('提示', "交易金额不允许为空", "info");
        return;
    }

    var SpendType = $('#SpendType').combobox('getValue');

    var PostData = {
        FK_Sell:CurSellGridId,
        TransTime: TransTime, TransactionObject: TranscationObject, FK_TranscationReason: TransReason,
        TransNum: TransNum, TransCost: TransCost, SpendType: SpendType
    };

    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/Sell/AddSellBatchDetailInfo',
        data: PostData,
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "添加成功", "info");
                $('#AddSellDetailInfo').window('close');
                AjaxGetDetail(CurSellGridId, function () {
                    $('#grid').datagrid('load');
                    $('#gridDetail').datagrid('clearSelections');
                    $('#gridDetail').datagrid('reload');
                });
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
    var rows = $('#gridDetail').datagrid('getSelections');
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
        url: '/Sell/DeleteSellBatchDetailInfo',
        data: { IdList: IdList },
        dataType: 'text',
        success: function (data) {
            if (data == "") {
                $.messager.alert('提示', "删除成功", "info");
                $('#AddSellBach').window('close');
                $('#AddSellDetailInfo').dialog('close');
                AjaxGetDetail(CurSellGridId, function () {
                    $('#grid').datagrid('load');
                    $('#gridDetail').datagrid('clearSelections');
                    $('#gridDetail').datagrid('load');
                });
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

function Delete() {
    var rows = $('#grid').datagrid('getSelections');
    if (rows.length == 0) {
        $.messager.alert("提示", "请选择要删除的交易信息", "info");
        return;
    }
    var IdList = "";
    for (var i = 0; i < rows.length; i++) {
        IdList = IdList + rows[i].Id + ",";
    }
    IdList = IdList.substr(0, IdList.length - 1);
    ShowShadeUi('提交处理中...');
    $.ajax({
        type: 'POST',
        url: '/Sell/Delete',
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