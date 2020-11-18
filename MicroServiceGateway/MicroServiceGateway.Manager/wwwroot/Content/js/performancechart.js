layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });


    var chatData1 = 0;
    var chatData2 = 0;
    var chatData3 = 0;
    var chatData41 = 0;
    var chatData42 = 0;

    function LineChart1(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: '%',
                    max: 100,
                    min: 0,
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: '%',
                    type: 'line',
                    smooth: true,
                    symbol: 'none',
                    sampling: 'average',
                    itemStyle: {
                        color: 'rgb(255, 70, 131)'
                    },
                    areaStyle: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgb(255, 158, 68)'
                        }, {
                            offset: 1,
                            color: 'rgb(255, 70, 131)'
                        }])
                    },
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData1);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart2(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: 'kb',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'mb',
                    type: 'line',
                    smooth: true,
                    symbol: 'none',
                    sampling: 'average',
                    itemStyle: {
                        color: 'rgb(255, 70, 131)'
                    },
                    areaStyle: {
                        color: new echarts.graphic.LinearGradient(0, 0, 0, 1, [{
                            offset: 0,
                            color: 'rgb(255, 158, 68)'
                        }, {
                            offset: 1,
                            color: 'rgb(255, 70, 131)'
                        }])
                    },
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData2);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart3(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: true,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: 'count',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'time',
                    type: 'bar',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData3);
            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }
    function LineChart4(eId, chart_title) {

        var dom = document.getElementById(eId);
        if (dom === undefined) return;
        var myChart = echarts.init(dom);
        var app = {};
        var option = {
            title: {
                text: chart_title
            },
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                data: [chart_title]
            },
            toolbox: {
                show: true,
                feature: {
                    dataView: { readOnly: false },
                    restore: {},
                    saveAsImage: {}
                }
            },
            dataZoom: {
                show: false,
                start: 0,
                end: 100
            },
            grid: {
                x: 50,
                y: 60,
                x2: 30,
                y2: 35
            },
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: (function () {
                        var now = new Date();
                        var res = [];
                        var len = 20;
                        while (len--) {
                            res.unshift(now.toLocaleTimeString().replace(/^\D*/, ''));
                            now = new Date(now - 2000);
                        }
                        return res;
                    })()
                },
                {
                    type: 'category',
                    boundaryGap: false,
                    data: (function () {
                        var res = [];
                        var len = 20;
                        while (len--) {
                            //res.push(len + 1);
                        }
                        return res;
                    })()
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    scale: true,
                    name: 'bit',
                    boundaryGap: [0, '100%']
                }
            ],
            series: [
                {
                    name: 'input',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                },
                {
                    name: 'ouput',
                    type: 'line',
                    data: (function () {
                        var res = [];
                        var len = 0;
                        while (len < 20) {
                            res.push((Math.random() * 10 + 20).toFixed(1) - 0);
                            len++;
                        }
                        return res;
                    })()
                }
            ]
        };

        clearInterval(app.timeTicket);

        app.count = 22;

        app.timeTicket = setInterval(function () {

            var data0 = option.series[0].data;
            data0.shift();
            data0.push(chatData41);

            var data1 = option.series[1].data;
            data1.shift();
            data1.push(chatData42);

            option.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option.xAxis[0].data.push(axisData);
            myChart.setOption(option);

        }, 1100);


        if (option && typeof option === "object") {
            var startTime = +new Date();
            myChart.setOption(option, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }


    //加载图表

    LineChart1("msgnode-cpu-div", "cpu");
    LineChart2("msgnode-mem-div", "memory");
    LineChart3("msgnode-cmd-div", "threads");
    LineChart4("msgnode-net-div", "net");

    var name = decodeURI(GetRequest().name);

    function refresh() {
        $.get("/api/msgnode/getperformance", "nodename=" + name, function (sdata) {

            layer.close(layerIndex);

            if (sdata.Code === 1) {
                chatData1 = sdata.Data.CPU;
                chatData2 = sdata.Data.MemoryUsage;
                chatData3 = sdata.Data.TotalThreads;
                chatData41 = sdata.Data.BytesRec;
                chatData42 = sdata.Data.BytesSen;
            }
            else {
                if (sdata.Message.indexOf("连接失败") > -1) {
                    parent.location.reload();
                }
                else
                    layer.msg("操作失败:" + sdata.Message, { time: 2000 });
            }
        });
    }

    setInterval(function () { refresh(); }, 1000);

});