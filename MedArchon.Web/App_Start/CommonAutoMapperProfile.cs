using AutoMapper;
using MedArchon.Common.Commands;
using MedArchon.Web.Models;

namespace MedArchon.Web.App_Start
{
    public class CommonAutoMapperProfile : Profile
	{
        protected override void Configure()
        {
            CreateMap<TaskEntryViewModel, CreateTaskCommand>();
        }
	}
}