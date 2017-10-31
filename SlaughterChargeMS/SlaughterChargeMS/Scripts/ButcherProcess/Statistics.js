//双击后的行Id或者查看详情后的行Id
var CurSellGridId;

$(function () {
    InitGrid({ Type: 0 });
    InitCombobox();
    InitGridDetail();
    $('#gridDetail').datagrid('options').url = '/Sell/QuerySellBatchDetailInfo';
});

//初始化表格数据
function InitGrid(queryParams) {
    var url = '/ButcherProcess/QueryButcherStatiticsWithPager';
    $('#grid').datagrid({
        url: url,
        queryParams: queryParams,
        pagination: true,
        pageSize: 20,
        rownumbers: true,
        singleSelect: true,
        remoteSort: false,
        idField: 'Index',
        striped: true,
        nowrap: false,
        columns: [[
            { field: 'ck', width: '1%', checkbox: true, align: 'center' },
            { field: 'Index', width: '20%', hidden: true },
            { title: '统计区间', field: 'Range', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '收入金额', field: 'IncomeMoney', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '支出金额', field: 'PayMoney', width: '25%', sortable: true, sortable: true, align: 'center' },
            { title: '盈利金额', field: 'ProfitMoney', width: '22%', sortable: true, sortable: true, align: 'center' },
        ]], onDblClickRow: function (index, row) {
            showDetailDialog(row);
        }
    });
    $('#grid').datagrid('clearSelections');
}

//初始化筛选框的下拉条件
function InitCombobox() {
    var StatisticsTypeData = [{ Code: 2, Name: '年' }, { Code: 1, Name: '月' }, { Code: 0, Name: '日' }];
    $('#StatisticsType').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 300,
        required: true,
        editable: false,//不可编辑，只能选择
        data: StatisticsTypeData,
        onChange: function (newValue) {
            $('#StatisticsYear').combobox('select', 0);
            $('#StatisticsMonth').combobox('select', 0);
            console.log(newValue);
            if (newValue == 2) {
                $('#yearDiv').show();
                $('#MonthDiv').hide();
                $('#DayDiv').hide();
            }
            else if (newValue == 1) {
                $('#MonthDiv').show();
                $('#DayDiv').hide();
                $('#yearDiv').show();
            }
            else {
                $('#MonthDiv').show();
                $('#DayDiv').show();
                $('#yearDiv').show();
            }
        }
    });
    $('#StatisticsType').combobox('select', 0);

    var yearData = [{ Code: 0, Name: '全部' }];
    var year = parseInt(GetCurDateTime().split('-')[0]);
    for (var i = 0; i <= 20; i++) {
        yearData.push({ Code: year - i, Name: '' + (year - i) });
    }
    var MonthData = [{ Code: 0, Name: '全部' }];
    for (var i = 1; i <= 12; i++) {
        MonthData.push({ Code: i, Name: '' + i });
    }
    var DayData = [{ Code: 0, Name: '全部' }];
    for (var i = 1; i <= 31; i++) {
        DayData.push({ Code: i, Name: '' + i });
    }
    //日
    $('#StatisticsDay').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 'auto',
        required: true,
        editable: false,//不可编辑，只能选择
        data: DayData,
    });
    $('#StatisticsDay').combobox('select', '0');
    //月
    $('#StatisticsMonth').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 'auto',
        required: true,
        editable: false,//不可编辑，只能选择
        data: MonthData,
        onChange: function (newValue) {
            if (newValue == 0) {
                $('#StatisticsDay').combobox({
                    valueField: 'Code',
                    textField: 'Name',
                    panelHeight: 'auto',
                    required: true,
                    editable: false,//不可编辑，只能选择
                    data: [{ Code: 0, Name: '全部' }]
                });
            }
            else {
                $('#StatisticsDay').combobox({
                    valueField: 'Code',
                    textField: 'Name',
                    panelHeight: 'auto',
                    required: true,
                    editable: false,//不可编辑，只能选择
                    data: DayData
                });
            }
            $('#StatisticsDay').combobox('select', 0);
        }
    });
    $('#StatisticsMonth').combobox('select', '0');
    //年
    $('#StatisticsYear').combobox({
        valueField: 'Code',
        textField: 'Name',
        panelHeight: 'auto',
        required: true,
        editable: false,//不可编辑，只能选择
        data: yearData,
        onChange: function (newValue) {
            if (newValue == 0) {
                $('#StatisticsMonth').combobox({
                    valueField: 'Code',
                    textField: 'Name',
                    panelHeight: 'auto',
                    required: true,
                    editable: false,//不可编辑，只能选择
                    data: [{ Code: 0, Name: '全部' }]
                });
            }
            else {
                $('#StatisticsMonth').combobox({
                    valueField: 'Code',
                    textField: 'Name',
                    panelHeight: 'auto',
                    required: true,
                    editable: false,//不可编辑，只能选择
                    data: MonthData
                });
            }
            $('#StatisticsMonth').combobox('select', 0);
            $('#StatisticsDay').combobox({
                valueField: 'Code',
                textField: 'Name',
                panelHeight: 'auto',
                required: true,
                editable: false,//不可编辑，只能选择
                data: [{ Code: 0, Name: '全部' }]
            });
            $('#StatisticsDay').combobox('select', 0);
        }
    });
    $('#StatisticsYear').combobox('select', '0');


}

function Search() {
    var data = {};
    var type = $('#StatisticsType').combobox('getValue');
    var year = $('#StatisticsYear').combobox('getValue');
    var month = $('#StatisticsMonth').combobox('getValue');
    var day = $('#StatisticsDay').combobox('getValue');
    data["Type"] = type;
    if (type == 2) {
        if (year != 0)
            data["Range"] = year;
    }
    else if (type == 1)//按月统计
    {
        if (year != 0 && month != 0) {
            data["Range"] = year + "-" + (month.length == 1 ? "0" : "") + month;
        }
        else if (year != 0 && month == 0)
            data["Range"] = year;
    }
    else {
        var Range = "";
        if (year != 0) {
            Range = year + "";
            if (month != 0) {
                Range = Range + "-" + (month.length == 1 ? "0" : "") + month;
                if (day != 0) {
                    Range = Range + "-" + (day.length == 1 ? "0" : "") + day;
                }
            }
        }
        data["Range"] = Range;
    }
    InitGrid(data);
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
        QuantityNum: QuantityNum, RemainderNum: 0, QuantitySpend: SellSpend
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
function showDetailDialog(Row) {
    $('#ViewRange').html(Row.Range);
    $('#ViewIncome').html(Row.IncomeMoney);
    $('#ViewPay').html(Row.PayMoney);
    $('#ViewProfit').html(Row.ProfitMoney);
    $('#gridDetail').datagrid('options').url = '/ButcherProcess/QueryButcherStatiticsViewWithPager';
    $('#gridDetail').datagrid('load', { Range: Row.Range });
    $('#StatisticsViewDialog').window('open');
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
        idField: 'ReasonCode',
        striped: true,
        nowrap: false,
        columns: [[
            { field: 'ReasonCode', hidden: true },
            { title: '收支原因', field: 'ReasonName', width: '30%', sortable: true, sortable: true, align: 'center' },
            { title: '变更数目', field: 'UpdateNum', width: '30%', sortable: true, sortable: true, align: 'center' },
            { title: '收支金额', field: 'TotalMoney', width: '32%', sortable: true, sortable: true, align: 'center' },
        ]],
        toolbar: [{
            text: '查看所有收入细节',
            iconCls: 'icon-ok',
            handler: function () {
                var range = $('#ViewRange').html();
                parent.addTab(range + '屠宰加工统计收入详情', '/ButcherProcess/DetailView?Type=0&Range=' + range + '&ReasonCode=0', 'icon icon-nav');
            }
        }, '-', {
            text: '查看所有支出细节',
            iconCls: 'icon-ok',
            handler: function () {
                var range = $('#ViewRange').html();
                //alert('/ButcherProcess/DetailView?Type=1&Range=' + range + '&ReasonCode=0');
                parent.addTab(range + '屠宰加工统计支出详情', '/ButcherProcess/DetailView?Type=1&Range=' + range + '&ReasonCode=0', 'icon icon-nav');
            }
        }, '-', {
            text: '<label style="color:red;">双击条目查看详情</label>',
            width: 300,
            handler: function () {

            }
        }],
        onDblClickRow: function (index, row) {
            var reasonCode = row.ReasonCode;
            var range = $('#ViewRange').html();
            var type = reasonCode.substr(0, 2) == "03" ? 0 : 1;
            parent.addTab(range + row.ReasonName + '统计详情', '/ButcherProcess/DetailView?Type=' + type + '&Range=' + range + '&ReasonCode=' + reasonCode, 'icon icon-nav');
        }
    });
}