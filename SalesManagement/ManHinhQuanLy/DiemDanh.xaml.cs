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
    /// Interaction logic for DiemDanh.xaml
    /// </summary>
    public partial class DiemDanh : Window
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        ObservableCollection<NhanVien> listNVCa1 = new ObservableCollection<NhanVien>();
        ObservableCollection<NhanVien> listNVCa2 = new ObservableCollection<NhanVien>();
        ObservableCollection<LichLam> listLichLam = new ObservableCollection<LichLam>();
        public DiemDanh()
        {
           
            InitializeComponent();
            getLichLam();
            getData();
            
            //Danh sách nhân viên ca 1 trong ngày
            for (int i=0;i<listLichLam.Count;i++)
            {
                for(int j=0;j<listNV.Count;j++)
                {
                    if (listLichLam[i].MaNV == listNV[j].MaNV && int.Parse(listLichLam[i].Ca.Trim()) == 1 && listLichLam[i].isDiemDanh==0)
                    {
                 
                        listNVCa1.Add(listNV[j]);
                    }
                }
            }
         
            DataGridListEmployeeDD.ItemsSource = listNVCa1;
            //Danh sách nhân viên ca 2 trong ngày
            for (int i = 0; i < listLichLam.Count; i++)
            {
                for (int j = 0; j < listNV.Count; j++)
                {
                    if (listLichLam[i].MaNV == listNV[j].MaNV && int.Parse(listLichLam[i].Ca.Trim()) == 2&&listLichLam[i].isDiemDanh==0)
                    {

                        listNVCa2.Add(listNV[j]);
                    }
                }
            }
       
            DataGridListEmployeeDDCa2.ItemsSource = listNVCa2;
        }
        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        public void getLichLam()
        {
            //Lấy danh sách lịch làm từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from LichLam";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                LichLam ll = new LichLam();
                ll.MaNV = sqlReader.GetString(0).Trim();
                ll.NgayLam = sqlReader.GetDateTime(1);
                ll.Ca = sqlReader.GetString(2).Trim();
                if(!sqlReader.IsDBNull(3))
                {
                    ll.isDiemDanh = 1;
                }
                else
                {
                    ll.isDiemDanh = 0;

                }
                if(!sqlReader.IsDBNull(4))
                {
                    ll.LyDo = sqlReader.GetString(4);
                }
                else
                {
                    ll.LyDo = "";
                }
                

                //Nếu đúng ngày đăng ký làm việc thì thêm vào danh sách
                if (ll.NgayLam==DateTime.Today)
                {
                    listLichLam.Add(ll);
                   

                }
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

                
                //Kiểm tra với danh sách nhân viên làm việc ngày hôm nay
               
                        listNV.Add(nv);
                    
                
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        private void CoMat_Click(object sender, RoutedEventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                //Kết nối tới CSDL
                connectSQL(App.sqlString, out sqlConnection);

                sqlCommand.CommandType = CommandType.Text;
                //tIỀN HÀNH cập nhật DỮ LIỆU VÀO SQL
                string date = "'" + DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day + "'";
                string Ca = "1";
                string sql = "update LichLam set CoMat=@CoMat where LichLam.NgayLam=" + date + "and LichLam.Ca='" + Ca + "' and LichLam.MaNV=" + "'" + listNVCa1[DataGridListEmployeeDD.SelectedIndex].MaNV + "'";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Parameters.Add("@CoMat", SqlDbType.Bit).Value = true;
               
                int ret = sqlCommand.ExecuteNonQuery();

                if (ret > 0)
                {

                    MessageBox.Show("Điểm danh thành công");
                    sqlCommand.Cancel();
                 
                }
                else
                {
                    MessageBox.Show("Điểm danh không thành công");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Điểm danh không thành công");
            }
            refreshData();

        }
        private void CoMatCa2_Click(object sender, RoutedEventArgs e)
        {

            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                //Kết nối tới CSDL
                connectSQL(App.sqlString, out sqlConnection);

                sqlCommand.CommandType = CommandType.Text;
                //tIỀN HÀNH cập nhật DỮ LIỆU VÀO SQL
                string date = "'" + DateTime.Today.Year + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day + "'";
                string Ca = "2";
           
                string sql = "update LichLam set CoMat=@CoMat, LyDo=@LyDo where LichLam.NgayLam=" + date + "and LichLam.Ca='" + Ca + "' and LichLam.MaNV=" + "'" + listNVCa2[DataGridListEmployeeDDCa2.SelectedIndex].MaNV + "'";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Parameters.Add("@CoMat", SqlDbType.Bit).Value = true;
                sqlCommand.Parameters.Add("@LyDo", SqlDbType.NVarChar).Value = "";
                int ret = sqlCommand.ExecuteNonQuery();

                if (ret > 0)
                {

                    MessageBox.Show("Điểm danh thành công");
                    sqlCommand.Cancel();
            
                }
                else
                {
                    MessageBox.Show("Điểm danh không thành công");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Điểm danh không thành công");
            }
            refreshData();

        }
        private void Vang_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                LyDo lyDo = new LyDo(listNVCa1[DataGridListEmployeeDD.SelectedIndex].MaNV, 1);


                lyDo.ShowDialog();

                refreshData();
            }
            catch(Exception)
            {
                MessageBox.Show("Điểm danh không thành công");
            }

        }
        private void VangCa2_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                LyDo lyDo = new LyDo(listNVCa2[DataGridListEmployeeDDCa2.SelectedIndex].MaNV, 2);


                lyDo.ShowDialog();

                refreshData();
            }
            catch(Exception)
            {
                MessageBox.Show("Điểm danh không thành công");
            }

        }
        //reload lại dữ liệu
        public void refreshData()
        {
            listLichLam.Clear();
            getLichLam();
            for(int i=0;i<listNVCa1.Count;i++)
            {
                for(int j=0;j<listLichLam.Count;j++)
                {
                    if(listNVCa1[i].MaNV==listLichLam[j].MaNV&&listLichLam[j].isDiemDanh==1)
                    {
                        listNVCa1.RemoveAt(i);
                        if (listNVCa1.Count == 0)
                            break;
                    }
                }
            }
          
            for (int i = 0; i < listNVCa2.Count; i++)
            {
                for (int j = 0; j < listLichLam.Count; j++)
                {
                    if (listNVCa2[i].MaNV == listLichLam[j].MaNV&& listLichLam[j].isDiemDanh == 1)
                    {
                        listNVCa2.RemoveAt(i);
                        if (listNVCa2.Count == 0)
                            break;
                    }
                }
            }
            DataGridListEmployeeDD.ItemsSource = null;
            DataGridListEmployeeDD.ItemsSource = listNVCa1;
            DataGridListEmployeeDDCa2.ItemsSource = null;
            DataGridListEmployeeDDCa2.ItemsSource = listNVCa2;
            DataGridListEmployeeDDCa2.Items.Refresh();
            DataGridListEmployeeDD.Items.Refresh();
        }
    }
}
