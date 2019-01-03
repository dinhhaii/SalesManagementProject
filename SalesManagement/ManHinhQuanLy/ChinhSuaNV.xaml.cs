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
using SalesManagement;
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.Text.RegularExpressions;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for ChinhSuaNV.xaml
    /// </summary>
    public partial class ChinhSuaNV : Window
    {   public string editMaNV { get; set; }
   
        SqlConnection sqlConnection = null;
        ObservableCollection<TaiKhoan> listTK = new ObservableCollection<TaiKhoan>();
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        public ChinhSuaNV( string value)
        {
            editMaNV = value;
            InitializeComponent();
            updateData();
        }
        //Connect to SQL Server
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
                if (!sqlReader.IsDBNull(4))
                    nv.Email = sqlReader.GetString(4).Trim();
                else
                    nv.Email = "";
                if (!sqlReader.IsDBNull(5))
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
        public void updateData()
        {
            getData();
            for(int i=0;i<listNV.Count;i++)
            {
                if(listNV[i].MaNV==editMaNV)
                {
                    txtMaNV.Text = listNV[i].MaNV;
                    txtTenNV.Text = listNV[i].TenNV;
                    txtGioiTinh.Text = listNV[i].GioiTinh;
                    txtMail.Text = listNV[i].Email;
                    txtDiaChi.Text = listNV[i].DiaChi;
                    txtSDT.Text = listNV[i].SDT;
                    txtLuong.Text = listNV[i].Luong.ToString();
                    txtViTri.Text = listNV[i].ViTri;
                    listNV.RemoveAt(i);//Xóa nhân viên cần cập nhật khỏi danh sách
                }
            }
           
            getTK();   
            for(int i=0;i<listTK.Count;i++)
            {
                if (listTK[i].MaNV == editMaNV)
                {
                    txtTenTaiKhoan.Text = listTK[i].TenTaiKhoan;
                    txtTenTaiKhoan.IsEnabled = false;//K dùng để sửa
                }
                
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //Kiểm tra và xóa nhân viên cần cập nhật khỏi danh sách
            for (int i = 0; i < listNV.Count; i++)
            {
                if (listNV[i].MaNV == editMaNV)
                {
                   
                    listNV.RemoveAt(i);//Xóa nhân viên cần cập nhật khỏi danh sách
                }
            }
            bool duplicate = false;
            //Kiểm tra xem có trùng mã nhân viên không
            for (int i = 0; i < listNV.Count; i++)
            {
                if (listNV[i].MaNV.Trim().ToLower() == txtMaNV.Text.ToLower())
                {
                    duplicate = true;
                    break;
                }
            }
            bool duplicatePass = false;   
            if(passWord.Password==passWord1.Password&&passWord.Password!="")
            {
                duplicatePass = true;
            }
            
            bool input = true;
            if (!IsNumber(txtLuong.Text))
            {
                MessageBox.Show("Thông tin nhập thiếu hoặc chưa đúng. Vui lòng nhập lại!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Error);
                input = false;
            }
            SqlCommand sqlCommand = new SqlCommand();
            SqlCommand command = new SqlCommand();//Dùng để thêm tài khoản
            
            if (input == true)
            {   if (duplicate == false&&duplicatePass==true)
                {
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);

                        sqlCommand.CommandType = CommandType.Text;
                        //tIỀN HÀNH cập nhật DỮ LIỆU VÀO SQL
                        string sql = "update NhanVien set MaNV=@MaNV ,TenNV=@TenNV, GioiTinh=@GioiTinh, SDT=@SDT, Email=@Email, DiaChi=@DiaChi,Luong=@Luong,ViTri=@ViTri where NhanVien.MaNV=" + "'" + editMaNV + "'";
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

                        //Update tài khoản
                        string sqlTK = "update TaiKhoan set MatKhau=@MatKhau where TaiKhoan.MaNV="+ "'" + editMaNV + "'";
                        command.CommandText = sqlTK;
                        command.Connection = sqlConnection;

                     
                        command.Parameters.Add("@MatKhau", SqlDbType.NChar).Value = passWord.Password;

                    }
                    catch(Exception)
                    {
                        //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                        MessageBox.Show("Thông tin nhập chưa đúng hoặc còn thiếu!!");
                    }
                    int ret = sqlCommand.ExecuteNonQuery();
                    int r = command.ExecuteNonQuery();
                    if (ret > 0 && r > 0)
                    {
                        //Cập nhật lại dữ liệu
                        listNV.Clear();
                        getData();
                        listTK.Clear();
                        getTK();
                        MessageBox.Show("Cập nhật thành công");
                        sqlCommand.Cancel();
                    }
                }
                else
                { if (duplicatePass == false)
                    {
                        MessageBox.Show("Nhập lại mật khẩu sai hoặc mật khẩu rỗng, vui lòng kiểm tra lại");
                    }
                if(duplicate==true)
                    {
                        MessageBox.Show("Trùng mã nhân viên, vui lòng kiểm tra lại");
                    }
                }
            }
            else
            {
                MessageBox.Show("Nhập không đúng, vui lòng nhập lại");
            }
            if (sqlConnection.State == ConnectionState.Open)
                sqlConnection.Close();
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
