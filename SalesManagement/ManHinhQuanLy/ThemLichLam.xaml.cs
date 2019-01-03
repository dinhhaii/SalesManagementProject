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
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for ThemLichLam.xaml
    /// </summary>
    public partial class ThemLichLam : Window
    {
        public string addMaNV { get; set; }
        SqlConnection sqlConnection = null;
        public ThemLichLam(string value)
        {
            addMaNV = value;
            InitializeComponent();
        }
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            int k = int.Parse(txtCa.Text.Trim());
            
            
            
            if (k == 1 || k == 2)
            {
               
                if (datePicker.SelectedDate >= DateTime.Today)
                {
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);

                        sqlCommand.CommandType = CommandType.Text;
                        //tIỀN HÀNH THÊM DỮ LIỆU VÀO SQL
                        string sql = "insert into LichLam(MaNV,NgayLam,Ca,CoMat,LyDo) values(@MaNV,@NgayLam,@Ca,NULL,NULL)";
                        sqlCommand.CommandText = sql;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Parameters.Add("@MaNV", SqlDbType.NChar).Value = addMaNV;
                        sqlCommand.Parameters.Add("@NgayLam", SqlDbType.Date).Value = datePicker.SelectedDate;
                        sqlCommand.Parameters.Add("@Ca", SqlDbType.NChar).Value = "" + txtCa.Text;
                        int ret = sqlCommand.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            MessageBox.Show("Đăng ký lịch làm thành công");
                            txtCa.Text = "";
                            datePicker.Text = "";
                            if (sqlConnection.State == ConnectionState.Open)
                                sqlConnection.Close();
                            sqlCommand.Cancel();
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Đã đăng ký ngày làm và ca làm này!!Vui lòng đăng ký ca khác");
                    }
                }
                else
                {
                    MessageBox.Show("Chọn ngày sai, xin vui lòng chọn lại");
                }
            }
            else
            {
                    MessageBox.Show("Nhập sai ca, xin vui lòng nhập lại");
            }

            
        }

        private void btnThoat_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
