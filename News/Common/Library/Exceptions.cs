using System;

namespace News.Common.Library
{
    /// <summary>
    /// Ngoại lệ tại lớp BaseDAO
    /// </summary>
    public class BaseDAOException : Exception
    {

        public BaseDAOException(Exception inner)
            : base("Có lỗi khi thao tác kết nối dữ liệu (BaseDAO)", inner)
        {
        }

    }
    /// <summary>
    /// Ngoại lệ tại lớp DAO
    /// </summary>
    public class RepositoryException : Exception
    {

        public RepositoryException(Exception inner)
            : base("Có lỗi tại Repository", inner)
        {
        }
    }
    /// <summary>
    /// Ngoại lệ tại lớp BO
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(Exception inner)
            : base("Có lỗi tại lớp Business", inner)
        {
        }
    }
    /// <summary>
    /// Ngoại lệ tại lớp WebApplication
    /// </summary>
    public class WebException : Exception
    {

        public WebException(Exception inner)
            : base("Lỗi trên web chương trình.", inner)
        {
        }
    }
    /// <summary>
    /// Xử lý ngoại lệ
    /// </summary>
    public sealed class ExceptionHandlers
    {
        /// <summary>
        /// Xử lý ngoại lệ tại các tầng ứng dụng
        /// </summary>
        /// <param name="ex">Thông tin ngoại lệ</param>
        /// <param name="type">tầng gây ra ngoại lệ</param>
        public static void Handle(Exception ex, ExceptionTypes type)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Loại ngoại lệ
    /// </summary>
    public enum ExceptionTypes
    {
        BASE_DAO_EXCEPTION,
        REPOSITORY_EXCEPTION,
        BUSINESS_EXCEPTION,
        WEB_EXCEPTION
    }

}
