
function OpenPopup(modalId, modalContainId, url, FormId) {
    loaderstart();
    $('#' + modalContainId + '').empty();
    $('#' + modalContainId + '').load(url, function (response, status, xhr) {
        if (response.indexOf("<meta charset=\"utf-8\" />") != -1) {
            $('#' + modalId + '').modal('hide');
            location.reload();
            return;
        }
        $('#' + modalId + '').modal({
            keyboard: true
        }, 'show');
        bindForm(this, modalId, modalContainId, FormId);
        loaderstop();
    });
}

function loaderstart() {
    $('#loading-image').show();
    $('#loading-image .bg').height('100%');
    $('#loading-image').fadeIn(300);
    $('body').css('cursor', 'wait');
}

function loaderstop() {
    $('#loading-image').hide();
    $('#loading-image .bg').height('100%');
    $('#loading-image').fadeOut(300);
    $('body').css('cursor', 'default');
}

function bindForm(dialog, modalId, modalContainid, FormId) {
    $('#' + FormId + '', dialog).submit(function () {
        if ($('#' + FormId + '').valid()) {
            loaderstart();
            $.ajax({
                url: this.action,
                type: this.method,
                cache: false,
                data: $(this).serialize(),
                crossDomain: true,
                success: function (html, status, xhr) {
                    if (html.success) {
                        $('#' + modalId + '').modal('hide');
                        if (html.url == "/Admin/Payments") {
                            if ($('#example').find("tr").length > 0)
                                $('#example').DataTable().ajax.reload(null, false);
                            var str = '<div class="alert alert-success" id="errordiv"><button class="close" data-close="alert" id="closeMsg" type= "button"></button><span>Payment updated successfully..</span></div >';
                            window.parent.document.getElementById("ErrorMsg").innerHTML = str;
                        }
                        else if (html.url != null) {
                            location.href = html.url;
                        }
                        else {
                            location.reload();
                        }
                    } else {
                        $('#' + modalContainid + '').html(html);

                        bindForm(dialog, modalId, modalContainid, FormId);
                    }
                    loaderstop();
                },
                error: function (e) {
                    loaderstop();
                }
            });
            return false;
        }
    });
}

function Delete(e, url, msg, yes, no, fn_callback) {
    bootbox.confirm({
        message: msg,
        buttons: {
            confirm: {
                label: yes,
                className: 'btn-success'
            },
            cancel: {
                label: no,
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                //loaderstart();
                var data = { Id: parseInt($(e).attr('data-id')) }
                $.post(url, data, function (data) {
                    if (fn_callback)
                        fn_callback(data);
                    else
                        location.reload();
                });
                //loaderstop();
            }
        }
    });
}

function ChangeStatus(e, url, msg, yes, no, fn_callback) {
    bootbox.confirm({
        message: msg,
        buttons: {
            confirm: {
                label: yes,
                className: 'btn-success'
            },
            cancel: {
                label: no,
                className: 'btn-danger'
            }
        },
        callback: function (result) {
            if (result) {
                var data = { Id: parseInt($(e).attr('data-id')) }
                $.post(url, data, function (data) {
                    if (fn_callback)
                        fn_callback(data);
                    else
                        location.reload();
                    //$('#example').DataTable().ajax.reload(null, false);
                });
            }
        }
    });
}

function ConvertToBase64String(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}

function ChangeLanguage(lngId) {
    $.ajax({
        url: '/Admin/Login/ChangeLanguage?lng=' + lngId,
        dataType: "json",
        type: "GET",
        contentType: 'application/json; charset=utf-8',
        async: true,
        processData: false,
        cache: false,
        success: function (data) {
            window.location.reload();
        },
        error: function (xhr) {
            alert('error');
        }
    });
}

function Rating(data, outof) {
    var str = "";
    var cnt = 0;
    for (var i = cnt; i < data; i++) {
        cnt++;
        str += '<i class="fa fa-star rating-star-yellow"></i>';
    }
    for (var i = cnt; i < outof; i++) {
        cnt++;
        str += '<i class="fa fa-star-o"></i>';
    }
    return str;
}

function SetActiveTab(elmId) {
    $($("#" + elmId + "").parent().parent().find('a')[0]).trigger("click");
    $("#" + elmId + "").addClass("active");
    $("#" + elmId + "").parent().parent("li").addClass("open");
    $("#" + elmId + "").parent("ul").show();
}

function MakeDataTableResponsive(tblId) {
    $("#" + tblId + " tr th").each(function (e, index) {
        $("#" + tblId + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());;
    });
}