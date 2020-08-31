using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1.ClassMysql
{
    class MySql_importfg_warehouse
    {
        MySqlConnection mysql = ClassMySqlConnect.StringConnection();

        public bool Open()
        {
            try
            {
                mysql.Open();
                if (mysql.State == System.Data.ConnectionState.Open)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
