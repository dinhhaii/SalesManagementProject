   M S B A M L   `   `   `  ����J  FSalesManagement, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:SalesManagement.ManHinhThongKeSalesManagement.ManHinhThongKe  5      E ADragablz, Version=0.0.3.203, Culture=neutral, PublicKeyToken=null5(clr-namespace:Dragablz;assembly=DragablzDragablz 5      l hSystem.Windows.Controls.DataVisualization.Toolkit, Version=4.0.0.0, Culture=neutral, PublicKeyToken=null�{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit2System.Windows.Controls.DataVisualization.Charting 5      ,    &SalesManagement.ManHinhThongKe.ThongKe   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      6dg-http://schemas.microsoft.com/wpf/2008/toolkit  5      8local,clr-namespace:SalesManagement.ManHinhThongKe  5      V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      7dragablz(clr-namespace:Dragablz;assembly=Dragablz  5      �chartingToolkit{clr-namespace:System.Windows.Controls.DataVisualization.Charting;assembly=System.Windows.Controls.DataVisualization.Toolkit  5      -   5          >�Name$  ThongKeUserControl��5	       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $g apack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Flipper.xaml?�6%   �� 5      $d ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml?�6%   �� 5      $h bpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml?�6%   �� 5      $t npack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml?�6%   �� 5      $s mpack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Blue.xaml?�6%   �� 5      $J Dpack://application:,,,/Dragablz;component/Themes/materialdesign.xaml?�6%   %    5      .��5      � .+�5   
   �� .��5      � ���	#FF303030 � RowDefinitions 5!      �� 5"      $��30 �6$   62   �� 5#      6&   .+�5&      �� $��1��6   .+�5'      ��  �� Orientation$ 
Horizontal=�6!   .+�5(      �� 
�������$��Chọn ngày: ��67   $��20(�6J   $��0 0 20 0q�6X   6l   (  "System.Windows.Controls.DatePicker  5)      -   5        $��doanhthungay��5)   %   $��15(�69   
�������$	��Top=�6Z   $
��Left=�6r   $	��140��6�   6�   5*      &   MaterialDesignThemes.Wpf.Flipper  5,      $��Stretch=�5-      $��1��5.         MaterialDesignCardFlipper#��[  5/         FrontContent 50      �� 51      -   5        $��btnflipper1��52      $��50��53        MaterialDesignFlatButton#��[ 54         FlipCommand#��Z 55      $��25(�56      
�������.��57   (   THỐNG KÊ THEO NGÀY59         BackContent 5;      � 5<      $	��230��6#    5=   "   �� 5>   &   $��50 �64   6B   �� 5?   &   $	��180 �64   6C   .+�5A   "   �� 
�������$��Stretch=�6@   $ 
Horizontal=�6^   $��3��6w   .+�5B   &   �� $	��150��5C   !    & "MaterialDesignToolForegroundButton#��[ 5D      #��Z 5E      $
��Left=�5F      .��5G   *   '  !MaterialDesignThemes.Wpf.PackIcon  �  �ControlzEx.PackIconBase`1[[MaterialDesignThemes.Wpf.PackIconKind, MaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=null]]	   Kind$	 	ArrowLeft=�5H   %   $��Right=�5I   %   5J   '   �� 5K   &   -   5        $��	txtblngay��5L   !   $��	100 0 0 0q�5M   !   $��20(�5N   !   $��Center=�5O   !   THỐNG KÊ THEO NGÀY5Q   '   5R   #   � 5T   "   $��1��6'   $	��180��64   $	��700��6A   
 � ColumnDefinitions
 5U   &   �� 5V   *   $��3* �6;   6H   �� 5W   *   $��3* �6;   6H   �� 5X   *   6=    5[   &   �� 5\   *   6:   �� 5]   *   6:   �� 5^   *   6:   �� 5_   *   6:   .+�5b   &   �� $��0��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$(��"Số lượng sản phẩm tồn :��6�   6�   �� 5c   &   $��1��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$,��&Số lượng sản phẩm đã bán :��6�   6�   �� 5d   &   $��2��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$/��)Số lượng sản phẩm đổi trả :��6�   6�   �� 5e   &   $��3��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$��Doanh thu :��6�   6�   �� 5g   &   -   5        $��slton��5g   .   $��0��6;   
�������$��1��6W   $��17(�6g   $��Center=�6u   $��0��6�   6�   �� 5h   &   -   5        $��slban��5h   .   $��0��6;   
�������$��1��6W   $��17(�6g   $��Center=�6u   $��1��6�   6�   �� 5i   &   -   5        $��sldoitra��5i   .   $��0��6>   
�������$��1��6Z   $��17(�6j   $��Center=�6x   $��2��6�   6�   �� 5j   &   -   5        $��doanhthu��5j   .   $��0��6>   
�������$��1��6Z   $��17(�6j   $��Center=�6x   $��3��6�   6�   �� 5m   &   $��2��60   $��3��6@   $
��Bold"�6M   $��17(�6_   $��Center=�6m   
�������$�� VNĐ��6�   6�   5n   #   5o      5q      �� 5s      $ 
Horizontal=�6!   .+�5t      �� 
�������$��Chọn tháng: ��67   $��0 0 20 0q�6K   $��20(�6]   6m   �� 5u      -	   5        $��cbbox��5u   M   $��60��6Z   $��15(�6e   
�������6�   �� 5v      -
   5        $��	txtboxnam��5v   9   $y�Hidden=�6$   
�������$��/��6^   $��	20 0 20 0q�6g   $��20(�6z   6�   �� 5w      -   5        $��cbboxnam��5w   f   $y�Hidden=�6#   $��60��6v   $��15(�6�   
�������6�   5y        5{      $��Stretch=�5|      $��1��5}      #��[  5~       5      �� 5�      -   5        $��btnflipperthang��5�      $��50��5�      #��[ 5�      #��Z 5�      $��25(�5�      
�������.��5�   (   THỐNG KÊ THEO THÁNG5�       5�      � 5�      $	��230��6#    5�   "   �� 5�   &   $��50 �64   6B   �� 5�   &   $	��180 �64   6C   .+�5�   "   �� 
�������$��Stretch=�6@   $ 
Horizontal=�6^   $��3��6w   .+�5�   &   �� $	��150��5�   !   #��[ 5�      #��Z 5�      $
��Left=�5�      .��5�   *     $	 	ArrowLeft=�5�   %   $��Right=�5�   %   5�   '   �� 5�   &   -   5        $��
txtblthang��5�   !   $��	100 0 0 0q�5�   !   $��20(�5�   !   $��Center=�5�   !   THỐNG KÊ THEO THÁNG5�   '   5�   #   � 5�   "   $��1��6'   $	��180��64   $	��700��6A   
 5�   &   �� 5�   *   $��3* �6;   6H   �� 5�   *   $��3* �6;   6H   �� 5�   *   6=    5�   &   �� 5�   *   6:   �� 5�   *   6:   �� 5�   *   6:   �� 5�   *   6:   .+�5�   &   �� $��0��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$(��"Số lượng sản phẩm tồn :��6�   6�   �� 5�   &   $��1��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$,��&Số lượng sản phẩm đã bán :��6�   6�   �� 5�   &   $��2��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$/��)Số lượng sản phẩm đổi trả :��6�   6�   �� 5�   &   $��3��60   $
��Bold"�6=   $��17(�6O   $��Center=�6]   
�������$��Doanh thu :��6�   6�   �� 5�   &   -   5        $��
sltonthang��5�   .   $��0��6@   
�������$��1��6\   $��17(�6l   $��Center=�6z   $��0��6�   6�   �� 5�   &   -   5        $��
slbanthang��5�   .   $��0��6@   
�������$��1��6\   $��17(�6l   $��Center=�6z   $��1��6�   6�   �� 5�   &   -   5        $��sldoitrathang��5�   .   $��0��6C   
�������$��1��6_   $��17(�6o   $��Center=�6}   $��2��6�   6�   �� 5�   &   -   5        $��doanhthuthang��5�   .   $��0��6C   
�������$��1��6_   $��17(�6o   $��Center=�6}   $��3��6�   6�   �� 5�   &   $��2��60   $��3��6@   $
��Bold"�6M   $��17(�6_   $��Center=�6m   
�������$�� VNĐ��6�   6�   5�   #   5�      5�      � 5�      $	��400��6   .+�5�      >  8System.Windows.Controls.DataVisualization.Charting.Chart  -   5        $��columnChart��5�   d   
�������$��Stretch=�6E      Title$N HSản phẩm bán chạy (Số lượng sản phẩm đã bán được)��6w   $��Stretch=�6�      Series. 5�      E  ?System.Windows.Controls.DataVisualization.Charting.ColumnSeries  ?  9System.Windows.Controls.DataVisualization.Charting.Series   Title$ Số lượng��6;   
���   �H  BSystem.Windows.Controls.DataVisualization.Charting.DataPointSeries   DependentValuePath$ Value��6_      IndependentValuePath$	 Key��6z      ItemsSource 6�   �� 5�      5�      5�      5�      5�      5�      5�      