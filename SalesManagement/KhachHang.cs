﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement
{
    class KhachHang
    {
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string GioiTinh { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string DiaChi { get; set; }
        private bool isSelected;
        public bool isChecked
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
            }
        }
    }
}
