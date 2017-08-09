function getSelected() {
    var row = $('#tt').datagrid('getSelected');
    if (row) {
        alert('Item ID:' + row.itemid + "\nPrice:" + row.listprice);
    }
}

function getSelections() {
    var ids = [];
    var rows = $('#tt').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        ids.push(rows[i].itemid);
    }
    alert(ids.join('\n'));
}


$('#tt').datagrid({
    onDblClickCell: function (index, field, value) {
        Console.log('存在双击');
    }
});

function updateGroups2(rowIndex, field, value) {
    var ids = [];
    var rows = $('#tt').datagrid('getSelections');
    for (var i = 0; i < rows.length; i++) {
        ids.push(rows[i].id);
    }// 获取选中行id

    if (field == 'name') { //判断列名
        $('#tt').datagrid('updateRow', {
            index: rowIndex,//行号
            row: {
                name: '<input  type="text"   id="upname" name="upname"  value="' + value + '"  onblur="updateName(' + ids + ')" />', //name:双击的列名
                iconCls: 'icon-save'
            }
        });
    }
    if (field == 'ip') {
        $('#tt').datagrid('updateRow', {
            index: rowIndex,
            row: {
                ip: '<input  type="text"   id="upip" name="upip"  value="' + value + '"  onblur="updateIp(' + ids + ')" />',
                iconCls: 'icon-save'
            }
        });
    }

}

//这个用行rows写的 一次修改一行
function updateGroups(field, row) {
    $('#tt').datagrid('updateRow', {
        index: field,
        row: {
            name: '<input  type="text"   id="upname" name="upname"  value="' + row.name + '"  onblur="updateName(' + row.id + ')" />',
            ip: '<input  type="text"   id="upname" name="upname"  value="' + row.ip + '"  onblur="updateName(' + row.id + ')" />',
            iconCls: 'icon-save'
        }
    });
}

//这个是离开输入框时间 提交
function updateName(id) {
    var upname = document.getElementById("upname").value;
    if (upname != '') {
        $.ajax({
            type: 'post',
            url: '${basepath}/sys/group/updateName',
            data: { "id": id, "upname": upname },
            success: function (data) {
                $("#tt").datagrid("reload", id);
            }
        });
    }
}