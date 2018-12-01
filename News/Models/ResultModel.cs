using System.Collections.Generic;


namespace News.Models
{
    public class ResultModel<T>
    {
        public ResultModel()
        {
            IsError = false;
            Message = string.Empty;
        }

        public ResultModel(bool isError, string message, T data)
        {
            IsError = isError;
            if (isError)
            {
                Message = string.Format("{0}", message);
            }
            else
            {
                Message = message;
                Data = data;
            }
        }
        public ResultModel(bool isError, string errorMessage, string message, T data)
        {
            IsError = isError;
            if (isError)
            {
                Message = string.Format("{0}: {1}", errorMessage, message);
            }
            else
            {
                Message = message;
                Data = data;
            }
        }
        public T Data { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }

    public class ListPagedResultModel<T>
    {
        public ListPagedResultModel()
        {
            IsError = false;
            Message = string.Empty;
        }
        public List<T> ListItem { get; set; }
        public int TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
