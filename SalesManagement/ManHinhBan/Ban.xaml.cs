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
using SalesManagement;

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
        List<SP_KH> listSPKH = new List<SP_KH>();
        List<SanPhamMua> listSPMua = new List<SanPhamMua>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool isDelete { get; set; }
        public bool isEditing { get; set; }
        public string TenKHMua { get; set; }

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
            khuyenmai.Text = "0%";
            thanhtien.Text = "0đ";
            tongtien.Text = "0đ";
            this.DataContext = this;
        }

        public class SanPhamMua : SP_KH
        {
            public string TenKH { get; set; }
            public string TenSP { get; set; }
            public double Gia { get; set; }
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
                kh.MaKH = sqlReader.GetString(0).Trim();
                kh.TenKH = sqlReader.GetString(1).Trim();
                kh.GioiTinh = sqlReader.GetString(2).Trim();
                kh.SDT = sqlReader.GetString(3).Trim();
                kh.Email = sqlReader.GetString(4).Trim();
                kh.DiaChi = sqlReader.GetString(5).Trim();

                if (kh != null)
                {
                    listKH.Add(kh);
                }
            }
            sqlReader.Close();
            sqlConnection.Close();
        }

        public void getSP_KH()
        {
            connectSQL(App.sqlString, out sqlConnection);
            SqlCommand sqlCom = new SqlCommand();
            sqlCom.CommandType = CommandType.Text;
            sqlCom.CommandText = "select * from SP_KH";
            sqlCom.Connection = sqlConnection;
            SqlDataReader sqlReader = sqlCom.ExecuteReader();
            while (sqlReader.Read())
            {
                SP_KH spkh = new SP_KH();
                spkh.MaSP = sqlReader.GetString(0).Trim();
                spkh.MaKH = sqlReader.GetString(1).Trim();
                spkh.STT = sqlReader.GetString(2).Trim();
                spkh.NgayBan = sqlReader.GetDateTime(3);
                spkh.SoLuong = sqlReader.GetInt32(4);
                spkh.KhuyenMai = sqlReader.GetFloat(5);
                
                if (spkh != null)
                {
                    listSPKH.Add(spkh);
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


        private void DataGridKHCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            KhachHang temp = row.Item as KhachHang;
            if (temp != null)
            {
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
                        ChinhSuaKhachHang window = new ChinhSuaKhachHang(temp.MaKH);
                        window.ShowDialog();
                        refreshDataKH();
                    }
                }
                else
                {
                    if (temp != null)
                    {
                        TenKHMua = temp.TenKH;
                        customername.Text = temp.TenKH;
                        DataGridSP.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        //Click Cell from DataGridSP
        private void DataGridSPCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);
            TextBox myTextBox = VisualTreeHelpers.FindChild<TextBox>(row);
            //Thông tin về sản phẩm được chọn (temp)
            SanPham temp = row.Item as SanPham;

            int i = 0;
            for(; i < listSPMua.Count; i++)
            {
                if(listSPMua[i].TenSP == temp.TenSP)
                {
                    bool themsp = true;
                    int result;
                    if (int.TryParse(myTextBox.Text, out result))
                    {
                        int SL = int.Parse(myTextBox.Text) + 1;
                        //Nếu số lượng sản phẩm lớn hơn trong kho
                        foreach (SanPham sp in listSP)
                        {
                            if (sp.MaSP == temp.MaSP)
                            {
                                if (SL > sp.SoLuong)
                                {
                                    themsp = false;
                                    break;
                                }
                            }
                        }
                        if (themsp)
                        {
                            myTextBox.Text = SL.ToString();
                            listSPMua[i].SoLuong = SL;
                            listSPMua[i].Gia = temp.Gia * (100 - listSPMua[i].KhuyenMai) / 100f * (double)SL;
                        }
                        else
                        {
                            myTextBox.Text = (SL - 1).ToString();
                        }
                    }
                    break;
                }
            }

            SanPhamMua tmp = new SanPhamMua();
            if (i == listSPMua.Count)
            {
                bool themsp = true;
                int result;
                if (int.TryParse(myTextBox.Text, out result))
                {
                    int SL = int.Parse(myTextBox.Text) + 1;
                    //Nếu số lượng sản phẩm lớn hơn trong kho
                    foreach (SanPham sp in listSP)
                    {
                        if (sp.MaSP == temp.MaSP)
                        {
                            if(SL > sp.SoLuong)
                            {
                                themsp = false;
                                break;
                            }
                        }
                    }
                    if (themsp)
                    {
                        myTextBox.Text = SL.ToString();
                        tmp.SoLuong = SL;
                    }
                    else
                    {
                        myTextBox.Text = (SL - 1).ToString();
                    }
                }

                if (themsp == true) {
                    //Thông tin không thay đổi
                    foreach (KhachHang item in listKH)
                    {
                        if (item.TenKH == TenKHMua)
                        {
                            tmp.MaKH = item.MaKH;
                            break;
                        }
                    }
                    tmp.TenKH = TenKHMua;
                    tmp.TenSP = temp.TenSP;
                    tmp.MaSP = temp.MaSP;
                    tmp.NgayBan = DateTime.Now;

                    //Thông tin được thêm có thay đổi       
                    tmp.KhuyenMai = 0;
                    tmp.Gia = temp.Gia * ((float)100 - tmp.KhuyenMai) / 100f * (double)tmp.SoLuong;
                    listSPMua.Add(tmp);
                }
            }
            updateTotal();
        }

        public void updateTotal()
        {
            ListViewSPMua.ItemsSource = null;
            ListViewSPMua.ItemsSource = listSPMua;

            double tong = 0f;
            foreach (SanPhamMua item in listSPMua)
            {
                tong += item.Gia;
            }
            tongtien.Text = tong.ToString() + "đ";
            string km = khuyenmai.Text.Substring(0, khuyenmai.Text.Length - 1);
            thanhtien.Text = (tong * (100f - (float)int.Parse(km)) / 100f).ToString() + "đ";
        }

        private void decrease_Click(object sender, RoutedEventArgs e)
        {
            SanPham obj = ((FrameworkElement)sender).DataContext as SanPham;
            for(int i = 0;i < DataGridSP.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)DataGridSP.ItemContainerGenerator.ContainerFromIndex(i);
                if((row.Item as SanPham).MaSP == obj.MaSP)
                {
                    TextBox myTextBox = VisualTreeHelpers.FindChild<TextBox>(row);
                    int result;
                    if (int.TryParse(myTextBox.Text, out result))
                    {
                        int SL = int.Parse(myTextBox.Text) - 1;
                        if (SL >= 0)
                        {
                            for (int index = 0; index < listSPMua.Count; index++)
                            {
                                if (listSPMua[index].MaSP == obj.MaSP)
                                {
                                    if(SL == 0)
                                    {
                                        listSPMua.RemoveAt(index);
                                    }
                                    else
                                    {   
                                        listSPMua[index].SoLuong = SL;
                                        listSPMua[index].Gia = obj.Gia * (100 - listSPMua[index].KhuyenMai) / 100f * (double)SL;
                                    }
                                    myTextBox.Text = SL.ToString();
                                    break;
                                }
                            }
                        }  
                        break;
                    }
                }
            }

            updateTotal();
        }

        //Click ListViewItem
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SanPhamMua obj = ((FrameworkElement)sender).DataContext as SanPhamMua;
            KhuyenMai1SP window = new KhuyenMai1SP();
            window.ShowDialog();
            float _khuyenmai = float.Parse(KhuyenMai1SP.KM, System.Globalization.CultureInfo.InvariantCulture);
            obj.KhuyenMai = _khuyenmai;
            for(int i = 0; i < listSPMua.Count; i++)
            {
                if(obj.MaSP == listSPMua[i].MaSP)
                {
                    listSPMua[i].KhuyenMai = _khuyenmai;
                    break;
                }
            }
            for (int i = 0; i < listSP.Count; i++)
            {
                if (listSP[i].MaSP == obj.MaSP)
                {
                    obj.Gia = listSP[i].Gia * (100 - obj.KhuyenMai) / 100f * obj.SoLuong;
                    break;
                }
            }
            updateTotal();
        }

        //Mở cửa sổ nhập Khuyến mãi
        private void ButtonKM_Click(object sender, RoutedEventArgs e)
        {
            KhuyenMai window = new KhuyenMai();
            window.ShowDialog();
            string _khuyenmai = KhuyenMai.KM;
            khuyenmai.Text = _khuyenmai + "%";
            updateTotal();
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
            DataGridSP.ItemsSource = null;
            DataGridSP.ItemsSource = listSP;
            DataGridSP.Items.Refresh();
        }


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

        private void ButtonTT_Click(object sender, RoutedEventArgs e)
        {
            ThanhToan window = new ThanhToan(thanhtien.Text);
            window.ShowDialog();
            if (ThanhToan.thanhtoanthanhcong == true)
            {
                getSP_KH();
                bool input = false;
                //Kiểm tra xem có trùng mã sản phẩm trong List SP_KH
                bool ttthanhcong = false;
                foreach (SanPhamMua item in listSPMua)
                {
                    int count = 0;
                    for (int i = 0; i < listSPKH.Count; i++)
                    {
                        if (item.MaSP == listSPKH[i].MaSP)
                        {
                            count++;
                        }
                    }

                    SP_KH spkh = new SP_KH();
                    SqlCommand sqlCommand = new SqlCommand();
                    try
                    {
                        //Kết nối tới CSDL
                        connectSQL(App.sqlString, out sqlConnection);

                        sqlCommand.CommandType = CommandType.Text;
                        //tIỀN HÀNH THÊM DỮ LIỆU VÀO SQL
                        string sql = "insert into SP_KH(MaSP,MaKH,STT,NgayBan,SoLuong,KhuyenMai) values(@MaSP,@MaKH,@STT,@NgayBan,@SoLuong,@KhuyenMai)";
                        sqlCommand.CommandText = sql;
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.Parameters.Add("@MaSP", SqlDbType.NChar).Value = item.MaSP.Trim();
                        sqlCommand.Parameters.Add("@MaKH", SqlDbType.NChar).Value = item.MaKH.Trim();
                        sqlCommand.Parameters.Add("@STT", SqlDbType.NChar).Value = (1 + count).ToString();
                        sqlCommand.Parameters.Add("@NgayBan", SqlDbType.Date).Value = item.NgayBan;
                        sqlCommand.Parameters.Add("@SoLuong", SqlDbType.Int).Value = item.SoLuong;
                        sqlCommand.Parameters.Add("@KhuyenMai", SqlDbType.Real).Value = item.KhuyenMai; ;
                        input = true;
                    }
                    catch (Exception)
                    {
                        //NẾU NHẬP KHÔNG ĐÚNG BÁO LỖI
                        MessageBox.Show("Thanh toán không thành công! Lỗi cơ sở dữ liệu", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Hand); ;
                        input = false;
                    }
                    //Nếu nhập đúng
                    if (input == true)
                    {
                        int ret = sqlCommand.ExecuteNonQuery();
                        if (ret > 0)
                        {
                            sqlCommand.Cancel();
                            foreach (SanPham sp in listSP)
                            {
                                if (sp.MaSP.ToString().ToLower() == item.MaSP.ToString().ToLower())
                                {
                                    int sl = sp.SoLuong - item.SoLuong;
                                    if (sl > 0)
                                    {
                                        sqlCommand.CommandText = "UPDATE SanPham SET SoLuong=" + sl.ToString() + " WHERE MaSP='" + sp.MaSP + "'";
                                    }
                                    else
                                    {
                                        sqlCommand.CommandText = "DELETE FROM SanPham WHERE MaSP='" + sp.MaSP + "'";
                                    }
                                    try
                                    {
                                        int res = sqlCommand.ExecuteNonQuery();
                                        if (res > 0)
                                        {
                                            ttthanhcong = true;
                                        }
                                        else
                                        {
                                            ttthanhcong = false;
                                            MessageBox.Show("Thanh toán không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Hand); ;

                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ttthanhcong = false;
                                        MessageBox.Show("Thanh toán không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Hand); ;

                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ttthanhcong = false;
                            MessageBox.Show("Thanh toán không thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Hand); ;

                        }
                    }
                    if (sqlConnection.State == ConnectionState.Open)
                        sqlConnection.Close();
                    sqlCommand.Cancel();
                }
                if (ttthanhcong == true)
                {
                    MessageBox.Show("Thanh toán thành công!", "Sales Management", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    
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
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa " + count.ToString() + " khách hàng chứ?", "Sales Management", MessageBoxButton.YesNo, MessageBoxImage.Hand);
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

        private void btnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditing)
            {
                isEditing = true;
                status.Text = "Chọn các mục cần chỉnh sửa";
                iconeditbutton.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                btnEditCustomer.Background = Brushes.Red;
                btnEditCustomer.Margin = new Thickness(0, 0, 25, 20);
                btnDeleteCustomer.Visibility = Visibility.Collapsed;
                btnAddCustomer.Visibility = Visibility.Collapsed;
                DataGridSP.Visibility = Visibility.Collapsed;
                DataGridKH.CanUserSortColumns = false;
            }
            else
            {
                isEditing = false;
                status.Text = "";
                iconeditbutton.Kind = MaterialDesignThemes.Wpf.PackIconKind.Edit;
                btnEditCustomer.Background = new BrushConverter().ConvertFrom("#FF2962FF") as Brush;
                btnEditCustomer.Margin = new Thickness(0, 0, 85, 20);
                btnDeleteCustomer.Visibility = Visibility.Visible;
                btnAddCustomer.Visibility = Visibility.Visible;
                DataGridKH.CanUserSortColumns = true;
            }
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            listSPMua.Clear();
            listSP.Clear();
            listKH.Clear();
            getKH();
            getSP();
            DataGridSP.ItemsSource = null;
            DataGridSP.ItemsSource = listSP;
            DataGridKH.ItemsSource = null;
            DataGridKH.ItemsSource = listKH;
            ListViewSPMua.ItemsSource = null;
            ListViewSPMua.ItemsSource = listSPMua;
        }
    }
}
