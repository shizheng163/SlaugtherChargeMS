/**
*  作者：史正  (shizheng163@126.com)
*  时间：2017年7月13日14:12:51
*  文件名：Common
*  说明： 各个模块通用方法
*───────────────────────────────────
* 
*/
/*字符串格式化扩展
    eg:
    var Pattern = "My name is {0},My old is {1}";
    var Introduce_MySelfy = Pattern.format("张三","18");
*/
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题  
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

var SpinOpts = {
    lines: 13, // The number of lines to draw,
    length: 42, // The length of each line width: 14 // The line thickness
    radius: 33, // The radius of the inner circle
    scale: 1, // Scales overall size of the spinner
    corners: 1, // Corner roundness (0..1)
    color: '#000', // #rgb or #rrggbb or array of colors
    opacity: 0.4, // Opacity of the lines
    rotate: 0, // The rotation offset
    direction: 1, // 1: clockwise, -1: counterclockwise
    speed: 1, // Rounds per second
    trail: 64, // Afterglow percentage
    fps: 20, // Frames per second when using setTimeout() as a fallback for CSS
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    className: 'spinner', // The CSS class to assign to the spinner
    top: '50%', // Top position relative to parent
    left: '50%', // Left position relative to parent
    shadow: false, // Whether to render a shadow
    hwaccel: false, // Whether to use hardware acceleration
    position: 'absolute' // Element positioning
}
//开启进度条 无说明文字，适用于加载迅速的
/*
    使用页面需链接 
    spin.min.js
    jquery-xx.min.js
    先后顺序不可改变
    关闭进度条方式为:
    var obj = showSpin();
    obj.spin();
*/
function showSpin() {
    var target = document.body;
    var spinner = new Spinner(SpinOpts).spin(target);
    spinner.spin(target);
    return spinner;
}

//easyui对象添加属性  
function EasyUiAttrProperty(JqueryObj, PropertyName, PropertyValue) {
    var span = JqueryObj.siblings("span")[0];
    var targetInput = $(span).find("input:first");
    if (targetInput) {
        $(targetInput).attr(PropertyName, PropertyValue);
    }
}

var ProgressCount = 0;
//开启进度条  data: 显示数据
function ShowShadeUi(message)
{
    //var text;
    if(message == null)
        text = '<h1><img src="/Images/loading.gif" /> Just a moment...</h1>';
    else
        text = '<h1><img src="/Images/loading.gif" /> '+unescape(escape(message))+'</h1>';
    //$.blockUI({ message: text });
    ProgressCount++;
    if (ProgressCount == 1) {
        $.messager.progress({
            title: unescape(escape("加载中")),
            msg: text
        });
        //$('.messager-progress').html(text);
    }
}

function closeShadeUi() {
    //console.log('Close:' + ProgressCount);
    if (ProgressCount != 0)
        ProgressCount--;
    if (ProgressCount == 0)
        $.messager.progress('close');
    else {
        var text = '<h1><img src="/Images/loading.gif" /> '+unescape(escape("请稍候"))+'</h1>';
        $.messager.progress({
            title:unescape(escape("加载中")),
            msg:text
        });
        //$('.messager-progress').html(text);
    }
}

//绑定字典信息Code shizheng添加
/*
    type=0.全部
    type=1.-请选择-
    其他，不添加
*/
function BindDictItem(comboid, catlogCode, type, value) {
    ShowShadeUi("请稍后......");
    $.ajax({
        url: '/Common/GetComboBoxValueByCode',
        dataType: 'json',
        data: { name: catlogCode },
        success: function (data) {
            if (type == 0)
                data.unshift({ "Name": unescape(escape("全部")), "Code": "0" });
            else if (type == 1)
                data.unshift({ "Name": unescape(escape("-请选择-")), "Code": "" });
            $('#' + comboid).combobox({
                valueField: 'Code',
                textField: 'Name',
                panelHeight: 300,
                required: true,
                editable: false,//不可编辑，只能选择
                data: data
            });
            if (value != null)
                $('#' + comboid).combobox('select', value);
        }, error: function (XMLHttpRequest, textStatus, errorThrown) {
            $.messager.alert('提示', XMLHttpRequest.status + "," + XMLHttpRequest.readyState + "," + textStatus, 'error');
        },
        complete: function ()
        {
            closeShadeUi();
        }
    });
}
//获取当前日期 type=1 只获取日期部分 type=2 也获取时间部分
function GetCurDateTime(type)
{
    var curr_time = new Date();
    var strDate = curr_time.getYear() + "-";//获取出错 获取成117
    if (strDate == "117-")
        strDate = strDate.replace("1", "20");
    strDate += curr_time.getMonth() + 1 + "-";
    strDate += curr_time.getDate();
    if (type == null || type == 1)
        return strDate;
    strDate += " "+curr_time.getHours() + ":";
    strDate += curr_time.getMinutes() + ":";
    strDate += curr_time.getSeconds();
    return strDate;
}