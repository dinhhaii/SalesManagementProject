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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
namespace SalesManagement.ManHinhQuanLy
{
    /// <summary>
    /// Interaction logic for QuanLy.xaml
    /// </summary>
    public partial class QuanLy : UserControl
    {

     
        SqlConnection sqlConnection = null;
        ObservableCollection<NhanVien> listSearchKH = new ObservableCollection<NhanVien>();
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();
        public bool isDelete { get; set; }
        public bool isEditing { get; set; }
        public QuanLy()
        {
            
            InitializeComponent();
            isEditing = false;
            isDelete = false;
            getData();
            DataGridListEmployee.ItemsSource = listNV;
            
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
        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            if (isDelete)
            {
                //try
                //{
                NhanVien temp = row.Item as NhanVien;

                for (int i = 0; i < listNV.Count; i++)
                {
                    if (temp == listNV[i])
                    {
                        //MessageBox.Show(listSP[i].isChecked.ToString());
                        if (listNV[i].isChecked == false)
                        {
                            listNV[i].isChecked = true;
                        }
                        else
                        {
                            listNV[i].isChecked = false;
                        }
                        DataGridListEmployee.ItemsSource = null;
                        DataGridListEmployee.ItemsSource = listNV;
                        break;
                    }
                }
                //}
                //catch (Exception) {
                //    MessageBox.Show("Error");
                //}
            }
            else
            {
                NhanVien temp = row.DataContext as NhanVien;
                ChinhSuaNV suaNV = new ChinhSuaNV(temp.MaNV);
                suaNV.ShowDialog();
                refreshData();
            }
        }

      

        private void isCheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            if (checkBox.IsChecked == true)
            {
                for (int i = 0; i < listNV.Count; i++)
                {
                    listNV[i].isChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < listNV.Count; i++)
                {
                    listNV[i].isChecked = false;
                }
            }
            DataGridListEmployee.ItemsSource = null;
            DataGridListEmployee.ItemsSource = listNV;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (checkBox.IsChecked == true)
            {
                dataGridRow.Background = Brushes.IndianRed;
                listNV[index].isChecked = true;
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (checkBox.IsChecked == false)
            {
                Brush temp = new BrushConverter().ConvertFrom("#1FFFFFFF") as Brush;
                dataGridRow.Background = temp;
                listNV[index].isChecked = false;
            }
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            ThemNhanVien them = new ThemNhanVien();
            them.ShowDialog();         
            refreshData();
          
        }

        private void btnDiemDanh_Click(object sender, RoutedEventArgs e)
        {
            DiemDanh diemDanh = new DiemDanh();
            diemDanh.ShowDialog();
            refreshData();

        }

     
      

        private void btnDeleteEmployee_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                DataGridListEmployee.Columns[0].Visibility = Visibility.Visible;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                isDelete = true;
                btnDeleteEmployee.Background = Brushes.Red;
                btnAddEmployee.Visibility = Visibility.Collapsed;
                DataGridListEmployee.CanUserSortColumns = false;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                DataGridListEmployee.Columns[0].Visibility = Visibility.Collapsed;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
                isDelete = false;
                btnDeleteEmployee.Background = new BrushConverter().ConvertFrom("#FF2962FF") as Brush;
                btnAddEmployee.Visibility = Visibility.Visible;
                DataGridListEmployee.CanUserSortColumns = true;
                btnDelete.Visibility = Visibility.Collapsed;
                for (int i = 0; i < listNV.Count; i++)
                {
                    listNV[i].isChecked = false;
                }
                DataGridListEmployee.ItemsSource = null;
                DataGridListEmployee.ItemsSource = listNV;
            }
        }
        public void refreshData()
        {
            listNV.Clear();
            getData();
            DataGridListEmployee.ItemsSource = null;
            DataGridListEmployee.ItemsSource = listNV;
            DataGridListEmployee.Items.Refresh();
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < listNV.Count; i++)
            {
                if (listNV[i].isChecked == true)
                {
                    count++;
                }
            }

            //Duyệt danh sách listSP và xóa dữ liệu từ Database
            if (count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa " + count.ToString() + " nhân viên chứ?", "Sales Management", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < listNV.Count; i++)
                    {
                        if (listNV[i].isChecked == true)
                        {
                         

                            SqlCommand sqlCmd = new SqlCommand();
                          
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                sqlCmd.CommandText = "DELETE FROM NhanVien WHERE NhanVien.MaNV = '" + listNV[i].MaNV + "'";
                               
                                sqlCmd.Connection = sqlConnection;
                                int ret = sqlCmd.ExecuteNonQuery();
                                if (ret > 0)
                                {
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Xóa không thành công!!");
                                }

                            }
                            catch (Exception)
                            {
                                MessageBox.Show(" Xóa không thành công");
                            }
                            finally
                            {
                                if (sqlConnection.State == ConnectionState.Open)
                                    sqlConnection.Close();
                                sqlCmd.Cancel();
                            }
                        }
                    }
                }
            }

            //Reload dữ liệu
            refreshData();
        }

        private void btnDKLich_Click(object sender, RoutedEventArgs e)
        {


            DangKyLichLam dangKy = new DangKyLichLam();
            dangKy.ShowDialog();
         
            refreshData();
        }

        private void btnQuanLyLuong_Click(object sender, RoutedEventArgs e)
        {
            QuanLyLuong quanLyLuong = new QuanLyLuong();
            quanLyLuong.ShowDialog();
            refreshData();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            resetTxtSearch.Visibility = Visibility.Visible;
            if (listSearchKH.Count > 0)
            {
                //Nếu tìm kiếm sản phẩm khác thì reset lại list sản phẩm tìm kiếm
                listSearchKH.Clear();
            }
            if (txtSearch.Text.Trim() == "")
            {
                resetTxtSearch.Visibility = Visibility.Collapsed;
                DataGridListEmployee.ItemsSource = null;
                DataGridListEmployee.ItemsSource = listNV;

            }
            else
            {
                for (int i = 0; i < listNV.Count; i++)
                {
                    string str = listNV[i].TenNV.Trim().ToLower().Replace(" ", "");
                    //Tách chuỗi nhập vào thành các từ
                    string expression = txtSearch.Text.ToLower();
                    string[] token = expression.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    int index = 0;
                    //Duyệt qua mảng các từ trong chuỗi nhập
                    for (; index < token.Length; index++)
                    {
                        //Xóa khoảng trắng
                        token[index] = token[index].Trim();
                        //Kiểm tra từ đó xuất hiện trong tên sản phẩm không
                        int pos = listNV[i].TenNV.ToLower().IndexOf(token[index]);
                        //Nếu không xuất hiện thì
                        if (pos < 0)
                        {
                            break;
                        }
                    }
                    if (index == token.Length)
                    {
                        listSearchKH.Add(listNV[i]);
                    }
                }

                DataGridListEmployee.ItemsSource = null;
                DataGridListEmployee.ItemsSource = listSearchKH;
            }

        }

        private void resetTxtSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
        }
    }

}
