using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exam.Models;

namespace Exam.Services
{
    interface IGetAllQuestions
    {
        List<Question> GetQuestions(object Source);
    }
}
