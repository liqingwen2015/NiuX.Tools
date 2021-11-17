namespace System.Linq.Expressions
{
    /// <summary>
    /// 表达式
    /// </summary>
    /// <remarks>常规的方式不能直接拼接表达式</remarks>
    public static class NiuXExpressionExtensions
    {
        /// <summary>
        /// And
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        /// <remarks>
        /// 不能以直接这样方式拼接，因为两个拼接的参数（如 x）不是同一个参数（如 x），会抛异常
        /// return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, expr2.Body), expr1.Parameters); 
        /// </remarks>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression) => leftExpression.Merge(rightExpression, Expression.And);

        /// <summary>
        /// AndAlso
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression) => leftExpression.Merge(rightExpression, Expression.AndAlso);

        /// <summary>
        /// Or
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression) => leftExpression.Merge(rightExpression, Expression.Or);

        /// <summary>
        /// Or
        /// </summary>
        /// <returns></returns>
        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression) => leftExpression.Merge(rightExpression, Expression.OrElse);

        /// <summary>
        /// Not，取反
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expression) => expression == null ? null : Expression.Lambda<Func<T, bool>>(Expression.Not(expression.Body), expression.Parameters[0]);

        /// <summary>
        /// 合并
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="leftExpression"></param>
        /// <param name="rightExpression"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> Merge<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression, Func<Expression, Expression, Expression> func)
        {
            if (leftExpression == null)
            {
                return rightExpression;
            }

            if (rightExpression == null)
            {
                return leftExpression;
            }

            var newParameter = Expression.Parameter(typeof(T), "x");
            var visitor = new ParameterReplaceExpressionVisitor(newParameter);

            var left = visitor.ReplaceParameter(leftExpression.Body);
            var right = visitor.ReplaceParameter(rightExpression.Body);
            var body = func(left, right);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }


        /// <summary>
        /// 参数替换表达式访问器
        /// </summary>
        private class ParameterReplaceExpressionVisitor : ExpressionVisitor
        {
            private ParameterExpression NewParameterExpression { get; }

            public ParameterReplaceExpressionVisitor(ParameterExpression parameterExpression) => NewParameterExpression = parameterExpression;

            /// <summary>
            /// 替换参数
            /// </summary>
            /// <param name="exp"></param>
            /// <returns></returns>
            public Expression ReplaceParameter(Expression exp) => Visit(exp);

            protected override Expression VisitParameter(ParameterExpression node) => NewParameterExpression;
        }
    }

}