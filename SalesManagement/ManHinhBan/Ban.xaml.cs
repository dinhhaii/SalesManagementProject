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
using System.Data.SqlClient;
using System.Data;
using System.ComponentModel;

namespace SalesManagement.ManHinhBan
{
    /// <summary>
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class Ban : UserControl, INotifyPropertyChanged
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        ObservableCollection<KhachHang> listKH = new ObservableCollection<KhachHang>();
        ObservableCollection<KhachHang> listSearchKH = new ObservableCollection<KhachHang>();
        List<SanPhamMua> listSPMua = new List<SanPhamMua>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool isDelete { get; set; }
        public bool isEditing { get; set; }
        public string ThanhTien = "0";
        public string TT
        {
            get { return ThanhTien; }
            set
            {
                ThanhTien = value;
                OnPropertyChanged("TT");
            }
        }

        public Ban()
        {
            InitializeComponent();
            getKH();
            getSP();
            isEditing = false;
            isDelete = false;
            DataGridKH.ItemsSource = listKH;
            DataGridSP.ItemsSource = listSP;
            ListViewSPMua.ItemsSource = listSPMua;
            this.DataContext = this;
        }

        public class SanPhamMua
        {
            public string TenKH { get; set; }
            public string TenSP { get; set; }
            public string KhuyenMai { get; set; }
            public string SL { get; set; }
            public string Gia { get; set; }
        }

        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
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

        public void getSP()
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
                sp.HinhAnhSP = sqlReader.GetString(2).Trim();
                sp.Size = sqlReader.GetString(3).Trim();
                sp.SoLuong = sqlReader.GetInt32(4);
                sp.Gia = sqlReader.GetFloat(5);
                sp.NgayNhap = sqlReader.GetDateTime(6);

                if (!sqlReader.IsDBNull(7))
                {
                    sp.DoiTra = sqlReader.GetString(7);
                }
                else
                    sp.DoiTra = "";
                if (sp != null)
                {
                    listSP.Add(sp);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getKH()
        {
            //Lấy danh sách khách hàng từ csdl
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from KhachHang";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                KhachHang kh = new KhachHang();
                kh.MaKH = sqlReader.GetString(0);
                kh.TenKH = sqlReader.GetString(1);
                kh.GioiTinh = sqlReader.GetString(2);
                kh.SDT = sqlReader.GetString(3);
                kh.Email = sqlReader.GetString(4);
                kh.DiaChi = sqlReader.GetString(5);

                if (kh != null)
                {
                    listKH.Add(kh);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }


        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            ThemKhachHang window = new ThemKhachHang();
            window.ShowDialog();
            refreshDataKH();
        }

        private void btnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                status.Text = "Chọn các mục cần xóa";
                DataGridKH.Columns[0].Visibility = Visibility.Visible;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                isDelete = true;
                btnDeleteCustomer.Background = Brushes.Red;
                btnAddCustomer.Visibility = Visibility.Collapsed;
                DataGridKH.CanUserSortColumns = false;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                status.Text = "";
                DataGridKH.Columns[0].Visibility = Visibility.Collapsed;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
                isDelete = false;
                btnDeleteCustomer.Background = new BrushConverter().ConvertFrom("#FF2962FF") as Brush;
                btnAddCustomer.Visibility = Visibility.Visible;
                DataGridKH.CanUserSortColumns = true;
                btnDelete.Visibility = Visibility.Collapsed;
                for (int i = 0; i < listKH.Count; i++)
                {
                    listKH[i].isChecked = false;
                }
                DataGridKH.ItemsSource = null;
                DataGridKH.ItemsSource = listKH;
            }

        }

        private void DataGridKHCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            KhachHang temp = row.Item as KhachHang;

            //Nếu ở chế độ xóa
            if (isDelete)
            {
                try
                {
                    for (int i = 0; i < listKH.Count; i++)
                    {
                        if (temp == listKH[i])
                        {
                            //MessageBox.Show(listSP[i].isChecked.ToString());
                            if (listKH[i].isChecked == false)
                            {
                                listKH[i].isChecked = true;
                            }
                            else
                            {
                                listKH[i].isChecked = false;
                            }
                            DataGridKH.ItemsSource = null;
                            DataGridKH.ItemsSource = listKH;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error");
                }
            }
            else if (isEditing)
            {
                if (temp != null)
                {
                    //ChinhSuaKhachHang window = new ChinhSuaKhachHang(temp.MaSP);
                    //(Application.Current.Windows[4] as ChinhSuaKhachHang).editMaSP = temp.MaSP;
                    //window.ShowDialog();
                    refreshDataKH();
                }
            }
            else
            {
                customername.Text = temp.TenKH;
                row.Background = Brushes.DarkSlateGray;
            }

        }
        private void CheckBoxHeader_CheckedUnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            if (checkBox.IsChecked == true)
            {
                for (int i = 0; i < listKH.Count; i++)
                {
                    listKH[i].isChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < listKH.Count; i++)
                {
                    listKH[i].isChecked = false;
                }
            }
            DataGridKH.ItemsSource = null;
            DataGridKH.ItemsSource = listKH;
        }

        private void CheckBoxItem_CheckedUnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();
            if (index < listKH.Count)
            {
                if (checkBox.IsChecked == false)
                {
                    Brush temp = new BrushConverter().ConvertFrom("#1FFFFFFF") as Brush;
                    dataGridRow.Background = temp;
                    listKH[index].isChecked = false;
                }
                if (checkBox.IsChecked == true)
                {
                    dataGridRow.Background = Brushes.IndianRed;
                    listKH[index].isChecked = true;
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < listKH.Count; i++)
            {
                if (listKH[i].isChecked == true)
                {
                    count++;
                }
            }

            //Duyệt danh sách listSP và xóa dữ liệu từ Database
            if (count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa " + count.ToString() + " sản phẩm chứ?", "Sales Management", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < listKH.Count; i++)
                    {
                        if (listKH[i].isChecked == true)
                        {
                            //listSP.RemoveAt(i);

                            SqlCommand sqlCmd = new SqlCommand();
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                sqlCmd.CommandText = "DELETE FROM KhachHang WHERE MaKH = '" + listKH[i].MaKH + "'";
                                sqlCmd.Connection = sqlConnection;
                                int ret = sqlCmd.ExecuteNonQuery();
                                if (ret > 0)
                                {
                                    status.Text = "Xoá thành công";
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
            refreshDataKH();
        }


        public void refreshDataKH()
        {
            listKH.Clear();
            getKH();
            DataGridKH.ItemsSource = null;
            DataGridKH.ItemsSource = listKH;
            DataGridKH.Items.Refresh();
        }

        public void refreshDataSP()
        {
            listSP.Clear();
            getSP();
            //DataGridSP.ItemsSource = null;
            //DataGridSP.ItemsSource = listSP;
            //DataGridSP.Items.Refresh();
        }

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                isEditing = false;
            }
            else
            {
                isEditing = true;
            }
        }

        //        private void Button_Click(object sender, RoutedEventArgs e)
        //        {
        //            refreshData();

        //        }

        //Tìm kiếm
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
                DataGridKH.ItemsSource = null;
                DataGridKH.ItemsSource = listKH;

            }
            else
            {
                for (int i = 0; i < listKH.Count; i++)
                {
                    string str = listKH[i].TenKH.Trim().ToLower().Replace(" ", "");
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
                        int pos = listKH[i].TenKH.ToLower().IndexOf(token[index]);
                        //Nếu không xuất hiện thì
                        if (pos < 0)
                        {
                            break;
                        }
                    }
                    if (index == token.Length)
                    {
                        listSearchKH.Add(listKH[i]);
                    }
                }

                DataGridKH.ItemsSource = null;
                DataGridKH.ItemsSource = listSearchKH;
            }

        }

        private void resetTxtSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
        }

        //Mở cửa sổ nhập Khuyến mãi
        private void ButtonKM_Click(object sender, RoutedEventArgs e)
        {
            KhuyenMai window = new KhuyenMai();
            window.ShowDialog();
        }

        private void ButtonTT_Click(object sender, RoutedEventArgs e)
        {
            ThanhToan window = new ThanhToan();
            window.ShowDialog();
        }


        private void ButtonNhap_Click(object sender, RoutedEventArgs e)
        {

        }

        //Click Cell from DataGridSP
        private void DataGridSPCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            SanPham temp = row.Item as SanPham;

        }
    }
}
