using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement
{
    class Chi
    {
        public string MaNV { get; set; }
        public double TongTien { get; set; }
        public string LyDo { get; set; }
        public DateTime ThoiGian { get; set; }
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
}
