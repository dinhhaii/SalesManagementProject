using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using SalesManagement;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace SalesManagement.ManHinhChi
{
    /// <summary>
    /// Interaction logic for ThemChi.xaml
    /// </summary>
    public partial class ThemChi : Window
    {
        ObservableCollection<Chi> listChi = new ObservableCollection<Chi>();
        SqlConnection sqlConnection = null;

        public ThemChi()
        {
            InitializeComponent();
            datePicker.Text = DateTime.Now.ToString();
            datePicker.DisplayDate = DateTime.Now;
            datePicker.IsEnabled = false;
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getData()
        {
            //Lấy danh sách sản phẩm từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from Chi";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                Chi sp = new Chi();
                sp.MaNV = sqlReader.GetString(0).Trim();
                sp.TongTien = sqlReader.GetFloat(1);
                sp.LyDo = sqlReader.GetString(2).Trim();
                sp.ThoiGian = sqlReader.GetDateTime(3);
                listChi.Add(sp);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            getData();
            bool duplicate = false;
           
            bool input = true;
            SqlCommand sqlCommand = new SqlCommand();

            if (!IsNumber(txtGia.Text))
            {
                MessageBox.Show("Thông tin nhập thiếu hoặc chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                input = false;
            }

            if (input == true)
            {
                if (duplicate == false)
                {
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);

                        sqlCommand.CommandType = CommandType.Text;
                        //TIẾN HÀNH THÊM DỮ LIỆU VÀO SQL
                        string sql = "insert into Chi(MaNV,TongTien,LyDo,ThoiGian) values(@MaNV,@TongTien,@LyDo,@ThoiGian)";
                        sqlCommand.CommandText = sql;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Parameters.Add("@MaNV", SqlDbType.NChar).Value = txtMaNV.Text;
                        sqlCommand.Parameters.Add("@TongTien", SqlDbType.Real).Value = float.Parse(txtGia.Text);
                        sqlCommand.Parameters.Add("@LyDo", SqlDbType.NVarChar).Value = txtLyDo.Text;
                        sqlCommand.Parameters.Add("@ThoiGian", SqlDbType.DateTime).Value = DateTime.Now;

                    }
                    catch (Exception)
                    {
                        //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                        MessageBox.Show("Thông tin nhập chưa đúng hoặc còn thiếu!!");
                    }
                    //Nếu nhập đúng

                    int ret = sqlCommand.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công");
                        txtMaNV.Text = "";
                        txtGia.Text = "";
                        txtLyDo.Text = "";
                        datePicker.Text = "";
                        if (sqlConnection.State == ConnectionState.Open)
                            sqlConnection.Close();
                        sqlCommand.Cancel();
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công!!");
                    }

                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            themChi.Close();

        }

        //Kiếm tra chuỗi nhập có phải là số
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
    }
}
