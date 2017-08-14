//窗体控件加载完执行
$(function () {
    //无参数时执行此方法
    initTable("");
});

//实现DataGird控件的绑定操作
function initTable(pars) {
    $('#tableShowData').datagrid({
        //定位到Table标签，Table标签的ID是tableShowData
        //fitColumns: true,
        url: '/Home/DataBaseConfigJson', //指向后台的Action来获取当前用户的信息的Json格式的数据
        title: '欢迎使用DBNote,设置数据库连接配置', //表格标题
        iconCls: 'icon-set icon',
        //height: 100,
        nowrap: true,
        loadMsg: '正在加载配置信息...',
        autoRowHeight: false,
        striped: true,
        collapsible: false,
        //pagination: true,
        //singleSelect: true,
        rownumbers: true, //添加列数字
        //sortName: 'ID',    //根据某个字段给easyUI排序
        //sortOrder: 'asc',
        remoteSort: false,
        idField: 'Id', //主键
        queryParams: pars, //异步查询的参数
        //pageList: [5, 10, 15, 20, 25, 30], //分页的分组设置
        //pageSize: 10, //每页的默认值大小
        columns: [
            [
                { title: '全选', checkbox: true },
                { field: 'Id', title: '编号' },
                { field: 'DbType', title: '数据库类型', width: 100 },
                { field: 'DbTypeString', title: '数据库类型', width: 100 },
                { field: 'LinkName', title: '连接名称', width: 150 },
                { field: 'LinkConnectionString', title: '连接地址' }
            ]
        ],
        onLoadSuccess: function () {
            $("#tableShowData").datagrid("hideColumn", "Id"); // 设置隐藏列
            $("#tableShowData").datagrid("hideColumn", "DbType"); // 设置隐藏列
        },
        //表头的按钮
        toolbar: [
            {
                id: 'btnCancle',
                text: '删除',
                iconCls: 'icon-cancel',
                handler: function () {
                    //实现直接删除所有数据的方法
                    Delete();
                }
            }, '-', {
                id: 'btnEdit',
                text: '编辑',
                iconCls: 'icon-edit',
                handler: function () {
                    //编辑所选人员的信息方法
                    Update();
                }
            }, '-', {
                id: 'btnAdd',
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    Add();
                }
            }
        ]
    });
}

//删除选中的行
function Delete() {
    var rows = getMoreSelectedRow();
    var data = {};
    data.requestData = JSON.stringify(rows);
    if (rows.length < 1) {
        msgShow("系统提示", "请选择需要删除的行", "info");
        return;
    }
    $.messager.confirm('系统提示', '您确定要删除记录么?', function (r) {
        if (r) {
            $.ajax({
                url: '/Home/DeleteConfig',
                data: data,
                type: 'POST',
                success: function (data) {
                    if (!data) {
                        msgShow('系统提示', '系统错误', 'error');
                    } else {
                        msgShow('系统提示', data.Des, 'info');
                        ReloadData();
                    }
                }, fail: function () {
                    msgShow('系统提示', '系统错误', 'error');
                }
            });
        }
    });
}

//更新数据
function Update() {
    var rows = getMoreSelectedRow();
    if (rows.length > 1) {
        msgShow('系统提示', '一次只能编辑一行。', 'info');
        return;
    }
    if (rows.length < 1) {
        msgShow('系统提示', '请选择需要编辑的行。', 'info');
        return;
    }
    rows = getSingleSelectedRow();
    var $dbType = $('#DbType');
    var $linkName = $('#LinkName');
    var $linkConnectionString = $("#LinkConnectionString");
    var $configId = $("#configId");
    $dbType.val(rows.DbType);
    $linkName.val(rows.LinkName);
    $linkConnectionString.val(rows.LinkConnectionString);
    $configId.val(rows.Id);
    $('#newConfigWindow').window('open');
}

//添加数据
function Add() {
    $('#newConfigWindow').window('open');
    $("#configId").val(0);
}

//关闭窗口
function closePwd() {
    $('#newConfigWindow').window('close');
}

//修改配置信息
function serverInfo() {
    var $dbType = $('#DbType');
    var $linkName = $('#LinkName');
    var $linkConnectionString = $("#LinkConnectionString");
    var $configId = $("#configId");

    if ($dbType.val() == '') {
        msgShow('系统提示', '请输入数据库类型！', 'warning');
        return false;
    }
    if ($linkName.val() == '') {
        msgShow('系统提示', '请输入连接名称！', 'warning');
        return false;
    }

    if ($linkConnectionString.val() == '') {
        msgShow('系统提示', '请输入连接地址', 'warning');
        return false;
    }
    var data = {};
    data.Id = $configId.val();
    data.LinkName = $linkName.val();
    data.DbType = $dbType.val();
    data.LinkConnectionString = $linkConnectionString.val();
    $.ajax({
        url: "/Home/UpdateOrAddConfig",
        data: data,
        type: 'POST',
        success: function (data) {
            if (!data) {
                msgShow('系统提示', '系统错误', 'error');
                return;
            } else if (data.Code == "0000") {
                $configId.val(0);
                $dbType.val(0);
                $linkName.val('');
                $linkConnectionString.val('');
                closePwd();
            }
            ReloadData();
            msgShow('系统提示', data.Des, 'info');
        }, fail: function () {
            msgShow('系统提示', '系统错误', 'error');
        }
    });
}
/*
*获取当选的数据行
*/
function getSingleSelectedRow() {
    var rowData = $('#tableShowData').datagrid('getSelected');
    return rowData;
}

/*
*获取多选的数据行
*/
function getMoreSelectedRow() {
    var rowData = $('#tableShowData').datagrid('getSelections');
    return rowData;
}

//重新加载表格
function ReloadData() {
    $("#tableShowData").datagrid('reload');
}
