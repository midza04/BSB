
var MONTHS = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
var randomScalingFactor = function () {
    return (Math.random() > 0.5 ? 1.0 : 0.0) * Math.round(Math.random() * 100);
};
var randomColorFactor = function () {
    return Math.round(Math.random() * 255);
};
var randomColor = function () {
    return 'rgba(' + randomColorFactor() + ',' + randomColorFactor() + ',' + randomColorFactor() + ',.7)';
};

window.onload = function () {
};

$(function () {

    var url = window.location.href;     // Returns full URL
    Chart.defaults.global.responsive = true;

    $.ajax({
        type: "GET",
        url: url + "/api/Charts",
        datatype: "json",
        success: function (result) {
            var ctx = document.getElementById("dashReport").getContext("2d");
            var data = JSON.parse(result);
            var barChartData = {
                labels: data.dates,
                options: {
                    responsive: true
                },                
                datasets: [{
                    label: 'Dataset 1',
                    backgroundColor: "rgba(220,220,220,0.5)",
                    data: data.recordCount
                }]
            };

            window.myBar = new Chart(ctx).Bar(barChartData);
        }
    });


    // Reference the auto-generated proxy for the hub.  
    var chat = $.connection.dashboardHub;

    chat.client.updateNumber = function (number) {
        $("#number").html(number);
    }

    chat.client.throwError = function (error) {
        console.log(error);
    };

    chat.client.testingMethod = function(x){
        console.log(x);
    }
    
    chat.client.updateServiceStatus = function (statusModel) {
        updateUIStatus("service-status", statusModel.serviceStatus, "service-status-wrapper");
        updateUIStatus("sql-status", statusModel.sqlStatus, "sql-status-wrapper");
        //$("#sql-status").html(statusModel.sqlStatus);
        //$("#cpu").html(statusModel.cpuUsage);
        $("#ram-available").html(statusModel.ramUsage);
        $("#space-available").html(statusModel.spaceAvailable);
    };

    chat.client.startService = function (response) {
        if (response === "Done") {
            $("#toggleService")
                .html("Stop Service")
                .removeClass("bk-success-dashboard")
                .addClass("btn-danger-dashboard")
                .data("servicestate", "Started");
        }
    };

    chat.client.stopService = function (response) {
        if (response === "Done") {
            $("#toggleService").html("Start Service").removeClass("btn-danger").addClass("btn-success").data("servicestate", "Stopped");
        }
    };

    chat.client.startSQLService = function (response) {
        if (response === "Done") {
            $("#toggleSQLService")
                .html("Stop Service")
                .removeClass("bk-success-dashboard")
                .addClass("btn-danger-dashboard")
                .data("servicestate", "Started");
        }
    };

    chat.client.stopSQLService = function (response) {
        if (response === "Done") {
            $("#toggleSQLService").html("Start Service").removeClass("btn-danger").addClass("btn-success").data("servicestate", "Stopped");
        }
    };


    // Start the connection.
    $.connection.hub.start().done(function () {
        setInterval(getServiceStatus, 1000);

        $("#restartService").on("click", function () {
            restartService();
        });

        $("#toggleService").on("click", function () {
            var state = $("#toggleService").data("servicestate");

            if (state == "Started"){
                stopService();
            }
            else if (state == "Stopped") {
                startService();
            }
            
        });

        $("#restartSQL").on("click", function () {
            restartSQLService();
        });

        $("#toggleSQLService").on("click", function () {
            var state = $("#toggleSQLService").data("servicestate");

            if (state == "Started") {
                stopSQLService();
            }
            else if (state == "Stopped") {
                startSQLService();
            }

        });

    });


    function startService() {
        chat.server.startDBFFintechService();
    }

    function stopService() {
        chat.server.stopDBFFintechService();
    }

    function restartService() {
        chat.server.restartDBFFintechService();
    }

    function startSQLService() {
        chat.server.startSQLService();
    }

    function stopSQLService() {
        chat.server.stopSQLService();
    }

    function restartSQLService() {
        chat.server.restartSQLService();
    }

    function getServiceStatus() {
        chat.server.serviceStatus();
    }

    function updateButtonStatus(elementID, status) {

    }

    function updateUIStatus(elementID, status, statusWrapper) {
        $("#" + elementID).html(status);
        if (status == "Stopped") {
            $("#" + statusWrapper).removeClass("bk-success-dashboard").addClass("bk-danger-dashboard");
        }
        else if (status == "Running" && ($("#" + statusWrapper).hasClass("bk-success-dashboard") == false)) {
            $("#" + statusWrapper).removeClass("bk-danger-dashboard").addClass("bk-success-dashboard");
        }
    }

});
