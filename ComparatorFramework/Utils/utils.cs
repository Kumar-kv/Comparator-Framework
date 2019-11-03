using ComparatorFramework.Model;
using log4net;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace ComparatorFramework.Utils
{
    public class utils
    {
        private static readonly ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void CompareCSV(string csvfilename1, string csvfilename2)
        {
            try
            {
                log.Info("Starting the process to compare CSV");

                //Get path of csv files
                string csvfilepath1 = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, @"TestData\", csvfilename1);
                string csvfilepath2 = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, @"TestData\", csvfilename2);
                string resultfile = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, @"Result\result.csv");

                //Read first csv and assign it a Data Table
                DataTable file1 = utils.GetDataTableFromCsv(csvfilepath1);

                //Read second csv and assign it a Data Table
                DataTable file2 = utils.GetDataTableFromCsv(csvfilepath2);

                //Find the difference and store the differences it a Data Table
                DataTable filediff = utils.Difference(file1, file2);

                //Object cellValue1 = filediff.Rows[0][0];

                //Write the differences to a file
                utils.WriteCsv(filediff, resultfile);
                log.Info("Succesffully compared csv files and the difference, if any is in result.csv");
            }
            catch (Exception)
            {
                log.Info("Exception while Comparing CSV");
                throw;
            }

        }

        public static DataTable GetDataTableFromCsv(string path)
        {
            try
            {
                log.Info("Initiating method to create data table from csv: "+ path);
                //Create Empty Data Table
                DataTable dataTable = new DataTable();

                //Read all lines of csv and store then in sting array
                String[] csv = File.ReadAllLines(path);

                //Add Table Column Schema
                SalesModel.SalesCSVModel(dataTable);

                //Read each row from string array and add a row in Data Table
                foreach (string csvrow in csv)
                {
                    var fields = csvrow.Split(','); // csv delimiter
                    var row = dataTable.NewRow();

                    row.ItemArray = fields;
                    dataTable.Rows.Add(row);
                }

                return dataTable;
            }
            catch (Exception)
            {
                log.Info("Exception while creating datatable from csv");
                throw;
            }
        }
                
        public static DataTable Difference(DataTable dataTable1, DataTable dataTable2)
        {

            try
            {
                log.Info("Initiating method to get difference between csv files");
                //Create Empty Data Table
                DataTable table = new DataTable();


                //Use Dataset for creating DataRelation
                using (DataSet ds = new DataSet())
                {
                    //Add tables
                    ds.Tables.AddRange(new DataTable[] { dataTable1.Copy(), dataTable2.Copy() });


                    //Get Columns for DataRelation
                    DataColumn[] firstcolumns = new DataColumn[ds.Tables[0].Columns.Count];


                    for (int i = 0; i < firstcolumns.Length; i++)
                    {
                        firstcolumns[i] = ds.Tables[0].Columns[i];
                    }


                    DataColumn[] secondcolumns = new DataColumn[ds.Tables[1].Columns.Count];
                    for (int i = 0; i < secondcolumns.Length; i++)
                    {
                        secondcolumns[i] = ds.Tables[1].Columns[i];
                    }


                    //Create DataRelation
                    DataRelation r1 = new DataRelation(string.Empty, firstcolumns, secondcolumns, false);
                    ds.Relations.Add(r1);


                    DataRelation r2 = new DataRelation(string.Empty, secondcolumns, firstcolumns, false);
                    ds.Relations.Add(r2);


                    //Create columns for return table
                    table = dataTable1.Clone();


                    //If First Row not in Second, Add to return table.
                    table.BeginLoadData();


                    foreach (DataRow parentrow in ds.Tables[0].Rows)
                    {
                        DataRow[] childrows = parentrow.GetChildRows(r1);
                        if (childrows == null || childrows.Length == 0)
                            table.LoadDataRow(parentrow.ItemArray, true);
                    }


                    foreach (DataRow parentrow in ds.Tables[1].Rows)
                    {
                        DataRow[] childrows = parentrow.GetChildRows(r2);
                        if (childrows == null || childrows.Length == 0)
                            table.LoadDataRow(parentrow.ItemArray, true);
                    }


                    table.EndLoadData();
                }


                return table;
            }
            catch (Exception)
            {
                log.Info("Exception while finding difference between csv files");
                throw;
            }


        }

        public static void WriteCsv(DataTable dt, string path)
        {
            try
            {
                log.Info("Initiating method to write diff to file");

                using (var writer = new StreamWriter(path))
                {
                    writer.WriteLine(string.Join(",", dt.Columns.Cast<DataColumn>().Select(dc => dc.ColumnName)));
                    foreach (DataRow row in dt.Rows)
                    {
                        writer.WriteLine(string.Join(",", row.ItemArray));
                    }
                }
            }
            catch (Exception)
            {
                log.Info("Exception while writing diff to file");
                throw;
            }
        }

        public static bool CompareCSVRowColCount (string csvfilename1, string csvfilename2)
        {
            try
            {
                log.Info("Initiating method to compare row and col count");

                //Get path of csv files
                string csvfile1 = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, @"TestData\", csvfilename1);
                string csvfile2 = Path.Combine(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, @"TestData\", csvfilename2);

                //Read first csv and assign it a Data Table
                DataTable file1dt = utils.GetDataTableFromCsv(csvfile1);

                //Read second csv and assign it a Data Table
                DataTable file2dt = utils.GetDataTableFromCsv(csvfile2);

                //Row Count
                int file1RowCount = file1dt.Rows.Count;
                int file2RowCount = file2dt.Rows.Count;

                //Col Count
                int file1ColCount = file1dt.Columns.Count;
                int file2ColCount = file2dt.Columns.Count;

                log.Info("For file: " + csvfilename1 + " - " + "Row count: " + file1RowCount + " and column count: " + file1ColCount);
                log.Info("For file: " + csvfilename2 + " - " + "Row count: " + file2RowCount + " and column count: " + file2ColCount);
                //Compare row and col count
                if (file1RowCount != file2RowCount || file1ColCount != file2ColCount)
                {
                    log.Info("Succesfully compared row and col count and Row/Count between files do not match");
                    return false;
                }
                else
                {
                    log.Info("Succesfully compared row and col count and Row/Count between files match");
                    return true;
                }
            }
            catch (Exception)
            {
                log.Info("Exception while comparing row and col count");
                throw;
            }
            
        }
                
    }
}
