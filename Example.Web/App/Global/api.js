$(function () {
    $(document).ajaxError(
        function (e, jqxhr) {
            if (jqxhr.status == 422) {
                var messages = $.parseJSON(jqxhr.responseText).ErrorMessages;
                if (messages != undefined) {
                    toastr.error(messages.join());
                    return;
                }
            }

            window.location.href = '/Error';
        }
    );
});

Global = window.Global || {};
Global = {
    Api: {
        __ajax: {
            post: function (url, data, callback) {
                return $.ajax({
                    type: "POST",
                    url: url,
                    data: JSON.stringify(data),
                    success: callback,
                    dataType: "json",
                    contentType: 'application/json',
                    accept: "application/json"
                });
            },
            put: function (url, data, callback) {
                return $.ajax({
                    type: "PUT",
                    url: url,
                    data: JSON.stringify(data),
                    success: callback,
                    dataType: "json",
                    contentType: 'application/json',
                    accept: "application/json"
                });
            },
            getJson: function (url) {
                if (arguments.length == 2)
                    return $.getJSON(url, arguments[1]);
                if (arguments.length == 3)
                    return $.getJSON(url, arguments[1], arguments[2]);

                throw new Error("invalid number of parameters");
            }
        },
        Task: {
            List: function (callback) {
                return Global.Api.__ajax.getJson("/api/Task/Get", callback);
            },
            Add: function (task, callback) {
                return Global.Api.__ajax.post('/api/Task/Post', task, callback);
            },
            View: function (taskId, callback) {
                return Global.Api.__ajax.getJson("/api/Task/Get/" + taskId, callback);
            },
            Complete: function (taskId, callback) {
                return Global.Api.__ajax.put('/api/Task/PutCompleteTask/'+taskId, null, callback);
            },
            Reopen: function (taskId, callback) {
                return Global.Api.__ajax.put('/api/Task/PutReopenTask/'+taskId, null, callback);
            }
            //,
            //AddNote: function (value, taskId, callback) {
            //    return Global.Api.__ajax.getJson("/api/Task/PutAddNote/" + taskId, value, callback);
            //}
        }
    }
}