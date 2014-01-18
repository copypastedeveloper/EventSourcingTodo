using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Example.Web.Denormalizer
{
    public class SetOperationList<T> : Dictionary<Expression<Func<T, object>>, object>
    {}
}