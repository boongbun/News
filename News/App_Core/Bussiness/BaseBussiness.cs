using System;
using News.Common.Library;

namespace News.App_Core.Bussiness
{
    public class BaseBussiness
    {
        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void HandleBOExceptions(Exception ex)
        {
            ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
        }
    }

}
