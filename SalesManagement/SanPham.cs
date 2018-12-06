using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement
{
    class SanPham : INotifyPropertyChanged
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string HinhAnhSP { get; set; }
        public string Size { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
        public DateTime NgayNhap { get; set; }
        public string DoiTra { get; set; }
        public bool isSelected = false;
        public bool isChecked {
            get { return isSelected; }
            set
            {
                isSelected = value;
                //OnPropertyChanged("IsChecked");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

    }
}
