   M S B A M L   `   `   `  ����J  FSalesManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullM+clr-namespace:SalesManagement.ManHinhQuanLySalesManagement.ManHinhQuanLy  5   	   E ADragablz, Version=0.0.3.203, Culture=neutral, PublicKeyToken=null5(clr-namespace:Dragablz;assembly=DragablzDragablz 5
   	   .    (SalesManagement.ManHinhQuanLy.ChinhSuaNV   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   =local+clr-namespace:SalesManagement.ManHinhQuanLy     5   	   V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      7dragablz(clr-namespace:Dragablz;assembly=Dragablz  5
   	     � Title$  
ChinhSuaNV��5   	   $	��600��6   $	��900��6)      MaterialDesignBody#���   5   	   $��Regular"�5   	   $��15(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?�6%   �� 5      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Blue.xaml?�6%   �� 5      $J Dpack://application:,,,/Dragablz;component/Themes/materialdesign.xaml?�6%   %    5      .��5      � ���	#FF303030 � ColumnDefinitions 5    
   �� 5!      6!   .+�5#   
   � $��50��6   $��10,44,390,476q�6    5$      �� 5%      $	��150 �6#   61   �� 5&      6&   .+�5(      �� $��Mã nhân viên��6   $��10,16,30,-16q�6,   �� 5)      -   5        $��txtMaNV��5*      $��1��5+      $��Center=�5-      $��
0,10,22,10q�60   .��6D   5.      50      �� 51   
   $��Tài khoản��6    �� TextAlignment$ Center=�6%   $��645,26,135,516q�6<   � 52   
   $��50��6   $��10,99,390,421q�6    53      �� 54      $	��150 �6#   �� 55      .+�57      �� $��Tên nhân viên��6   $��10,16,35,-16q�6-   �� 58      -   5         ��Name$ txtTenNV��59      $��1��5:      $��Center=�5<      $��
0,10,21,10q�6,   5=      � 5>   
   $��50��6   $��10,205,390,315q�6    5?      �� 5@      $	��150 �6#   �� 5A      .+�5C      �� $��Email��6   $��10,16,36,-16q�6%   �� 5D      -   5        $ txtMail��5E      $��1��5F      $��Center=�5H      $��30��6,   $��
0,10,21,10q�68   5I      � 5J   
   $��50��6   $��10,250,390,270q�6    5K      �� 5L      $	��150 �6#   �� 5M      .+�5O      �� $��Địa chỉ��6   $��10,16,47,-16q�6'   �� 5P      -   5        $ 	txtDiaChi��5Q      $��1��5R      $��Center=�5T      $��30��6,   $��
0,10,21,10q�68   5U      � 5V   
   $��50��6   $��10,300,390,220q�6    5W      �� 5X      $	��150 �6#   �� 5Y      .+�5[      �� $��Số điện thoại��6   $��10,16,47,-16q�6-   �� 5\      -   5        $ txtSDT��5]      $��1��5^      $��Center=�5`      $��30��6,   $��
0,10,22,10q�68   5a      � 5b   
   $��50��6   $��10,350,390,170q�6    5c      �� 5d      $	��150 �6#   �� 5e      .+�5g      �� $��	Vị trí��6   $��10,16,44,-16q�6&   �� 5h      -   5        $ txtViTri��5i      $��1��5j      $��Center=�5l      $��30��6,   $��
0,10,21,10q�68   5m      � 5n   
   $��50��6   $��10,400,390,120q�6    5o      �� 5p      $	��150 �6#   �� 5q      .+�5s      �� $��Lương��6   $��10,16,45,-16q�6%   �� 5t      -   5        $ txtLuong��5u      $��1��5v      $��Center=�5x      $��30��6,   $��
0,10,19,10q�68   5y      �� 5z   
   -   5        $��	btnUpdate��5{      $��40��5|      $	��150��5}        MaterialDesignRaisedButton#���  5~      +  %MaterialDesignThemes.Wpf.ShadowAssist	   ShadowDepth$	 Depth4=�5      $��529,355,213,166q�6B   .��6t   Cập nhật5�      �� 5�   
   -	   5        $��btnExit��5�      $��40��5�      $	��150��5�      #���  5�      $	 Depth4=�5�      $��722,355,20,166q�6B   .��6r   Thoát5�      � 5�   
   $��50��6   $��565,64,20,456q�6    5�      �� 5�      $	��150 �6#   �� 5�      .+�5�      �� $��Tên tài khoản��6   $��10,16,45,-16q�6-   �� 5�      -
   5        $ txtTenTaiKhoan��5�      $��1��5�      $��Center=�5�      $��30��6,   $��
0,10,19,10q�68   5�      � 5�   
   $��50��6   $��565,114,20,406q�6    5�      �� 5�      $	��150 �6#   �� 5�      .+�5�      �� $��Mật khẩu��6   $��10,16,45,-16q�6(   G� 5�      -   5        
 G�Name$
 passWord��5�      $��1��5�      $��Center=�5�      $��30��6,   $��
0,10,19,10q�68   5�      � 5�   
   $��50��6   $��10,154,390,366q�6    5�      �� 5�      $	��150 �6#   �� 5�      .+�5�      �� $��Giới tính��6   $��10,16,45,-16q�6)   �� 5�      -   5        $ txtGioiTinh��5�      $��1��5�      $��Center=�5�      $��30��6,   $��
0,10,19,10q�68   5�      � 5�   
   $��50��6   $��565,169,20,351q�6    5�      �� 5�      $	��150 �6#   �� 5�      .+�5�      �� $��Nhập lại mật khẩu��6   $��10,16,10,-16q�61   G� 5�      -   5        $
 	passWord1��5�      $��1��5�      $��Center=�5�      $��30��6,   $��
0,10,19,10q�68   5�      5�      5�      