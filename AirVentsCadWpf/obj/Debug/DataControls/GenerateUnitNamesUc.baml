   M S B A M L   `   `   `  ����  AirVentsCad5    /AirVentsCadWpf.DataControls.GenerateUnitNamesUc   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � -   5        ���	#FFFFFEFE$	��700��5   9   $	��700��6I     � ColumnDefinitions  5
   
   �� 5      $
��344* �6    � RowDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��15 �6   �� 5      $��23 �6   �� 5      $	��375 �6   �� 5      $��18 �6   .+�5   
   � $��Stretch=�6   $��1��6-   $��Stretch=�6:   ���	#FFF3E7E76V   � 5   
   $��Stretch=�6   $��4��6-   $��Stretch=�6:   ���	#FFF3E7E7 J� RenderTransformOrigin$ 0.5,3.2&�6m   $��
0,79,0,-78q�6�   �� 5   
   $��0��6   $ ��Классификатор��6   $
��Left=�65   $��
41,0,0,137q�6P   $��Center=�6d   $��Center=�6�   $��24(�6�   $��42��6�   $��Bottom=�6�   $	��177��6�   �� 5#   
   -   5         ��Name$ CmbTypeLane��5#      $	��Top=�6(   $��108,137,0,0q�5%      $��22��6X   $
��Left=�6�   $	��226��6�   �� 5&   
   -   5        $ CmbBlockType��5&      $	��Top=�6)   $��108,168,0,0q�5'      $��22��6Y   $
��Left=�6�   $	��226��6�   �� 5)   
   $��Типоряд:��6   $
��Left=�6#   $��
34,133,0,0q�6>   $	��Top=�6R   $��26��6j   $��59��6v   �� 5*   
   $��Тип блока:��6   $
��Left=�6%   $��
25,164,0,0q�6@   $	��Top=�6T   $��26��6l   $��68��6x   &   System.Windows.Controls.DataGrid  5.   
   -   5          Name$ DataGridSql��5.      $��
25,0,26,23q�5/      $��3��50         
IsReadOnly . $��Stretch=�53      $��Stretch=�63      	RowHeight$ 30Q�55         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 56         SelectionMode$ Extended=�57      
�������$��5q�59      
�������	   RowBackground
	 �����$ 
0.766,0.49&�5<   /   �� 5?   
   -   5        
 ��Name$
 ListBoxTypeLine��5?       �� SelectionMode$ Multiple=�5@      $��72��5B      $��361,37,26,0q�5C      $	��Top=�5D      .@�5F      �� -   5        $��chb30��5F      $��Профиль 30 мм��5G      6O   �� 5H      -   5        $��chb50��5H      $��Профиль 50 мм��5I      6O   �� 5J      -   5        $��chb70��5J      $��Профиль 70 мм��5K      6O   �� 5L      -	   5        $
��chbF��5L      $��Бескаркасная��5M      6M   5P      �� 5S   
   -
   5         ��Name$ ZoneText��5S      $��23��6$   $��429,139,26,0q�60    �� TextWrapping$
 Wrap=�6F   $��TextBox��6Z   $	��Top=�6i   �� 5U   
   -   5         ��Name$ 
ButtonSave��5U      $(��"Сохранить имя узла��6%   $��367,168,26,0q�6B   $	��Top=�6X   $ 0.138,0.022&�6p   $��22��6�   �� 5V   
   $��Имя узла:��6   $
��Left=�6$   $��361,137,0,0q�6?   $	��Top=�6T   $ 0.132,0.462&�6l   $��26��6�   $��63��6�   5\      5]      