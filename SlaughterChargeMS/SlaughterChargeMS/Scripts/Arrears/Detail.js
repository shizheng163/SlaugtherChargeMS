
$(function () {
    //加载全部数据
    InitGrid({ SearchType: 2, FK_UserInfo: window.UserId });
    InitCombobox();
});

//初始化表格数据
function InitGrid(queryParams) {
    var url = '/Arrears/QueryArrearsDetail';
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
            { title: '欠款/还款时间', field: 'TransTime', width: '16%', sortable: true, sortable: true, align: 'center' },
            { title: '类型', field: 'Type', width: '16%', sortable: true, sortable: true, align: 'center' },
            { title: '客户', field: 'Name', width: '16%', sortable: true, sortable: true, align: 'center' },
            { title: '欠款/还款金额', field: 'Money', width: '16%', sortable: true, sortable: true, align: 'center' },
            { title: '备注', field: 'Remark', width: '16%', align: 'center' },
            { title: '录入时间', field: 'EnterTime', width: '16%', align: 'center' },
        ]],
        toolbar: [ {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                DeleteDetail();
            }
        }],
    });
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

function DeleteDetail() {
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
        console.log(JSON.stringify(rows));
        IdList = IdList.substr(0, IdList.length - 1);
        ShowShadeUi('提交处理中...');
        $.ajax({
            type: 'POST',
            url: '/Arrears/Delete',
            data: { IdList: IdList },
            dataType: 'text',
            success: function (data) {
                if (data == "") {
                    $.messager.alert('提示', "删除成功", "info");
                    $('#grid').datagrid('load');
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

function Search() {
    var data = {};
    var type = $('#StatisticsType').combobox('getValue');
    var year = $('#StatisticsYear').combobox('getValue');
    var month = $('#StatisticsMonth').combobox('getValue');
    var day = $('#StatisticsDay').combobox('getValue');

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
    var SearchType = $('input[name="SearchType"]:checked').val();
    data["SearchType"] = SearchType;
    data["FK_UserInfo"] = window.UserId;
    InitGrid(data);
}