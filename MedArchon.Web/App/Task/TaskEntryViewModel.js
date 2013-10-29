function TaskEntryViewModel(caseId, documentType, grid) {
    var self = this;
    self.Name = ko.observable().extend({ required: true });
    self.Description = ko.observable();
    self.DueDate = ko.observable();
    
    self.errors = ko.validation.group(self);

    self.Create = function (data,e) {
        if (!self.isValid()) {
            e.stopPropagation();
            toastr.info("Please enter all required fields.");
            return;
        }

        Global.Api.Task.Add(ko.mapping.toJS(self), function () {
            toastr.success('Added a task.');
        });
    };
}