
namespace System.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public static class NiuXTaskExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static Task<TResult> ToTaskResult<TResult>(this TResult result) => Task.FromResult(result);
    }
}