   M S B A M L   `   `   `  ���� J  FSalesManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SalesManagement.ManHinhThongKeSalesManagement.ManHinhThongKe  E ADragablz, Version=0.0.3.203, Culture=neutral, PublicKeyToken=null5(clr-namespace:Dragablz;assembly=DragablzDragablz ,    &SalesManagement.ManHinhThongKe.ThongKe   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     >local,clr-namespace:SalesManagement.ManHinhThongKe     V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  7dragablz(clr-namespace:Dragablz;assembly=Dragablz  -     >�Name$  ThongKeUserControl�� � 	Resources  �� MergedDictionaries �� Source��  �� $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Flipper.xaml?��� $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?��� $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?��� $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?��� $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Blue.xaml?��� $J Dpack://application:,,,/Dragablz;component/Themes/materialdesign.xaml?�%    .��� .+��� .��� ���	#FF303030 � RowDefinitions �� $��30 ��� .+��� $��1��.+���  �� Orientation$ 
Horizontal=�.+��� 
�������$��Chọn ngày: ��$��20(�$��0 0 20 0q�(  "System.Windows.Controls.DatePicker  -   $��doanhthungay��$��15(�
�������$	��Top=�$
��Left=�$	��140��&   MaterialDesignThemes.Wpf.Flipper  $��Stretch=�$��1��   MaterialDesignCardFlipper#��[     FrontContent �� -   $��btnflipper1��$��50��  MaterialDesignFlatButton#��[    FlipCommand#��Z $��25(�
�������.��THỐNG KÊ THEO NGÀY   BackContent � $	��230�� �� $��50 ��� $	��180 �.+��� 
�������$��Stretch=�$ 
Horizontal=�$��3��.+��� $	��150�� & "MaterialDesignToolForegroundButton#��[ #��Z $
��Left=�.��'  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]	   Kind$	 	ArrowLeft=�$��Right=��� -   $��	txtblngay��$��	100 0 0 0q�$��20(�$��Center=�THỐNG KÊ THEO NGÀY� $��1��$	��180��$	��700��
 � ColumnDefinitions
 �� $��3* ��� $��3* ���  �� �� �� �� .+��� $��0��$
��Bold"�$��17(�$��Center=�
�������$(��"Số lượng sản phẩm tồn :���� $��1��$
��Bold"�$��17(�$��Center=�
�������$,��&Số lượng sản phẩm đã bán :���� $��2��$
��Bold"�$��17(�$��Center=�
�������$/��)Số lượng sản phẩm đổi trả :���� $��3��$
��Bold"�$��17(�$��Center=�
�������$��Doanh thu :���� -   $��slton��$��0��
�������$��1��$��17(�$��Center=�$��0���� -   $��slban��$��0��
�������$��1��$��17(�$��Center=�$��1���� -   $��sldoitra��$��0��
�������$��1��$��17(�$��Center=�$��2���� -   $��doanhthu��$��0��
�������$��1��$��17(�$��Center=�$��3���� $��2��$��3��$
��Bold"�$��17(�$��Center=�
�������$�� VNĐ���� $ 
Horizontal=�.+��� 
�������$��Chọn tháng: ��$��0 0 20 0q�$��20(��� -	   $��cbbox��$��60��$��15(�
��������� -
   $��	txtboxnam��$y�Hidden=�
�������$��/��$��	20 0 20 0q�$��20(��� -   $��cbboxnam��$y�Hidden=�$��60��$��15(�
�������  $��Stretch=�$��1��#��[   �� -   $��btnflipperthang��$��50��#��[ #��Z $��25(�
�������.��THỐNG KÊ THEO THÁNG � $	��230�� �� $��50 ��� $	��180 �.+��� 
�������$��Stretch=�$ 
Horizontal=�$��3��.+��� $	��150��#��[ #��Z $
��Left=�.��  $	 	ArrowLeft=�$��Right=��� -   $��
txtblthang��$��	100 0 0 0q�$��20(�$��Center=�THỐNG KÊ THEO THÁNG� $��1��$	��180��$	��700��
 �� $��3* ��� $��3* ���  �� �� �� �� .+��� $��0��$
��Bold"�$��17(�$��Center=�
�������$(��"Số lượng sản phẩm tồn :���� $��1��$
��Bold"�$��17(�$��Center=�
�������$,��&Số lượng sản phẩm đã bán :���� $��2��$
��Bold"�$��17(�$��Center=�
�������$/��)Số lượng sản phẩm đổi trả :���� $��3��$
��Bold"�$��17(�$��Center=�
�������$��Doanh thu :���� -   $��
sltonthang��$��0��
�������$��1��$��17(�$��Center=�$��0���� -   $��
slbanthang��$��0��
�������$��1��$��17(�$��Center=�$��1���� -   $��sldoitrathang��$��0��
�������$��1��$��17(�$��Center=�$��2���� -   $��doanhthuthang��$��0��
�������$��1��$��17(�$��Center=�$��3���� $��2��$��3��$
��Bold"�$��17(�$��Center=�
�������$�� VNĐ��