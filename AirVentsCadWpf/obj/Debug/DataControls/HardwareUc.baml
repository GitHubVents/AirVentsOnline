   M S B A M L   `   `   `  ����  AirVentsCad,    &AirVentsCadWpf.DataControls.HardwareUc   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      � -   5        ���	#FFFFFEFE$	��500��5   9   $	��500��6H     � ColumnDefinitions  5
   
   �� 5      $	��400��6    � RowDefinitions 5   
   �� 5      $	��200 �6   �� 5      $��15 �6   �� 5      $��23 �6   �� 5      $	��241 �6   �� 5      $��19 �6   �� 5      $	��128 �6   �� 5      $��20 �6   .+�5   
   � $��Stretch=�6   $��1��6-   $��Stretch=�6:   ���	#FFF3E7E7$��2��6m   � 5   
   $��Stretch=�6   $��4��6-   $��Stretch=�6:   ���	#FFF3E7E7 J� RenderTransformOrigin$ 0.5,3.2&�6m   $��2��6�   �� 5   
   $��0��6   $ ��Комплектующие��6   $
��Left=�65   $��
41,0,0,137q�6P   $��Center=�6d   $��Center=�6�   $��24(�6�   $��42��6�   $��Bottom=�6�   $	��193��6�   �� 5"   
   -   5         ��Name$ CmbTypeLane��5"      $	��Top=�6(   $��108,137,0,0q�5$      $��22��6X   $
��Left=�6d   $	��141��6   �� 5%   
   -   5        $ CmbBlockType��5%      $	��Top=�6)   $��108,168,0,0q�5&      $��22��6Y   $
��Left=�6e   $	��141��6�   �� 5(   
   $��Типоряд:��6   $
��Left=�6#   $��
34,133,0,0q�6>   $	��Top=�6R   $��26��6j   $��59��6v   �� 5)   
   $��Тип блока:��6   $
��Left=�6%   $��
25,164,0,0q�6@   $	��Top=�6T   $��26��6l   $��68��6x   &   System.Windows.Controls.DataGrid  5+   
   -   5          Name$ DataGridSql2��5+      $
��10,0q�5,      $��3��5.      $	��Top=�5/      $	��222��50         	RowHeight$ 30Q�52         ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$
 auto 53         SelectionMode$ Extended=�54      
�������$��5q�56      
�������   RowBackground
 �����58   /   �� 5;   
   -   5        	 ��Name$	 BtnSaveEdit��5;      $+��%Сохранить изменения��6&   $
��Left=�6^   $��	72,33,0,0q�6y   $��5��6�   $	��Top=�6�   $	��162��6�   $��22��6�   �� 5<   
   -   5        $	 BtnAdd��5<      $1��+Добавить комплектующие��6!   $
��Left=�6W   $��	72,76,0,0q�6r   $��5��6�   $	��Top=�6�   $	��162��6�   $��22��6�   �� 5>   
   -   5        $	 	BtnDelete��5>      $��Удалить��6<   $
��Left=�6N   $��
255,33,0,0q�6i   $��5��6}   $	��Top=�6�   $��75��6�   $��22��6�   5@      5A      