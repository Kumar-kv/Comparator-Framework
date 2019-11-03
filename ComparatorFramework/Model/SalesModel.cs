using System.Data;

namespace ComparatorFramework.Model
{
    public class SalesModel
    {

        public static DataTable SalesCSVModel(DataTable dt)
        {
            DataTable dataTable = dt;

            dataTable.Columns.Add("Region", typeof(string));
            dataTable.Columns.Add("Country", typeof(string));
            dataTable.Columns.Add("ItemType", typeof(string));
            dataTable.Columns.Add("SalesChannel", typeof(string));
            dataTable.Columns.Add("OrderPriority", typeof(string));
            dataTable.Columns.Add("OrderDate", typeof(string));
            dataTable.Columns.Add("OrderID", typeof(string));
            dataTable.Columns.Add("ShipDate", typeof(string));
            dataTable.Columns.Add("UnitsSold", typeof(string));
            dataTable.Columns.Add("UnitPrice", typeof(string));
            dataTable.Columns.Add("UnitCost", typeof(string));
            dataTable.Columns.Add("TotalRevenue", typeof(string));
            dataTable.Columns.Add("TotalCost", typeof(string));
            dataTable.Columns.Add("TotalProfit", typeof(string));

            return dataTable;
        }
    }
}
