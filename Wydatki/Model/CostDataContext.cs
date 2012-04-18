using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;

namespace Wydatki.Model
{
    public class CostDataContext : DataContext
    {
                // Pass the connection string to the base class.
        public CostDataContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the cost items.
        public Table<CostItem> Items;

        // Specify a table for the categories.
        public Table<CostCategory> Categories;
    }
}
