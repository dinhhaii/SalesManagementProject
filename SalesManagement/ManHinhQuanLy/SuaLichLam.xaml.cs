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
    /// Interaction logic for SuaLichLam.xaml
    /// </summary>
    public partial class SuaLichLam : Window
    {
        SqlConnection sqlConnection = null;
        public string editMaNVLichLam { get; set; }
        ObservableCollection<LichLam> listLichLam = new ObservableCollection<LichLam>();
        ObservableCollection<LichLam> listLichLam1NV = new ObservableCollection<LichLam>();
        public bool isDelete { get; set; }
        public bool isEditing { get; set; }
        public SuaLichLam(string value)
        {
            editMaNVLichLam = value;
            InitializeComponent();
            getLichLam();
            isEditing = false;
            isDelete = false;
            for (int i=0;i<listLichLam.Count;i++)
            {
                if(listLichLam[i].MaNV==editMaNVLichLam)
                {
                    listLichLam1NV.Add(listLichLam[i]);
                }
            }
            DataGridLichLam.ItemsSource = listLichLam1NV;
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
                if (!sqlReader.IsDBNull(3))
                {
                    ll.isDiemDanh = 1;
                }
                else
                {
                    ll.isDiemDanh = 0;

                }
                if (!sqlReader.IsDBNull(4))
                {
                    ll.LyDo = sqlReader.GetString(4);
                }
                else
                {
                    ll.LyDo = "";
                }             
                    listLichLam.Add(ll);                
            }
            sqlReader.Close();
            sqlConnection.Close();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            DataGridRow dataGridRow = VisualTreeHelpers.FindAncestor<DataGridRow>(checkBox);
            int index = dataGridRow.GetIndex();

            if (checkBox.IsChecked == true)
            {
                dataGridRow.Background = Brushes.IndianRed;
                listLichLam1NV[index].isChecked = true;
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
                listLichLam1NV[index].isChecked = false;
            }
        }

        private void isCheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)e.OriginalSource;
            if (checkBox.IsChecked == true)
            {
                for (int i = 0; i < listLichLam1NV.Count; i++)
                {
                    listLichLam1NV[i].isChecked = true;
                }
            }
            else
            {
                for (int i = 0; i < listLichLam1NV.Count; i++)
                {
                    listLichLam1NV[i].isChecked = false;
                }
            }
            DataGridLichLam.ItemsSource = null;
            DataGridLichLam.ItemsSource = listLichLam1NV;
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell myCell = sender as DataGridCell;
            DataGridRow row = DataGridRow.GetRowContainingElement(myCell);

            //Nếu ở chế độ chỉnh sửa
            if (isDelete)
            {
                //try
                //{
                LichLam temp = row.Item as LichLam;

                for (int i = 0; i < listLichLam1NV.Count; i++)
                {
                    if (temp == listLichLam1NV[i])
                    {
                        //MessageBox.Show(listSP[i].isChecked.ToString());
                        if (listLichLam1NV[i].isChecked == false)
                        {
                            listLichLam1NV[i].isChecked = true;
                        }
                        else
                        {
                            listLichLam1NV[i].isChecked = false;
                        }
                        DataGridLichLam.ItemsSource = null;
                        DataGridLichLam.ItemsSource = listLichLam1NV;
                        break;
                    }
                }       
            }
            else
            {

                LichLam temp = row.DataContext as LichLam;
                string date ="'"+ temp.NgayLam.Year+"/"+temp.NgayLam.Month+"/"+temp.NgayLam.Day+"'";
                SuaChiTietLich suaChiTiet = new SuaChiTietLich(temp.MaNV, date, temp.Ca);
                //(Application.Current.Windows[4] as ChinhSuaSanPham).editMaSP = temp.MaSP;
                suaChiTiet.ShowDialog();

                refreshData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            for (int i = 0; i < listLichLam1NV.Count; i++)
            {
                if (listLichLam1NV[i].isChecked == true)
                {
                    count++;
                }
            }

            //Duyệt danh sách listSP và xóa dữ liệu từ Database
            if (count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa " + count.ToString() + " lịch làm này chứ?", "Sales Management", MessageBoxButton.YesNo, MessageBoxImage.Hand);
                if (result == MessageBoxResult.Yes)
                {
                    for (int i = 0; i < listLichLam1NV.Count; i++)
                    {
                        if (listLichLam1NV[i].isChecked == true)
                        {
                         

                            SqlCommand sqlCmd = new SqlCommand();
                            try
                            {
                                connectSQL(App.sqlString, out sqlConnection);
                                sqlCmd.CommandType = CommandType.Text;
                                string date = "'" + listLichLam1NV[i].NgayLam.Year + "/" + listLichLam1NV[i].NgayLam.Month + "/" + listLichLam1NV[i].NgayLam.Day + "'";
                               
                                sqlCmd.CommandText = "DELETE FROM LichLam WHERE LichLam.MaNV = '" + editMaNVLichLam +"'"+" and LichLam.NgayLam="+date+" and LichLam.Ca='"+listLichLam1NV[i].Ca.Trim()+"'";
                                sqlCmd.Connection = sqlConnection;
                                int ret = sqlCmd.ExecuteNonQuery();
                                if (ret > 0)
                                {
                                    MessageBox.Show("Xóa thành công!", "Sales Management");
                                  
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
        public void refreshData()
        {
            listLichLam1NV.Clear();
            listLichLam.Clear();
            getLichLam();
            for (int i = 0; i < listLichLam.Count; i++)
            {
                if (listLichLam[i].MaNV == editMaNVLichLam)
                {
                    listLichLam1NV.Add(listLichLam[i]);
                }
            }
           
            DataGridLichLam.ItemsSource = null;
            DataGridLichLam.ItemsSource = listLichLam1NV;
            DataGridLichLam.Items.Refresh();
        }
        private void btnDeleteLichLam_Click(object sender, RoutedEventArgs e)
        {
            if (!isDelete)
            {
                DataGridLichLam.Columns[0].Visibility = Visibility.Visible;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Close;
                isDelete = true;
                btnDeleteLichLam.Background = Brushes.Red;
                
                DataGridLichLam.CanUserSortColumns = false;
                btnDelete.Visibility = Visibility.Visible;
            }
            else
            {
                DataGridLichLam.Columns[0].Visibility = Visibility.Collapsed;
                icondeletebtn.Kind = MaterialDesignThemes.Wpf.PackIconKind.Delete;
                isDelete = false;
                btnDeleteLichLam.Background = new BrushConverter().ConvertFrom("#FF2962FF") as Brush;

                DataGridLichLam.CanUserSortColumns = true;
                btnDelete.Visibility = Visibility.Collapsed;
                for (int i = 0; i < listLichLam1NV.Count; i++)
                {
                    listLichLam1NV[i].isChecked = false;
                }
                DataGridLichLam.ItemsSource = null;
                DataGridLichLam.ItemsSource = listLichLam1NV;
            }
        }
    }
}
