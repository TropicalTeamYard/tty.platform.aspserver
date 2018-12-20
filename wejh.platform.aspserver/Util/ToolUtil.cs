using System;
using System.Data;
using wejh.Model;

namespace wejh.Util
{
    public static class ToolUtil
    {
        public static ResponceModel AutoLogin(string credit)
        {
            if (credit == null)
            {
                return ResponceModel.GetInstanceInvalid();
            }
            else
            {
                DataTable data = null;
                try
                {
                    int flag = -1;
                    if (MySqlUtil.TryQuery(UserSql.GetQuerycommandMobileCredit(credit), out var dataTable1))
                    {
                        data = dataTable1;
                        flag = 0;
                    }
                    if (MySqlUtil.TryQuery(UserSql.GetQuerycommandPcCredit(credit), out var dataTable2))
                    {
                        data = dataTable2;
                        flag = 1;
                    }

                    if (flag == -1)
                    {
                        return new ResponceModel(403, "自动登录失败。");
                    }
                    else
                    {
                        //验证账户。
                        UserSql userSql = UserSql.FromDataRow(data.Rows[0]);

                        var result = Function.JhUserFunc.CheckJhUser(userSql.username, userSql.password);

                        if (result.code == 200)
                        {
                            //移动端登录
                            if (flag == 0)
                            {
                                userSql.mobile_credit = StringUtil.GetNewToken();
                                //更新数据库。
                                MySqlUtil.Execute(userSql.GetUpdatecommandMobile());
                                return new ResponceModel(200, "自动登录成功。", userSql.ToUserResultMobile());
                            }
                            else
                            {
                                userSql.pc_credit = StringUtil.GetNewToken();
                                //更新数据库。
                                MySqlUtil.Execute(userSql.GetUpdateCommandPc());
                                return new ResponceModel(200, "自动登录成功。", userSql.ToUserResultPc());
                            }
                        }
                        else
                        {
                            return new ResponceModel(403, "自动登录已失效，请重新绑定账号");
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ResponceModel.GetInstanceError(ex);
                }
            }
        }

        /// <summary>
        /// 本地检查用户凭证是否生效。
        /// </summary>
        /// <param name="credit"></param>
        /// <returns></returns>
        public static bool CheckUser(string credit, out string username)
        {
            if (credit == null)
            {
                username = null;
                return false;
            }
            else
            {
                DataTable data = null;
                int flag = -1;
                if (MySqlUtil.TryQuery(UserSql.GetQuerycommandMobileCredit(credit), out var dataTable1))
                {
                    data = dataTable1;
                    flag = 0;
                }
                if (MySqlUtil.TryQuery(UserSql.GetQuerycommandPcCredit(credit), out var dataTable2))
                {
                    data = dataTable2;
                    flag = 1;
                }
                if (flag != -1)
                {
                    //验证账户。
                    UserSql userSql = UserSql.FromDataRow(data.Rows[0]);

                    username = userSql.username;
                    return true;
                }
                else
                {
                    username = null;
                    return false;
                }
            }
        }
    }
}