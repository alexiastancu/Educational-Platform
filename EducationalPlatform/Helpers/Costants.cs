using EducationalPlatform.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationalPlatform.Helpers
{
    public class Costants
    {
        static SchoolEntities dbContext = new SchoolEntities();
        //public static string connectionString = @"Data Source=LAPTOP-P7L9D7DP\SQLEXPRESS0;initial catalog=School;Integrated Security=True;MultipleActiveResultSets=True";
        public static string connectionString = @"data source=ALEXIA\SQLEXPRESS;initial catalog=School;integrated security=True;multipleactiveresultsets=True";
        

    }
}
