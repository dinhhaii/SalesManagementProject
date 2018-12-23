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

namespace SalesManagement.ManHinhNhap
{
    /// <summary>
    /// Interaction logic for ChinhSuaSanPham.xaml
    /// </summary>
    public partial class ChinhSuaSanPham : Window
    {
        public string editMaSP { get; set; }
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        SqlConnection sqlConnection = null;
        private string strfileName;

        public ChinhSuaSanPham(string value)
        {
            InitializeComponent();
            editMaSP = value;
            updateData();
        }

        public void updateData()
        {
            getData();
            for (int i = 0; i < listSP.Count; i++)
            {
                if (listSP[i].MaSP == editMaSP)
                {
                    txtMaSP.Text = listSP[i].MaSP;
                    txtTenSP.Text = listSP[i].TenSP;
                    txtSoLuong.Text = listSP[i].SoLuong.ToString();
                    txtSize.Text = listSP[i].Size;
                    txtGia.Text = listSP[i].Gia.ToString();
                    datePicker.Text = listSP[i].NgayNhap.ToString();
                    txtBoxLyDo.Text = listSP[i].DoiTra;
                    strfileName = listSP[i].HinhAnhSP;
                    BitmapImage bm = new BitmapImage();
                    bm.BeginInit();
                    bm.UriSource = new Uri(listSP[i].HinhAnhSP, UriKind.RelativeOrAbsolute);
                    bm.EndInit();
                    HinhAnhSP.Source = bm;
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SanPham sp = new SanPham();
            SqlCommand sqlCmd = new SqlCommand();


            try
            {
                //Kết nối đến CSDL
                connectSQL(App.sqlString, out sqlConnection);
                sqlCmd.CommandType = CommandType.Text;
                bool input = true;

                if (!IsNumber(txtSoLuong.Text))
                {
                    MessageBox.Show("Thuộc tính Số lượng nhập chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                    input = false;
                }
                if (!IsNumber(txtGia.Text))
                {
                    MessageBox.Show("Thuộc tính Giá nhập chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                    input = false;
                }

                if (input)
                {
                    //Xóa dữ liệu
                    StringBuilder cmdtext = new StringBuilder();
                    cmdtext.Append("DELETE FROM SanPham WHERE SanPham.MaSP = '");
                    cmdtext.Append(editMaSP);
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
                        string sqlquery = "insert into SanPham(MaSP,TenSP,HinhAnhSP,Size,SoLuong,Gia,NgayNhap,DoiTra) values(@MaSP,@tenSP,@HinhAnhSP,@Size,@SoLuong,@Gia,@NgayNhap,@DoiTra)";
                        sqlCmd.CommandText = sqlquery;
                        sqlCmd.Connection = sqlConnection;
                        sqlCmd.Parameters.Add("@MaSP", SqlDbType.NChar).Value = txtMaSP.Text;
                        sqlCmd.Parameters.Add("@tenSP", SqlDbType.NVarChar).Value = txtTenSP.Text;

                        sqlCmd.Parameters.Add("@Size", SqlDbType.NChar).Value = txtSize.Text;
                        sqlCmd.Parameters.Add("@SoLuong", SqlDbType.Int).Value = int.Parse(txtSoLuong.Text);
                        sqlCmd.Parameters.Add("@Gia", SqlDbType.Real).Value = float.Parse(txtGia.Text);
                        sqlCmd.Parameters.Add("@NgayNhap", SqlDbType.Date).Value = datePicker.DisplayDate;
                        sqlCmd.Parameters.Add("@DoiTra", SqlDbType.NVarChar).Value = txtBoxLyDo.Text;
                        if (strfileName != null)
                        {
                            sqlCmd.Parameters.Add("@HinhAnhSP", SqlDbType.NChar).Value = strfileName;
                        }
                        else
                        {
                            sqlCmd.Parameters.Add("@HinhAnhSP", SqlDbType.NChar).Value = "";
                        }
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
            chinhSuaSP.Close();

        }

        //Thêm ảnh 
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

        //Kiếm tra chuỗi nhập có phải là số
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
    }
}