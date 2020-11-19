layui.config({
    base: '/content/lay/modules/layui/'
    , version: '1531663423583'
}).use('global');

layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;
    //
    var layerIndex = -1;

    //index列表
    $(function () {

        var atimer = setInterval(function () {
            $.get("/user/authenticated", null, function (adata) {
                if (adata.Code === 1 && adata.Data === false) {
                    layer.msg("当前操作需要登录", { time: 2000 }, function () {
                        clearInterval(atimer);
                        location.href = "/login.html";
                    });
                }
            });
        }, 2000);


        layerIndex = layer.msg('加载中', {
            icon: 16
            , shade: 0.3
        });

        //搜索

        $("#search_list").keyup(function () {

            var searchText = $(this).val();

            if (searchText === "") {
                $(".index_link").each(function (index) {
                    $(this).parent().show();
                });
            }

            $(".index_link").each(function (index) {
                if ($(this).text().indexOf(searchText) === -1 && $(this).attr("title").indexOf(searchText) === -1) {
                    $(this).parent().hide();
                }
                else {
                    $(this).parent().show();
                }
            });
        });

        //默认加载msgnodes列表
        $.get("/api/msgnode/getlist", null, function (data) {

            layer.close(layerIndex);

            if (data.Code === 1) {

                if (data.Data !== undefined && data.Data !== null && data.Data.length > 0) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var html = `<dd class="layui-nav-itemed">
                                <a class='index_link' href="javascript:;" data-name='${data.Data[i].NodeName}' title='${JSON.stringify(data.Data[i])}'>&nbsp;&nbsp;<i class="layui-icon layui-icon-senior"></i> ${data.Data[i].NodeName}</a>                                
                            </dd>`;
                        $("dl.msgnodes").append(html);
                    }

                    //点击微服务网关实例
                    $("dl.msgnodes a.index_link").on("click", function () {
                        
                        var name = encodeURI($(this).attr("data-name"));

                        //var _parent = $(this).parent();
                        //var isLoaded = _parent.attr("data-loaded");
                        //if (isLoaded !== undefined) {
                        //    _parent.removeAttr("data-loaded");
                        //    _parent.find("dl").remove();
                        //    return;
                        //}
                        //_parent.attr("data-loaded", "1");

                        $(".layadmin-iframe").attr("src", "/nodechart.html?name=" + encodeURI(name));
                    });
                }
            }
            else if (data.Code === 3) {
                layer.msg("操作失败:" + data.Message, { time: 2000 }, function () {
                    location.href = "/login.html";
                });
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });

        //默认加载ms列表
        $.get("/api/ms/getvirtualaddress", null, function (data) {

            layer.close(layerIndex);

            if (data.Code === 1) {

                if (data.Data !== undefined && data.Data !== null && data.Data.length > 0) {
                    for (var i = 0; i < data.Data.length; i++) {
                        var html = `<dd class="layui-nav-itemed">
                                <a class='va_link' href="javascript:;" data-name='${data.Data[i]}' title='${JSON.stringify(data.Data[i])}'>&nbsp;&nbsp;<i class="layui-icon layui-icon-senior"></i> ${data.Data[i]}</a>                                
                            </dd>`;
                        $("dl.ms").append(html);
                    }

                    //点击微服务实例
                    $("dl.ms a.va_link").on("click", function () {
                        
                        var virtualaddress = encodeURI($(this).attr("data-name"));

                        //var _parent = $(this).parent();
                        //var isLoaded = _parent.attr("data-loaded");
                        //if (isLoaded !== undefined) {
                        //    _parent.removeAttr("data-loaded");
                        //    _parent.find("dl").remove();
                        //    return;
                        //}
                        //_parent.attr("data-loaded", "1");

                        $(".layadmin-iframe").attr("src", "/mslist.html?virtualaddress=" + encodeURI(virtualaddress));
                    });
                }
            }
            else if (data.Code === 3) {
                layer.msg("操作失败:" + data.Message, { time: 2000 }, function () {
                    location.href = "/login.html";
                });
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });


    //添加node按钮
    $("#add_link").on("click", function () {
        layer.open({
            title: 'Add MSGNode',
            type: 2,
            area: ['580px', '320px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/nodeadd.html', 'no'],
            end: function () {
                location.reload();
            }
        });
    });

    //移除node按钮
    $("#rem_link").on("click", function () {
        layer.open({
            title: 'Delete MSGNode',
            type: 2,
            area: ['580px', '180px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/nodedel.html', 'no']
        });
    });
    //node server configs按钮
    $("#conf_link").on("click", function () {
        layer.open({
            title: 'MSGNode Configs',
            type: 2,
            area: ['670px', '560px'],
            fixed: true,
            resize: false,
            move: false,
            maxmin: false,
            time: 0,
            content: ['/configs.html', 'no']
        });
    });

    //users list按钮
    $("#account_link").on("click", function () {
        $(".layadmin-iframe").attr("src", "/userlist.html");
    });

    //提交添加node表单
    $("#add_btn").on("click", function () {
        //var str = $("#add_form").serialize(); //layui jquery bug
        var name = encodeURIComponent($("input[name='name']").val());
        var ip = encodeURIComponent($("input[name='ip']").val());
        var port = encodeURIComponent($("input[name='port']").val());
        var password = encodeURIComponent($("input[name='password']").val());
        var str = `name=${name}&ip=${ip}&port=${port}&password=${password}`;
        $.post("/api/config/set", str, function (data) {
            if (data.Code === 1) {
                parent.location.reload();
            }
            else if (data.Code === 3) {
                layer.msg("操作失败:" + data.Message, { time: 2000, shade: 0.3, shadeClose: false }, function () {
                    top.location.href = "/login.html";
                });
            }
            else {
                layer.msg("操作失败:" + data.Message, { time: 2000 });
            }
        });
    });
});

