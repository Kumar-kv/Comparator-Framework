using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComparatorFramework.Utils;
using log4net;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private static readonly ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [TestMethod, Description("Verify CSV with 50000 records - 6MB")]
        public void VerifyCSV_6MB_50000Records()
        {
            utils.CompareCSV("50000SalesRecords_Source1.csv", "50000SalesRecords_Source2.csv");
        }

        [TestMethod, Description("Verify CSV with 100000 records - 12MB")]
        public void VerifyCSV_12MB_100000Records()
        {
            utils.CompareCSV("100000SalesRecords_Source1.csv", "100000SalesRecords_Source2.csv");
        }

        [TestMethod, Description("Verify CSV with 500000 records - 60MB")]
        public void VerifyCSV_60MB_500000Records()
        {
            utils.CompareCSV("500000SalesRecords_Source1.csv", "500000SalesRecords_Source2.csv");
        }

        [TestMethod, Description("Compare CSV Row and Column Count")]
        public void VerifyRowColCount()
        {
            bool compareresult = utils.CompareCSVRowColCount("100000SalesRecords_Source1.csv", "100000SalesRecords_Source2.csv");
            if (compareresult)
                Console.WriteLine(" Match");
            else
                Console.WriteLine("Not Match");
        }
    }
}
