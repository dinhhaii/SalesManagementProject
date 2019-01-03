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
    /// Interaction logic for LyDo.xaml
    /// </summary>
    public partial class LyDo : Window
    {
        public string Ca { get; set; }
        public string editMaNV { get; set; }
        SqlConnection sqlConnection = null;
        ObservableCollection<DangKyLichLam> listDKLich = new ObservableCollection<DangKyLichLam>();
        public LyDo(string valueMaNV,int ca)
        {
            Ca = ""+ca;
            editMaNV = valueMaNV;
            InitializeComponent();
        }
        public void connectSQL(string sql, out SqlConnection sqlConnection)
        {
            //if (sqlConnection == null)
            sqlConnection = new SqlConnection(sql);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();

            try
            {
                //Kết nối tới CSDL
                connectSQL(App.sqlString, out sqlConnection);

                sqlCommand.CommandType = CommandType.Text;
                //tIỀN HÀNH cập nhật DỮ LIỆU VÀO SQL
                string date = "'" + DateTime.Today.Year  + "/" + DateTime.Today.Month + "/" + DateTime.Today.Day + "'";
               
                string sql = "update LichLam set CoMat=@CoMat, LyDo=@LyDo where LichLam.NgayLam="+date+ "and LichLam.Ca='"+Ca+"' and LichLam.MaNV=" + "'" + editMaNV + "'";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;
                sqlCommand.Parameters.Add("@CoMat", SqlDbType.Bit).Value = false;
                if (txtLyDo.Text != "")
                {
                    sqlCommand.Parameters.Add("@LyDo", SqlDbType.NVarChar).Value = txtLyDo.Text;
                }
                else
                {
                    sqlCommand.Parameters.Add("@LyDo", SqlDbType.NVarChar).Value = "";
                }
                int ret = sqlCommand.ExecuteNonQuery();

                if (ret > 0)
                {

                    MessageBox.Show("Điểm danh thành công");
                    sqlCommand.Cancel();
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Điểm danh không thành công");
            }


        }
    }
}
