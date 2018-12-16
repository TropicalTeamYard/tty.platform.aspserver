﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using wejh.Configs;

namespace wejh.Util
{
    public static class SqlUtil
    {
        private static SqlConnection conn = null;
        public static void Open()
        {
            conn = new SqlConnection
            {
                ConnectionString = Config.Conn
            };
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public static DataSet Query(string command)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                return dataSet;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public static bool TryQuery(string command,out DataSet set)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                set = dataSet;
                return dataSet.Tables[0].Rows.Count !=0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public static bool Exists(string command)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                return dataSet.Tables[0].Rows.Count != 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public static void Execute(string command)
        {
            try
            {
                Open();
                var cmd = new SqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public static void Close()
        {
            conn.Close();
        }
    }
}
