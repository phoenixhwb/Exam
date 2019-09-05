using Exam.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam.Services
{
    static class GetQuestions
    {
        public static List<Question> Result;
        public static void Get(IGetAllQuestions I)
        {
            Result = I.GetQuestions(@".\\database\\Lib.xls");
        }
    }
}
