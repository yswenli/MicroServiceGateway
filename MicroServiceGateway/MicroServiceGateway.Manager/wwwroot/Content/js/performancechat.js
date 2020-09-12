﻿layui.use(['jquery', 'layer', 'form'], function () {

    var layer = layui.layer, form = layui.form, $ = layui.$;

    var layerIndex = -1;

    layerIndex = layer.msg('加载中', {
        icon: 16
        , shade: 0.01
    });

    //
    function LineChart1(eId, chart_title, redis_info_url, redis_name) {

        var dom1 = document.getElementById(eId);
        if (dom1 === undefined) return;
        var myChart1 = echarts.init(dom1);
        var app1 = {};
        var option1 = null;
        option1 = {
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
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
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

        clearInterval(app1.timeTicket);

        app1.count = 22;

        app1.timeTicket = setInterval(function () {

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=1", function (redis_info_data) {
                //
                var data0 = option1.series[0].data;
                if (redis_info_data.Code === 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option1.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option1.xAxis[0].data.push(axisData);
            myChart1.setOption(option1);

        }, 1100);


        if (option1 && typeof option1 === "object") {
            var startTime = +new Date();
            myChart1.setOption(option1, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }

    function LineChart2(eId, chart_title, redis_info_url, redis_name) {
        var dom2 = document.getElementById(eId);
        if (dom2 === undefined) return;
        var myChart2 = echarts.init(dom2);
        var app2 = {};
        var option2 = null;
        option2 = {
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
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
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

        clearInterval(app2.timeTicket);

        app2.count = 22;

        app2.timeTicket = setInterval(function () {

            $.get(redis_info_url, "name=" + redis_name + "&isCpu=0", function (redis_info_data) {
                //
                var data0 = option2.series[0].data;
                if (redis_info_data.Code === 2) {
                    data0.shift();
                    data0.push(-1);
                }
                else {
                    //
                    data0.shift();
                    data0.push(redis_info_data.Data);
                }
            });
            option2.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option2.xAxis[0].data.push(axisData);
            myChart2.setOption(option2);

        }, 1200);


        if (option2 && typeof option2 === "object") {
            var startTime = +new Date();
            myChart2.setOption(option2, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
    }


    var chatData1 = 0;

    function LineChart11(eId, chart_title, redis_name) {

        var dom1 = document.getElementById(eId);
        if (dom1 === undefined) return;
        var myChart1 = echarts.init(dom1);
        var app1 = {};
        var option1 = null;
        option1 = {
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
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
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

        clearInterval(app1.timeTicket);

        app1.count = 22;

        app1.timeTicket = setInterval(function () {

            var data0 = option1.series[0].data;
            data0.shift();
            data0.push(chatData1);
            option1.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option1.xAxis[0].data.push(axisData);
            myChart1.setOption(option1);

        }, 1100);


        if (option1 && typeof option1 === "object") {
            var startTime = +new Date();
            myChart1.setOption(option1, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }

        var ws = new WebSocket(`ws://${document.domain}:16666/`);
        ws.onopen = function (evt) {
            console.log("Connection open ...");
            ws.send("getinfo");
            ws.send(`{"Name":"${redis_name}","IsCpu":true}`);
        };
        ws.onmessage = function (event) {

            var redis_info_data = JSON.parse(event.data);

            if (redis_info_data.Code === 1) {

                chatData1 = redis_info_data.Data;
            }
        };
    }

    var chatData2 = 0;

    function LineChart22(eId, chart_title, redis_name) {


        var dom2 = document.getElementById(eId);
        if (dom2 === undefined) return;
        var myChart2 = echarts.init(dom2);
        var app2 = {};
        var option2 = null;
        option2 = {
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
                    name: '使用率',
                    max: 100,
                    min: 0,
                    boundaryGap: [0.2, 0.2]
                }
            ],
            series: [
                {
                    name: '使用率',
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

        clearInterval(app2.timeTicket);

        app2.count = 22;

        app2.timeTicket = setInterval(function () {

            var data0 = option2.series[0].data;
            data0.shift();
            data0.push(chatData2);
            option2.xAxis[0].data.shift();
            var axisData = (new Date()).toLocaleTimeString().replace(/^\D*/, '');
            option2.xAxis[0].data.push(axisData);
            myChart2.setOption(option2);

        }, 1200);


        if (option2 && typeof option2 === "object") {
            var startTime = +new Date();
            myChart2.setOption(option2, true);
            var endTime = +new Date();
            var updateTime = endTime - startTime;
            // console.log("Time used:", updateTime);
        }
        var ws = new WebSocket(`ws://${document.domain}:16666/`);
        ws.onopen = function (evt) {
            console.log("Connection open ...");
            ws.send("getinfo");
            ws.send(`{"Name":"${redis_name}","IsCpu":false}`);
        };
        ws.onmessage = function (event) {

            var redis_info_data = JSON.parse(event.data);

            if (redis_info_data.Code === 1) {
                chatData2 = redis_info_data.Data;
            }
        };
    }

    var name = decodeURI(GetRequest().name);

    //加载图表
    //LineChart1("redis-cpu-div", "cpu使用情况", "/api/redis/getinfo", name);
    //LineChart2("redis-mem-div", "memory使用情况", "/api/redis/getinfo", name);

    LineChart11("redis-cpu-div", "cpu使用情况", name);
    LineChart22("redis-mem-div", "memory使用情况", name);


    $("#redis_name").html(name);


    layer.close(layerIndex);

});