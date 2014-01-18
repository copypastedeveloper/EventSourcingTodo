function TaskViewModel() {
	var self = this;

	self.Id = {};
	self.Description = {};
	self.Name = {};
    self.DueDate = ko.observable();
	self.Notes = ko.observableArray();
    
	self.CompleteTask = function () {
		Global.Api.Task.Complete(self.Id, function () {
			toastr.info("Task has been marked as completed.<br>" + self.Name);
			Global.Routing.Redirect('/');
		});
	};

    self.AddNote = function(text) {
        //whatever
    };
}
