using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
namespace SalesManagement
{
    class LichLam
    {
        public string MaNV { get; set; }
        public DateTime NgayLam { get; set; }
        public string Ca { get; set; }
        public bool CoMat { get; set; }
        public int isDiemDanh { get; set; }
        public string LyDo { get; set; }
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
