namespace MASA.IoT.Core.Contract.Base
{
    /// <summary>
    /// 列表分页基类
    /// </summary>
    public class PagingOptions
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页条数
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
