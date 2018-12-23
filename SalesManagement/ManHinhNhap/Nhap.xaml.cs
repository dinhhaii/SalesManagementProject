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

namespace SalesManagement.ManHinhNhap
{
    /// <summary>
    /// Interaction logic for Nhap.xaml
    /// </summary>
    public partial class Nhap : UserControl
    {
        SqlConnection sqlConnection = null;
        ObservableCollection<SanPham> listSP = new ObservableCollection<SanPham>();
        ObservableCollection<SanPham> listSPSearch = new ObservableCollection<SanPham>();
        public bool isDelete { get; set; }
        public bool isEditing { get; set; }

        public Nhap()
        {
            InitializeComponent();
            getData();
            isEditing = false;
            isDelete = false;
            DataGridNhap.ItemsSource = listSP;     

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

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ThemSanPham window = new ThemSanPham();
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
                for(int i = 0; i < listSP.Count; i++)
                {
                    listSP[i].isChecked = false;
                }
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSP;
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
                    SanPham temp = row.Item as SanPham;

                    for (int i = 0; i < listSP.Count; i++)
                    {
                        if (temp == listSP[i])
                        {
                            //MessageBox.Show(listSP[i].isChecked.ToString());
                            if (listSP[i].isChecked == false)
                            {
                                listSP[i].isChecked = true;
                            }
                            else
                            {
                                listSP[i].isChecked = false;
                            }
                            DataGridNhap.ItemsSource = null;
                            DataGridNhap.ItemsSource = listSP;
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
                SanPham temp = row.DataContext as SanPham;
                if (temp != null)
                {
                    ChinhSuaSanPham window = new ChinhSuaSanPham(temp.MaSP);
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
            if (index < listSP.Count)
            {
                if (checkBox.IsChecked == false)
                {
                    Brush temp = new BrushConverter().ConvertFrom("#1FFFFFFF") as Brush;
                    dataGridRow.Background = temp;
                    listSP[index].isChecked = false;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();
            if (index < listSP.Count)
            {
                if (checkBox.IsChecked == true)
                {
                    dataGridRow.Background = Brushes.IndianRed;
                    listSP[index].isChecked = true;
                }
            }

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < listSP.Count; i++)
            {
                if (listSP[i].isChecked == true)
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
                    for (int i = 0; i < listSP.Count; i++)
                    {
                        if (listSP[i].isChecked == true)
                        {
                            //listSP.RemoveAt(i);

                            SqlCommand sqlCmd = new SqlCommand();
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                sqlCmd.CommandText = "DELETE FROM SanPham WHERE SanPham.MaSP = '" + listSP[i].MaSP + "'";
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
                for (int i = 0; i < listSP.Count; i++)
                {
                    listSP[i].isChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < listSP.Count; i++)
                {
                    listSP[i].isChecked = false;
                }
            }
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSP;
        }

        public void refreshData()
        {
            listSP.Clear();
            getData();
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSP;
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
            if (listSPSearch.Count > 0)
            {
                //Nếu tìm kiếm sản phẩm khác thì reset lại list sản phẩm tìm kiếm
                listSPSearch.Clear();
            }
            if (txtSearch.Text.Trim() == "")
            {
                resetTxtSearch.Visibility = Visibility.Collapsed;
                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSP;

            }
            else
            {
                for (int i = 0; i < listSP.Count; i++)
                {
                    string str = listSP[i].TenSP.Trim().ToLower().Replace(" ", "");
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
                        int pos = listSP[i].TenSP.ToLower().IndexOf(token[index]);
                        //Nếu không xuất hiện thì
                        if (pos < 0)
                        {
                            break;
                        }
                    }
                    if(index == token.Length)
                    {
                        listSPSearch.Add(listSP[i]);
                    }
                }

                DataGridNhap.ItemsSource = null;
                DataGridNhap.ItemsSource = listSPSearch;
            }

        }

        private void resetTxtSearch_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = "";
        }

        private void Label_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            listSP.Clear();
            getData();
            DataGridNhap.ItemsSource = null;
            DataGridNhap.ItemsSource = listSP;
        }
    }
}