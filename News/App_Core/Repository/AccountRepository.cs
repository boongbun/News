using System;
using System.Data;
using System.Linq;
using News.Models;
using News.Models.Entities;

namespace News.App_Core.Repository
{
    public class AccountRepository : BaseRepository
    {
        BaseRepository repository = new BaseRepository();
        public AccountRepository(UserInfoModel userInfoModel) : base(userInfoModel)
        {

        }

        public ResultModel<SSO_USER> AuthenticationUserExists(UserInfoModel currentUser)
        {
            var result = new ResultModel<SSO_USER>()
            {
                IsError = true,
                Data = new SSO_USER()
            };

            try
            {
                repository.Command.Parameters.Clear();
                repository.AddParameter(":username", currentUser.USER_NAME.ToLower());
                

                result.Data = repository.GetList<SSO_USER>(
                    @"SELECT * FROM TMP_ACCOUNT Where lower(user_name) = :username",
                    CommandType.Text, CustomConnectionState.CloseOnExit).FirstOrDefault();
                if (result.Data !=null )
                {
                    result.IsError = result.Data.ID <= 0;
                }
                else
                {
                    result.IsError = true;
                }
                
            }
            catch (Exception e)
            {
                HandleDAOExceptions(e);
                result = new ResultModel<SSO_USER>() { IsError = true, Message = e.Message, Data = null };
            }
            
            return result;
        }

        

        public ResultModel<UserInfoModel> CheckValidateKey(string key)
        {
            var result = new ResultModel<UserInfoModel>()
            {
                Data = new UserInfoModel()
            };

            try
            {
                repository.Command.Parameters.Clear();
                repository.AddParameter(":key", key);

                var str = @"SELECT a.*,
                       b.session_id,
                       b.expired_date,
                       b.token_key
                  FROM TMP_ACCOUNT a, TMP_ACCOUNT_SESSION b
                 WHERE a.id = b.user_id AND token_key = :key ";

                result.Data = GetList<UserInfoModel>(str, CommandType.Text, CustomConnectionState.CloseOnExit)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                HandleDAOExceptions(e);
                result = new ResultModel<UserInfoModel>() { IsError = true, Message = e.Message, Data = null };
            }
            return result;
        }

        public ResultModel<bool> CheckValidateSession(UserInfoModel param)
        {
            var result = new ResultModel<bool>()
            {
                Data = false
            };

            try
            {
                repository.Command.Parameters.Clear();
                repository.AddParameter(":session_id", param.SessionId);
                repository.AddParameter(":user_id", param.ID);

                var str = @"SELECT *
                              FROM TMP_ACCOUNT_SESSION
                             WHERE user_id = :user_id AND session_id = :session_id";

                var data = GetList<UserInfoModel>(str, CommandType.Text, CustomConnectionState.CloseOnExit)
                    .FirstOrDefault();
                if (data != null)
                {
                    result.Data = data.ID > 0;
                }
                else
                {
                    result.Data = false;
                }
            }
            catch (Exception e)
            {
                HandleDAOExceptions(e);
                result = new ResultModel<bool>() { IsError = true, Message = e.Message, Data = false };
            }
            return result;
        }

        public ResultModel<bool> InsertSessionUser(UserInfoModel param)
        {
            var result = new ResultModel<bool>()
            {
                Data = false
            };
            try
            {
                repository.Command.Parameters.Clear();
                repository.AddParameter(":USER_ID", param.ID);

                var str = "DELETE TMP_ACCOUNT_SESSION WHERE USER_ID = :USER_ID ";
                repository.ExecuteNonQuery(str, CommandType.Text, CustomConnectionState.CloseOnExit);

                repository.Command.Parameters.Clear();
                repository.AddParameter(":SessionId", param.SessionId);
                repository.AddParameter(":USER_ID", param.ID);

                str = @"INSERT INTO TMP_ACCOUNT_SESSION (USER_ID, SESSION_ID) 
                        VALUES (:USER_ID, :SessionId)";
                repository.ExecuteNonQuery(str, CommandType.Text, CustomConnectionState.CloseOnExit);

                result.Data = true;
            }
            catch (Exception e)
            {
                HandleDAOExceptions(e);
                result = new ResultModel<bool>() { IsError = true, Message = e.Message, Data = false };
            }

            return result;
        }

        public ResultModel<bool> UpdateSessionAndTokenUser(SSO_SECURITY_CODE param)
        {
            var result = new ResultModel<bool>()
            {
                Data = false
            };
            try
            {
                repository.Command.Parameters.Clear();
                repository.AddParameter(":USER_ID", param.USER_ID);
                repository.AddParameter(":SESSION_ID", param.SESSION_ID);

                var str = @"MERGE INTO tmp_account_session a
                                USING(SELECT id, user_id
                                    FROM tmp_account_session
                                WHERE user_id = :USER_ID) b
                                ON(a.id = b.id AND a.user_id = b.user_id)
                                WHEN MATCHED
                                THEN
                                    UPDATE SET a.session_id = :session_id
                                    WHEN NOT MATCHED
                                THEN
                                INSERT(USER_ID, SESSION_ID)
                                VALUES(:USER_ID, :SESSION_ID) ";
                repository.ExecuteNonQuery(str, CommandType.Text, CustomConnectionState.CloseOnExit);

                result.Data = true;
            }
            catch (Exception e)
            {
                HandleDAOExceptions(e);
                result = new ResultModel<bool>() { IsError = true, Message = e.Message, Data = false };
            }

            return result;
        }


        public ResultModel<bool> LogOff(UserInfoModel param)
        {
            var result = new ResultModel<bool>()
            {
                Data = false
            };


            return result;
        }
    }
}