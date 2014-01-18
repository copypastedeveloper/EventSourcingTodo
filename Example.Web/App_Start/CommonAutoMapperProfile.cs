using AutoMapper;
using Example.Common.Commands;
using Example.Web.Models;

namespace Example.Web.App_Start
{
    public class CommonAutoMapperProfile : Profile
	{
        protected override void Configure()
        {
            CreateMap<TaskEntryViewModel, CreateTaskCommand>();
        }
	}
}