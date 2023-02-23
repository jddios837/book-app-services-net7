using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Data.SqlClient;

namespace WpfResponsive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string connectionString = "Data Source=localhost;" +
                                                "Initial Catalog=Northwind;" +
                                                "User Id=sa;" +
                                                "Password=Tetra714217#;" +
                                                "TrustServerCertificate=True;";

        private const string sql =
            "WAITFOR DELAY '00:00:05';" +
            "SELECT EmployeeId, FirstName, LastName FROM Employees";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GetEmployeesSyncButton_OnClick(object sender, RoutedEventArgs e)
        {
            Stopwatch timer = Stopwatch.StartNew();
            using SqlConnection connection = new(connectionString);
            try
            {
                connection.Open();

                SqlCommand command = new(sql, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string employee = string.Format("{0}: {1} {2}",
                        reader.GetInt32(0), reader.GetString(1), reader.GetString(2));

                    EmployeesListBox.Items.Add(employee);
                }
                    
                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            EmployeesListBox.Items.Add($"Sync: {timer.ElapsedMilliseconds:N0}");
        }

        private void GetEmployeesAsyncButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}