using System;
using News.BaseCore.Repository;
using News.Common.Library;
using News.Models;
using News.Models.Entities;
using Newtonsoft.Json;

namespace News.BaseCore.Bussiness
{
    public class AccountBussiness : BaseBussiness
    {
        private readonly AccountRepository repository;

        public AccountBussiness()
        {
            repository = new AccountRepository(new UserInfoModel());
        }

        public ResultModel<SSO_USER> AuthenticationUserExists(UserInfoModel currentUser)
        {
            try
            {
                return repository.AuthenticationUserExists(currentUser);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;
        }

        public ResultModel<bool> InsertSessionUser(UserInfoModel param)
        {
            try
            {
                return repository.InsertSessionUser(param);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;

        }
        public ResultModel<bool> UpdateSessionAndTokenUser(SSO_SECURITY_CODE param)
        {
            try
            {
                return repository.UpdateSessionAndTokenUser(param);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;

        }


        public ResultModel<string> GenerateTokenKey(long nguoidungid, string session)
        {
            var result = new ResultModel<string>();
            try
            {

                var secure = new SSO_SECURITY_CODE()
                {
                    SESSION_ID = session,
                    EXPIRED_TIME = DateTime.UtcNow.AddMinutes(SSOConfig.MinuteExpired),
                    USER_ID = nguoidungid,
                    SECURITY_CODE = Guid.NewGuid().ToString()
                };
                //Cập  nhật key vừa tạo vào CSDL
                
                result.Data = Convert.ToBase64String(Utils.GetBytes(Crypto.EncryptString(SSOConfig.SSOTokenKeyEncrypt, JsonConvert.SerializeObject(secure))));
                result.IsError = false;
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
                result.IsError = true;
                result.Message = ex.Message;
            }
            return result;
        }

        public ResultModel<UserInfoModel> CheckValidateKey(string key)
        {
            try
            {
                return repository.CheckValidateKey(key);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;
        }

        public ResultModel<bool> CheckValidateSession(UserInfoModel param)
        {
            try
            {
                return repository.CheckValidateSession(param);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;
        }

        public ResultModel<bool> LogOff(UserInfoModel param)
        {
            try
            {
                return repository.LogOff(param);
            }
            catch (Exception ex)
            {
                ExceptionHandlers.Handle(ex, ExceptionTypes.BUSINESS_EXCEPTION);
            }
            return null;
        }
    }
}