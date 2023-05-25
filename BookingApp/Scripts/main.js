//var API_URL = "http://171.244.38.75:8989";
var API_URL = "http://localhost:57874";

function callAPI(endpoint, method, body, success_callback, error_callback) {
    console.log(endpoint);
    $.ajax({
        url: API_URL + endpoint,
        //data: JSON.stringify(body),
        success: function (json) {
            success_callback && success_callback(json);
        },
        error: function (xhr, textStatus, errorThrown) {
            console.log(xhr, textStatus, errorThrown);
            error_callback && error_callback(xhr.responseText);
        },

        beforeSend: function (xhr) {
            xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("TOKEN"));
        },
        type: method,
        //contentType: 'application/json',
    });
}

function callAPIAnonymous(endpoint, method, body, success_callback, error_callback) {
    $.ajax({
        url: API_URL + endpoint,
        data: JSON.stringify(body),
        success: function (json) {
            success_callback && success_callback(json);
        },
        error: function (xhr, textStatus, errorThrown) {
            error_callback && error_callback($.parseJSON(xhr.responseText));
        },
        type: method,
        contentType: 'application/json',
    });
}

function goTo(action, controller, data) {
    callAPI("/" + controller + "/" + action, "GET", data,
        function (res) {
            $("#page_content").html(res);
        },
        function (error) {
            $("#page_content").html(error);
        });
}