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
    /// Interaction logic for ChinhSuaSanPham.xaml
    /// </summary>
    public partial class ChinhSuaChi : Window
    {
        public string editMaNV { get; set; }
        public string temp { get; set; }
        ObservableCollection<Chi> listChi = new ObservableCollection<Chi>();
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        SqlConnection sqlConnection = null;

        public ChinhSuaChi(string value)
        {
            InitializeComponent();
            editMaNV = value;
            getDataChi();
            updateData();
        }

        public void updateData()
        {
            for (int i = 0; i < listChi.Count; i++)
            {
                if (listChi[i].MaNV == editMaNV)
                {
                    txtMaNV.Text = listChi[i].MaNV;
                    txtGia.Text = listChi[i].TongTien.ToString();
                    datePicker.Text = listChi[i].ThoiGian.ToString();
                    txtLyDo.Text = listChi[i].LyDo;
                    break;
                }
            }
        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getDataChi()
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
                Chi chi = new Chi();
                chi.MaNV = sqlReader.GetString(0).Trim();
                chi.TongTien = sqlReader.GetFloat(1);
                chi.LyDo = sqlReader.GetString(2).Trim();
                chi.ThoiGian = sqlReader.GetDateTime(3);
                listChi.Add(chi);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            Chi sp = new Chi();
            SqlCommand sqlCmd = new SqlCommand();


            try
            {
                //Kết nối đến CSDL
                connectSQL(App.sqlString, out sqlConnection);
                sqlCmd.CommandType = CommandType.Text;
                bool input = true;
                if (!IsNumber(txtGia.Text))
                {
                    MessageBox.Show("Thuộc tính Giá nhập chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                    input = false;
                }

                if (input)
                {
                    //Xóa dữ liệu
                    StringBuilder cmdtext = new StringBuilder();
                    cmdtext.Append("DELETE FROM Chi WHERE Chi.MaNV = '");
                    cmdtext.Append(editMaNV);
                    //cmdtext.Append("'\nDELETE FROM SP_KH WHERE SP_KH.MaSP = '");
                    //cmdtext.Append(editMaSP);
                    cmdtext.Append("'");

                    sqlCmd.CommandText = cmdtext.ToString();
                    sqlCmd.Connection = sqlConnection;
                    //Tiến hành xóa dữ liệu
                    int retdelete = sqlCmd.ExecuteNonQuery();
                    if (retdelete > 0)
                    {
                        //Truy vấn cập nhật dữ liệu
                        string sqlquery = "insert into Chi(MaNV,TongTien,LyDo,ThoiGian) values(@MaNV,@TongTien,@LyDo,@ThoiGian)";
                        sqlCmd.CommandText = sqlquery;
                        sqlCmd.Connection = sqlConnection;
                        sqlCmd.Parameters.Add("@MaNV", SqlDbType.NChar).Value = txtMaNV.Text;
                        sqlCmd.Parameters.Add("@TongTien", SqlDbType.Real).Value = float.Parse(txtGia.Text);
                        sqlCmd.Parameters.Add("@ThoiGian", SqlDbType.DateTime).Value = datePicker.DisplayDate;
                        sqlCmd.Parameters.Add("@LyDo", SqlDbType.NVarChar).Value = txtLyDo.Text;
                        //Thực thi cập nhật sản phẩm vào cơ sở dữ liệu
                        int ret = sqlCmd.ExecuteNonQuery();
                        
                        if (ret > 0)
                        {
                            MessageBox.Show("Cập nhật thành công!");
                        }
                        else
                        {
                            MessageBox.Show("Cập nhật dữ liệu không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật dữ liệu không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Cập nhật dữ liệu không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                sqlCmd.Cancel();
                if (sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }

        }

        //Thoát khỏi cửa sổ Cập nhật sản phẩm
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            chinhSuaChi.Close();
        }

        //Kiếm tra chuỗi nhập có phải là số
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
    }
}