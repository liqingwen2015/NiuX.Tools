using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable CheckNamespace

namespace System.Collections.Generic
{
    public static class NiuXLinqExtensions
    {
        //#region WhereIf

        //public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, bool>> predicate)
        //{
        //    return condition ? source.Where(predicate) : source;
        //}

        //public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool condition, Expression<Func<T, int, bool>> predicate)
        //{
        //    return condition ? source.Where(predicate) : source;
        //}

        //public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
        //{
        //    return condition ? source.Where(predicate) : source;
        //}

        //public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
        //{
        //    return condition ? source.Where(predicate) : source;
        //}

        //#endregion

        #region Distinct

        public static IEnumerable<T> Distinct<T, TComparisonKey>(this IEnumerable<T> source, Func<T, TComparisonKey> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, TComparisonKey>(keySelector));
        }

        public static IEnumerable<T> Distinct<T, TComparisonKey>(this IEnumerable<T> source, Func<T, TComparisonKey> keySelector, IEqualityComparer<TComparisonKey> comparer)
        {
            return source.Distinct(new CommonEqualityComparer<T, TComparisonKey>(keySelector, comparer));
        }

        /// <summary>
        /// https://www.cnblogs.com/CreateMyself/p/12863407.html
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T>(this IEnumerable<T> source, Func<T, T, bool> comparer) where T : class
            => source.Distinct(new DynamicEqualityComparer<T>(comparer));

        private sealed class DynamicEqualityComparer<T> : IEqualityComparer<T>
            where T : class
        {
            private readonly Func<T, T, bool> _func;

            public DynamicEqualityComparer(Func<T, T, bool> func)
            {
                _func = func;
            }

            public bool Equals(T x, T y) => _func(x, y);

            public int GetHashCode(T obj) => 0;
        }

        #endregion

        /// <summary>对一个序列应用累加器函数。</summary>
        /// <param name="source">
        ///   <see cref="T:System.Collections.Generic.IEnumerable`1" /> 对其进行聚合。
        /// </param>
        /// <param name="func">要对每个元素调用的累加器函数。</param>
        /// <typeparam name="TSource">
        ///   中的元素的类型 <paramref name="source" />。
        /// </typeparam>
        /// <returns>累加器的最终值。</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="source" /> 或 <paramref name="func" /> 为 <see langword="null" />。
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///   <paramref name="source" /> 不包含任何元素。
        /// </exception>
        public static TSource Aggregate<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, int, TSource> func)
        {

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (func == null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            using (var enumerator = source.GetEnumerator())
            {
                var index = 0;

                if (!enumerator.MoveNext())
                {
                    throw new InvalidOperationException("NoElements");
                }

                var current = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    current = func(current, enumerator.Current, ++index);
                }

                return current;
            }
        }

        /// <summary>
        /// 遍历每项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null)
            {
                return;
            }

            foreach (var item in source)
            {
                action(item);
            }
        }
    }

    /// <summary>
    /// 通用相等比较器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TComparisonKey"></typeparam>
    public class CommonEqualityComparer<T, TComparisonKey> : IEqualityComparer<T>
    {
        private readonly Func<T, TComparisonKey> _keySelector;
        private readonly IEqualityComparer<TComparisonKey> _comparer;

        public CommonEqualityComparer(Func<T, TComparisonKey> keySelector, IEqualityComparer<TComparisonKey> comparer)
        {
            this._keySelector = keySelector;
            this._comparer = comparer;
        }

        public CommonEqualityComparer(Func<T, TComparisonKey> keySelector) : this(keySelector, EqualityComparer<TComparisonKey>.Default)
        { }

        public bool Equals(T x, T y)
        {
            return _comparer.Equals(_keySelector(x), _keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return _comparer.GetHashCode(_keySelector(obj));
        }
    }
}