   M S B A M L   `   `   `  ����  AirVentsCad-    'AirVentsCadWpf.AdminkaWindows.SettingsW   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Настройки��5   	    � WindowStyle$ 
ToolWindow=�6O   $��Center=�6h   $��Center=�6�    � SizeToContent$ WidthAndHeight=�6�    � WindowStartupLocation$ CenterScreen=�6�   .��5      � .+�5   
   � $��5q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5	      $
��Auto �6    �� 5
      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� -   5         ��Name$ SaveSettings��5      $��Сохранить��6+   $��Right=�6?   $��24��6[   $��3q�6g   $	��Top=�6r   $��75��6�   $��7��6�   ��6�   Z� �� 5      -   5         ��Name$ 	Developer��5      $��	Developer��6'   $
��Left=�6;   $��2,2,0,0q�6V   $	��Top=�6g   $��6��6   �� 5      -   5        $ TestPdmBaseName��5      $��TestPdmBaseName��6-   $
��Left=�6G   $��2,2,0,0q�6b   $	��Top=�6s   $��5��6�   �� 5      -   5        $ PdmBaseName��5      $��PdmBaseName��6)   $
��Left=�6?   $��2,2,0,0q�6Z   $	��Top=�6k   $��4��6�   �� 5      -   5        $ DestinationFolder��5      $��DestinationFolder��6/   $
��Left=�6K   $��2,2,0,0q�6f   $	��Top=�6w   $��3��6�   �� 5      -   5        $ SourceFolder��5      $��SourceFolder��6*   $
��Left=�6A   $��2,2,0,0q�6\   $	��Top=�6m   $��2��6�   � 5      -   5         �Name$ DestBase��5      $
��Left=�6%   $��2q�6@   $	��Top=�6K   $��1��6c    � ColumnDefinitions 5      �� 5      $
��Auto �6'   �� 5      $
��Auto �6'   .+�5      �� -	   5        	 ��Name$	 SQLBase��5      $��0q�6,   
 �� SelectedIndex$
 0��67   $��1��6I    �� MaxDropDownHeight$ 60��6Y   $��80��6p   $��Center=�6   $��Center=�6�   ��6�   Z� �� 5      $��0��6   $*��$База для сохранения��6(   $
��Left=�6F   $��2q�6a   $	��Top=�6l   5      � 5      $
��Left=�6   $��2q�6.   $	��Top=�69    5       �� 5!      $
��Auto �6'   �� 5"      $
��Auto �6'   .+�5$      �� -
   5        $	 VaultsComboBox��5$      $��2q�63   $
 0��6>   $��1��6P   $ 60��6`   $��80��6w   $��Center=�6�   $��Center=�6�   ��6�   Z� �� 5%      $��0��6   $'��!Рабочее хранилище��6(   $
��Left=�6D   $��2q�6_   $	��Top=�6j   5&      �� 5'      -   5        $ ФайлЛоггера��5'      $��Лог файл��6*   $
��Left=�6=   $��24��6X   $��3q�6d   $��Center=�6o   $��75��6�   $��7��6�   ��6�   Z� 6�   5(      5*      5+      