function TaskListViewModel() {
    var self = this;
    self.Tasks = ko.observableArray();

    self.Add = function () {
        $("#reusableModal").remove();
        var modal = $('<div id="reusableModal" class="modal hide fade" tabindex="-1" role="dialog" aria-hidden="true">/>');
        modal.load('/App/Task/TaskEntryView.html', function() {
            ko.applyBindings(new TaskEntryViewModel(), modal[0]);
            modal.modal();
        });
    };
}

function TaskListItem(task) {
    self = this;
    self.Id = task.Id;
    self.Name = task.Name;
    self.Completed = ko.observable(task.Completed);

    self.Completed.subscribe(function(newValue) {
        if (newValue) {
            Global.Api.Task.Complete(self.Id, function() {
                toastr.success("Task Completed");
            });
        } else {
            Global.Api.Task.Reopen(self.Id, function () {
                toastr.success("Task Reopened");
            });
        }
    }, self);
}