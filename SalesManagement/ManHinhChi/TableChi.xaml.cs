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

namespace SalesManagement.ManHinhChi
{
    /// <summary>
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class TableChi : UserControl
    {
        class ChiCucBo
        {
            public string MaNV1 { get; set; }
            public double TongTien1 { get; set; }
            public string LyDo1 { get; set; }
            public DateTime ThoiGian1 { get; set; }
            public string TenNV1 { get; set; }
            public bool isSelected = false;
            public bool isChecked
            {
                get { return isSelected; }
                set
                {
                    isSelected = value;
                    //OnPropertyChanged("IsChecked");
                }
            }
        }
        SqlConnection sqlConnection = null;
        ObservableCollection<ChiCucBo> listNVSearch = new ObservableCollection<ChiCucBo>();
        ObservableCollection<ChiCucBo> listChiCucBo = new ObservableCollection<ChiCucBo>();
        ObservableCollection<Chi> listChi = new ObservableCollection<Chi>();
        ObservableCollection<NhanVien> listNV = new ObservableCollection<NhanVien>();

        public bool isDelete { get; set; }
        public bool isEditing { get; set; }

        public TableChi()
        {
            InitializeComponent();
            getDataNV();
            getDataChi();
            chuyenDuLieu();
            isEditing = false;
            isDelete = false;
            DataGridNhap.ItemsSource = listChiCucBo;

        }

        //Connect to SQL Server
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }

        public void getDataNV()
        {
            //Lấy danh sách sản phẩm từ csdl
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
                listNV.Add(nv);
            }
            sqlReader.Close();
            sqlConnection.Close();
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

        public void chuyenDuLieu()
        {
            for (int i=0;i<listNV.Count;i++)
            {
                for (int j=0;j<listChi.Count;j++)
                {
                    if (listNV[i].MaNV == listChi[j].MaNV)
                    {
                        ChiCucBo chiCucBo = new ChiCucBo();
                        chiCucBo.MaNV1 = listNV[i].MaNV;
                        chiCucBo.TenNV1 = listNV[i].TenNV;
                        chiCucBo.LyDo1 = listChi[j].LyDo;
                        chiCucBo.ThoiGian1 = listChi[j].ThoiGian;
                        chiCucBo.TongTien1 = listChi[j].TongTien;
                        listChiCucBo.Add(chiCucBo);
                    }
                }
            }
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ThemChi window = new ThemChi();
            window.ShowDialog();
            refreshData();
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                status.Text = "Chọn các mục cần xóa";
                DataGridNhap.Columns[0].Visibility = Visibility.Visible;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                isDelete = true;
                btnDeleteProduct.Background = Brushes.Red;
                btnAddProduct.Visibility = Visibility.Collapsed;
                DataGridNhap.CanUserSortColumns = false;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                status.Text = "";
                DataGridNhap.Columns[0].Visibility = Visibility.Collapsed;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
                isDelete = false;
                btnDeleteProduct.Background = new BrushConverter().ConvertFrom("#FF2962FF") as Brush;
                btnAddProduct.Visibility = Visibility.Visible;
                DataGridNhap.CanUserSortColumns = true;
                btnDelete.Visibility = Visibility.Collapsed;
                for (int i = 0; i < listChiCucBo.Count; i++)
                {
                    listChiCucBo[i].isChecked = false;
                }
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listChiCucBo;
            }

        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);

            //Nếu ở chế độ chỉnh sửa
            if (isDelete)
            {
                try
                {
                    ChiCucBo temp = row.Item as ChiCucBo;

                    for (int i = 0; i < listChiCucBo.Count; i++)
                    {
                        if (temp == listChiCucBo[i])
                        {
                            //MessageBox.Show(listSP[i].isChecked.ToString());
                            if (listChiCucBo[i].isChecked == false)
                            {
                                listChiCucBo[i].isChecked = true;
                            }
                            else
                            {
                                listChiCucBo[i].isChecked = false;
                            }
                            DataGridNhap.ItemsSource = null;
                            DataGridNhap.ItemsSource = listChiCucBo;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Error");
                }
            }
            else
            {
                ChiCucBo temp = row.DataContext as ChiCucBo;
                if (temp != null)
                {
                    ChinhSuaChi window = new ChinhSuaChi(temp.MaNV1);
                    //(Application.Current.Windows[4] as ChinhSuaSanPham).editMaSP = temp.MaSP;
                    window.ShowDialog();
                    refreshData();
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();
            if (index < listChiCucBo.Count)
            {
                if (checkBox.IsChecked == false)
                {
                    Brush temp = new BrushConverter().ConvertFrom("#1FFFFFFF") as Brush;
                    dataGridRow.Background = temp;
                    listChiCucBo[index].isChecked = false;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();
            if (index < listChiCucBo.Count)
            {
                if (checkBox.IsChecked == true)
                {
                    dataGridRow.Background = Brushes.IndianRed;
                    listChiCucBo[index].isChecked = true;
                }
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < listChiCucBo.Count; i++)
            {
                if (listChiCucBo[i].isChecked == true)
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
                    for (int i = 0; i < listChiCucBo.Count; i++)
                    {
                        if (listChiCucBo[i].isChecked == true)
                        {
                            //listSP.RemoveAt(i);

                            SqlCommand sqlCmd = new SqlCommand();
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                string sqlFormattedDate = listChiCucBo[i].ThoiGian1.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                sqlCmd.CommandText = "DELETE FROM Chi WHERE Chi.ThoiGian = '"+ sqlFormattedDate + "'" ;
                                
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
            refreshData();
        }

        private void isCheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            if (checkBox.IsChecked == true)
            {
                for (int i = 0; i < listChiCucBo.Count; i++)
                {
                    listChiCucBo[i].isChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < listChiCucBo.Count; i++)
                {
                    listChiCucBo[i].isChecked = false;
                }
            }
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listChiCucBo;
        }

        public void refreshData()
        {
            listNV.Clear();
            listChi.Clear();
            listChiCucBo.Clear();
            getDataChi();
            getDataNV();
            chuyenDuLieu();
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listChiCucBo;
            DataGridNhap.Items.Refresh();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            refreshData();
        }

        //Tìm kiếm
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            resetTxtSearch.Visibility = Visibility.Visible;
            if (listNVSearch.Count > 0)
            {
                //Nếu tìm kiếm sản phẩm khác thì reset lại list sản phẩm tìm kiếm
                listNVSearch.Clear();
            }
            if (txtSearch.Text.Trim() == "")
            {
                resetTxtSearch.Visibility = Visibility.Collapsed;
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listChiCucBo;

            }
            else
            {
                for (int i = 0; i < listChiCucBo.Count; i++)
                {
                    string str = listChiCucBo[i].TenNV1.Trim().ToLower().Replace(" ", "");
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
                        int pos = listChiCucBo[i].TenNV1.ToLower().IndexOf(token[index]);
                        //Nếu không xuất hiện thì
                        if (pos < 0)
                        {
                            break;
                        }
                    }
                    if (index == token.Length)
                    {
                        listNVSearch.Add(listChiCucBo[i]);
                    }
                }

                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listNVSearch;
            }

        }

        private void resetTxtSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
        }
    }
}