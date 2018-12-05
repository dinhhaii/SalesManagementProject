using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SalesManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public string sqlString = @"Data Source=DESKTOP-DINHHAI\SQLEXPRESS;Initial Catalog=SalesManagement;Integrated Security=True";
        static public bool isEmployee { get; set; }
       
    }
}
