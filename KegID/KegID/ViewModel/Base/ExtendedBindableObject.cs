using System;
using System.Linq.Expressions;
using System.Reflection;
using Xamarin.Forms;

namespace KegID.ViewModel.Base
{
    public abstract class ExtendedBindableObject : BindableObject
    {
        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            var operand = lambdaExpression.Body is UnaryExpression body ? (MemberExpression)body.Operand : (MemberExpression)lambdaExpression.Body;
            return operand.Member;
        }
    }

}
