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
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for ThemNhanVien.xaml
    /// </summary>
    public partial class ThemNhanVien : Window
    {
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        ObservableCollection<TaiKhoan> listTK = new ObservableCollection<TaiKhoan>();
        SqlConnection sqlConnection = null;
        public ThemNhanVien()
        {
            InitializeComponent();
        }
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void getTK()
        {
            //Lấy danh sách nhân viên từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from TaiKhoan";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();

            while (sqlReader.Read())
            {
                TaiKhoan tk = new TaiKhoan();
                tk.MaNV = sqlReader.GetString(0).Trim();
                tk.TenTaiKhoan = sqlReader.GetString(1).Trim();
                tk.MatKhau = sqlReader.GetString(2).Trim();
                
                


                listTK.Add(tk);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        public void getData()
        {
            //Lấy danh sách nhân viên từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from NhanVien";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                NhanVien nv = new NhanVien();
                nv.MaNV = sqlReader.GetString(0).Trim();
                nv.TenNV = sqlReader.GetString(1).Trim();
                nv.GioiTinh = sqlReader.GetString(2).Trim();
                nv.SDT = sqlReader.GetString(3).Trim();
                if (sqlReader.IsDBNull(4))
                    nv.Email = sqlReader.GetString(4).Trim();
                else
                    nv.Email = "";
                if (sqlReader.IsDBNull(5))
                    nv.DiaChi = sqlReader.GetString(5).Trim();
                else
                    nv.DiaChi = "";
                nv.Luong = sqlReader.GetFloat(6);
                nv.ViTri = sqlReader.GetString(7).Trim();


                listNV.Add(nv);
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            getData();
            getTK();
            bool duplicate = false;
            bool duplicateTK = false;
            //Kiểm tra xem có trùng mã nhân viên không
            for (int i = 0; i < listNV.Count; i++)
            {
                if (listNV[i].MaNV.Trim().ToLower() == txtMaNV.Text.ToLower())
                {
                    duplicate = true;
                    break;
                }
            }
            for (int i = 0; i < listTK.Count; i++)
            {
                if (listTK[i].TenTaiKhoan.Trim().ToLower() == txtTenTaiKhoan.Text.ToLower())
                {
                    duplicateTK = true;
                    break;
                }
            }
           
                NhanVien nv = new NhanVien();
            bool input = true;
            SqlCommand sqlCommand = new SqlCommand();
            SqlCommand command = new SqlCommand();//Dùng để thêm tài khoản
            if(!IsNumber(txtLuong.Text))
            {
                MessageBox.Show("Thông tin nhập thiếu hoặc chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                input = false;
            }
            if(input==true)
            {
               
                    if (duplicate==false&&duplicateTK==false)
                    {
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);

                        sqlCommand.CommandType = CommandType.Text;
                        //tIỀN HÀNH THÊM DỮ LIỆU VÀO SQL
                        string sql = "insert into NhanVien(MaNV,TenNV,GioiTinh,SDT,Email,DiaChi,Luong,ViTri) values(@MaNV,@TenNV,@GioiTinh,@SDT,@Email,@DiaChi,@Luong,@ViTri)";
                        sqlCommand.CommandText = sql;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Parameters.Add("@MaNV", SqlDbType.NChar).Value = txtMaNV.Text;
                        sqlCommand.Parameters.Add("@TenNV", SqlDbType.NVarChar).Value = txtTenNV.Text;
                        sqlCommand.Parameters.Add("@GioiTinh", SqlDbType.NVarChar).Value = txtGioiTinh.Text;
                        sqlCommand.Parameters.Add("@SDT", SqlDbType.NChar).Value = txtSDT.Text;
                        if (txtMail.Text != "")
                            sqlCommand.Parameters.Add("@Email", SqlDbType.NChar).Value = txtMail.Text;
                        else
                            sqlCommand.Parameters.Add("@Email", SqlDbType.NChar).Value = "";
                        if (txtDiaChi.Text != "")
                            sqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = txtDiaChi.Text;
                        else
                            sqlCommand.Parameters.Add("@DiaChi", SqlDbType.NVarChar).Value = "";
                        sqlCommand.Parameters.Add("@Luong", SqlDbType.Real).Value = int.Parse(txtLuong.Text);
                        sqlCommand.Parameters.Add("@ViTri", SqlDbType.NVarChar).Value = txtViTri.Text;
                        
                        
                            string sqlTK = "insert into TaiKhoan(MaNV,TenTaiKhoan,MatKhau) values(@MaNV1,@TenTaiKhoan,@MatKhau)";
                            command.CommandText = sqlTK;
                            command.Connection = sqlConnection;
                            command.Parameters.Add("@MaNV1", SqlDbType.NChar).Value = txtMaNV.Text;
                            command.Parameters.Add("@TenTaiKhoan", SqlDbType.NChar).Value = txtTenTaiKhoan.Text;
                            command.Parameters.Add("@MatKhau", SqlDbType.NChar).Value = passWord.Password;
                        
                       
                      


                    }
                    catch (Exception)
                    {
                        //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                        MessageBox.Show("Thông tin nhập chưa đúng hoặc còn thiếu!!");
                    }
                    //Nếu nhập đúng

                    int ret = sqlCommand.ExecuteNonQuery();
                    int r = command.ExecuteNonQuery();
                    if (ret > 0&&r>0)
                    {
                        MessageBox.Show("Thêm thành công");
                        txtTenNV.Text = "";
                        txtMaNV.Text = "";
                        txtGioiTinh.Text = "";
                        txtMail.Text = "";
                        txtSDT.Text = "";
                        txtDiaChi.Text = "";
                        txtViTri.Text = "";
                        txtLuong.Text = "";
                        txtTenTaiKhoan.Text = "";
                        passWord.Password = "";
                        if (sqlConnection.State == ConnectionState.Open)
                            sqlConnection.Close();
                        sqlCommand.Cancel();
                        command.Cancel();
                    }
                    else
                    {
                        MessageBox.Show("Thêm không thành công!!");
                    }
                    }
                else
                {
                    if (duplicate == true)
                        MessageBox.Show("Mã nhân viên đã tồn tại");
                    if (duplicateTK == true)
                        MessageBox.Show("Tên đăng nhập đã tồn tại");
                }
            }

        }
        //Kiếm tra chuỗi nhập có phải là số
        public bool IsNumber(string pText)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(pText);
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
       
            this.Close();
        }
    }
}
