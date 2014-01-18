using System;
using System.Linq;

namespace Example.Web.ServiceContracts
{
    public interface IViewModelData
    {
        IQueryable<T> Query<T>();
        T GetById<T>(Guid id);
    }
}