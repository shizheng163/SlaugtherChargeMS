var _menus = { "menus": [{}] };


//设置登录窗口
/*   function openPwd() {
       $('#w').window({
           title: '修改密码',
           width: 300,
           modal: true,
           shadow: true,
           closed: true,
           height: 200,
           resizable: false
       });
   }
   */
//关闭登录窗口
function closePwd() {
    $('#w').window('close');
}



//修改密码
function ModPwd() {
    var $oldpass = $('#txtOldPass');
    var $newpass = $('#txtNewPass');
    var $rePass = $('#txtRePass');

    if ($oldpass.val() == '') {
        $.messager.alert('系统提示', '请输入原密码！', 'warning');
        return false;
    }
    if ($newpass.val() == '') {
        $.messager.alert('系统提示', '请输入新密码！', 'warning');
        return false;
    }

    if ($rePass.val() == '') {
        $.messager.alert('系统提示', '请再一次输入密码！', 'warning');
        return false;
    }
    if ($newpass.val() != $rePass.val()) {
        $.messager.alert('系统提示', '两次密码不一致！', 'warning');
        return false;
    }



    $.post('/Common/modPwd', { 'oldpwd': $oldpass.val(), 'newpwd': $newpass.val() }, function (msg) {
        if (msg == "") {
            $.messager.alert('系统提示', '密码修改成功！', 'info');
            $oldpass.val('');
            $newpass.val('');
            $rePass.val('');
            closePwd();
        }
        else {
            $.messager.alert('系统提示', msg, 'error');

        }
    })
}

$(function () {

    $.get("/Common/GetLoginName", function (response) {
        $("#login").html("欢迎您:" + response);
    });
    $.ajax({
        type: 'GET',
        url: '/Common/GetLoginMenusJson',
        async: false,//同步
        dataType: 'json',

        success: function (json) {
            try {
                _menus = json;
                InitLeftMenu();
            }
            catch (e) {
                _menus = { "menus": [{}] };
                msgShow("提示", e, 'error')
            }
        },
        error: function (xhr, status, error) {
            //alert("操作失败"); //xhr.responseText
            //错误或者未登陆,重新登陆
            window.location.href = '/Login/Index';
            //msgShow('系统提示', '操作失败！', 'error');
        }
    });
    //openPwd();

    $('#editpass').click(function () {

        $('#w').window('open');
    });

    $('#btnEp').click(function () {
        ModPwd();
    })

    $('#btnCancel').click(function () { closePwd(); })

    $('#loginOut').click(function () {
        $.messager.confirm('系统提示', '您确定要退出本次登录吗?', function (r) {

            if (r) {
                location.href = '/Login/Loginout';
            }
        });
    })

});
