using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.Models;

namespace Exam.Services
{
    class GetQuestionsFromExcel : IGetAllQuestions
    {
        private DataTable myData = new DataTable();

        public List<Question> GetQuestions(object Sourece)
        {
            List<Question> result = new List<Question>();
            string Path = (string)Sourece;
            ReadExcel(Path);
            foreach(DataRow DR in myData.Rows)
            {
                Question Q = new Question();
                Q.Class   = DR[0].ToString().Trim();
                Q.Content = DR[1].ToString().Trim();
                Q.Options = DR[2].ToString().Trim();
                Q.Answer  = DR[3].ToString().Trim();

                Console.WriteLine("Class:{0}Content:{1}Options:{2}Answer:{3}", Q.Class, Q.Content, Q.Options, Q.Answer);

                result.Add(Q);
            }

            return result;
        }

        private void ReadExcel(string Path)
        {
            string strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strCon);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [Sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strCon);
            ds = new DataSet();
            myCommand.Fill(myData);
        }

    }
}
