using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Security.Principal;
using System.Web.Http;
using AutoMapper;
using MedArchon.Common.Commands;
using MedArchon.Common.Commands.Bus;
using MedArchon.Web.Infrastructure;
using MedArchon.Web.Models;
using MedArchon.Web.ServiceContracts;
using MedArchon.Web.ViewModels;

namespace MedArchon.Web.Controllers
{
    public class TaskController : ApiController
    {
        readonly ICommandBus _commandBus;
        readonly IMappingEngine _mappingEngine;
        readonly IIdentity _identity;
        readonly IViewModelData _viewModelData;

        public TaskController(ICommandBus commandBus, IMappingEngine mappingEngine, IIdentity identity, IViewModelData viewModelData)
        {
            _commandBus = commandBus;
            _mappingEngine = mappingEngine;
            _identity = identity;
            _viewModelData = viewModelData;
        }
        
        public HttpResponseMessage Post(TaskEntryViewModel model)
        {
            var command = _mappingEngine.Map<CreateTaskCommand>(model);

            var response = _commandBus.Send(command);
            return response.Success ? SuccessResponseMessage() : ErrorResponseMessage(response);
        }

        public IEnumerable<TaskListViewModel> Get()
        {
            return _viewModelData.Query<TaskListViewModel>();
        }

        public TaskViewModel Get(Guid id)
        {
            return _viewModelData.GetById<TaskViewModel>(id);
        }

        public HttpResponseMessage PutCompleteTask(Guid id)
        {
            var command = new CompleteTaskCommand
            {
                TaskId = id
            };

            var response = _commandBus.Send(command);
            return response.Success ? SuccessResponseMessage() : ErrorResponseMessage(response);
        }

        public HttpResponseMessage PutReopenTask(Guid id)
        {
            var command = new ReopenTaskCommand
            {
                TaskId = id
            };

            var response = _commandBus.Send(command);
            return response.Success ? SuccessResponseMessage() : ErrorResponseMessage(response);
        }

        public static HttpResponseMessage ErrorResponseMessage(CommandResponse response)
        {
            //todo: don't return gibberish, make real messages for the enums (resx or something)
            var errorMessages = new ContentMessage
            {
                ErrorMessages = response.StatusCodes.Select(statusCode => statusCode.ToString()).ToList()
            };

            //todo: find a way to pick the media type formatter based on request accept headers.
            return new HttpResponseMessage((HttpStatusCode)422)
            {
                Content = new ObjectContent(errorMessages.GetType(), errorMessages, new JsonMediaTypeFormatter())
            };
        }

        static HttpResponseMessage SuccessResponseMessage()
        {
            return new HttpResponseMessage()
            {
                Content = new ObjectContent<object>(new { Success = true }, new JsonMediaTypeFormatter())
            };
        }
    }
}
