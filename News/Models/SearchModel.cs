using System.Collections.Generic;


namespace News.Models
{
    public class SearchModel<T>
    {
        /// <summary>
        /// Tiêu chí tìm kiếm
        /// </summary>
        public T Cretia { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string ColumnOrder { get; set; }
        public int DirectionOrder { get; set; }
        public long TinhThanhId { get; set; }
        //string columnOrder, long? directionOrder, int pageIndex, int pageSize, out int totalRecord
    }
}
