using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Example.Common.Testing
{
    public static class ObjectExtensions
    {
        public static T As<T>(this object objectToCast) where T : class
        {
            return objectToCast as T;
        }

        public static T SetValue<T>(this T objectToApplyTo, Expression<Func<T, object>> expression, object value)
        {
            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                var ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            if (body == null || body.Member.MemberType != MemberTypes.Property)
                throw new UnsupportedExpressionType(expression.Body.NodeType);

            var property = (PropertyInfo)body.Member;
            var setMethod = property.GetSetMethod(true);

            var propertyType = ((PropertyInfo)body.Member).PropertyType;
            var parameterT = Expression.Parameter(body.Member.ReflectedType, "x");
            var parameterTProperty = Expression.Parameter(propertyType, "y");

            var actionType = typeof(Action<,>).MakeGenericType(body.Member.ReflectedType, propertyType);
            var lambdaMethod =
                typeof(Expression).GetMethods()
                    .First(x => x.Name == "Lambda" && x.IsGenericMethod)
                    .MakeGenericMethod(actionType);

            var setExpression = (LambdaExpression)lambdaMethod.Invoke(null, new object[]
                {
                    Expression.Call(parameterT, setMethod, parameterTProperty),
                    new[]
                    {
                        parameterT,
                        parameterTProperty
                    }
                });

            setExpression.Compile().DynamicInvoke(GetTarget(objectToApplyTo, body.Expression), value);

            return objectToApplyTo;
        }

        static object GetTarget(object currentLevel, Expression expr)
        {
            switch (expr.NodeType)
            {
                case ExpressionType.Parameter:
                    return currentLevel;
                case ExpressionType.MemberAccess:
                    var mex = (MemberExpression)expr;
                    var pi = mex.Member as PropertyInfo;
                    if (pi == null) throw new ArgumentException();
                    object target = GetTarget(currentLevel, mex.Expression);
                    return pi.GetValue(target, null);
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public class UnsupportedExpressionType : Exception
    {
        public UnsupportedExpressionType(ExpressionType nodeType)
            : base(string.Format("Expressions of type {0} are not currently supported", nodeType))
        {
        }
    }
}
