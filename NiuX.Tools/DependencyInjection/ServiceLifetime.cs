namespace NiuX.DependencyInjection
{
    /// <summary>
    /// 服务生命周期
    /// </summary>
    public enum ServiceLifetime : sbyte
    {
        /// <summary>
        /// 单例
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// 作用域
        /// </summary>
        Scoped = 1,

        /// <summary>
        /// 瞬态
        /// </summary>
        Transient = 2,
    }
}
