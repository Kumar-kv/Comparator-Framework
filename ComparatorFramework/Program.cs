using ComparatorFramework.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComparatorFramework
{
    class Program
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            log.Info("Start of Main Program :-\n----------------------------\n\n");
            utils.CompareCSV("50000SalesRecords_Source1.csv", "50000SalesRecords_Source2.csv");
            log.Info("End of Main Program :-\n----------------------------\n\n");
        }
    }
}
