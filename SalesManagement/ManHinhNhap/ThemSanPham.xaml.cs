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


namespace SalesManagement.ManHinhNhap
{
    /// <summary>
    /// Interaction logic for ThemSanPham.xaml
    /// </summary>
    public partial class ThemSanPham : Window
    {
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        SqlConnection sqlConnection = null;
        private string strfileName;

        public ThemSanPham()
        {
            InitializeComponent();
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
            sqlCom.CommandText = "select * from SanPham";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                SanPham sp = new SanPham();
                sp.MaSP = sqlReader.GetString(0).Trim();
                sp.TenSP = sqlReader.GetString(1).Trim();
                if (!sqlReader.IsDBNull(2))
                {
                    sp.HinhAnhSP = sqlReader.GetString(2).Trim();
                }
                else
                    sp.HinhAnhSP = "";
                sp.Size = sqlReader.GetString(3).Trim();
                sp.SoLuong = sqlReader.GetInt32(4);
                sp.Gia = sqlReader.GetFloat(5);
                sp.NgayNhap = sqlReader.GetDateTime(6);
                if (!sqlReader.IsDBNull(7))
                {
                    sp.DoiTra = sqlReader.GetString(7).Trim();
                }
                else
                    sp.DoiTra = "";

                listSP.Add(sp);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            getData();
            bool duplicate = false;
            //Kiểm tra xem có trùng mã sản phẩm không
            for (int i = 0; i < listSP.Count; i++)
            {
                if (listSP[i].MaSP.Trim().ToLower() == txtMaSP.Text.ToLower())
                {
                    duplicate = true;
                    break;
                }

            }
            SanPham sp = new SanPham();
            int k = 0;//Kiểm tra nhập đúng hay chưa
            SqlCommand sqlCommand = new SqlCommand();
            if (duplicate == false)
            {
                try
                {
                    //Kết nối tới CSDL
                    connectSQL(App.sqlString, out sqlConnection);

                    sqlCommand.CommandType = CommandType.Text;
                    //tIỀN HÀNH THÊM DỮ LIỆU VÀO SQL
                    string sql = "insert into SanPham(MaSP,TenSP,HinhAnhSP,Size,SoLuong,Gia,NgayNhap,DoiTra) values(@MaSP,@tenSP,@HinhAnhSP,@Size,@SoLuong,@Gia,@NgayNhap,@DoiTra)";
                    sqlCommand.CommandText = sql;
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.Add("@MaSP", SqlDbType.NChar).Value = txtMaSP.Text;
                    sqlCommand.Parameters.Add("@tenSP", SqlDbType.NVarChar).Value = txtTenSP.Text;

                    sqlCommand.Parameters.Add("@Size", SqlDbType.NChar).Value = txtSize.Text;
                    sqlCommand.Parameters.Add("@SoLuong", SqlDbType.Int).Value = int.Parse(txtSoLuong.Text);
                    sqlCommand.Parameters.Add("@Gia", SqlDbType.Real).Value = float.Parse(txtGia.Text);
                    sqlCommand.Parameters.Add("@NgayNhap", SqlDbType.Date).Value = datePicker.DisplayDate;
                    //Nếu đổi trả
                    if (checkBox1.IsChecked == true)
                    {
                        sqlCommand.Parameters.Add("@DoiTra", SqlDbType.NVarChar).Value = txtBoxLyDo.Text;
                    }
                    else
                    {
                        sqlCommand.Parameters.Add("@DoiTra", SqlDbType.NVarChar).Value = "";
                    }
                    if (strfileName != null)
                    {
                        sqlCommand.Parameters.Add("@HinhAnhSP", SqlDbType.NChar).Value = strfileName;
                    }
                    else
                    {
                        sqlCommand.Parameters.Add("@HinhAnhSP", SqlDbType.NChar).Value = "";
                    }
                    k = 1;//Nếu nhập đúng
                }
                catch (Exception)
                {
                    //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                    MessageBox.Show("Thông tin đăng nhập chưa đúng hoặc còn thiếu!!");
                }
                //Nếu nhập đúng
                if (k == 1)
                {
                    int ret = sqlCommand.ExecuteNonQuery();
                    if (ret > 0)
                    {
                        MessageBox.Show("Thêm thành công");
                        txtGia.Text = "";
                        txtMaSP.Text = "";
                        txtSoLuong.Text = "";
                        txtTenSP.Text = "";
                        txtSize.Text = "";
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
            else
            {
                MessageBox.Show("Nhập trùng mã sản phẩm, xin vui lòng nhập lại!!");
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            themSP.Close();
        }

        private void btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";//Chọn những file có định dạng png,jpg ban đầu

            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                strfileName = openFileDialog.FileName;//Lấy đường dẫn
                BitmapImage bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(strfileName, UriKind.RelativeOrAbsolute);
                bm.EndInit();
                HinhAnhSP.Source = bm;
            }
        }
    }
}
