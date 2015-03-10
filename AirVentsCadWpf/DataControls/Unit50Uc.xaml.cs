using System;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AirVentsCadWpf.AirVentsClasses;
using ModelSw = AirVentsCadWpf.AirVentsClasses.UnitsBuilding.ModelSw;

namespace AirVentsCadWpf.DataControls
{
    /// <summary>
    /// Interaction logic for Unit50UC.xaml
    /// </summary>
    public partial class Unit50Uc
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Unit50Uc"/> class.
        /// </summary>
        public Unit50Uc()
        {
            InitializeComponent();

          //  GridTypeOfUnit50.Children.Add(new UnitElement());
            var sqlBaseData = new SqlBaseData();
            var airVentsStandardSize = sqlBaseData.AirVentsStandardSize();
            SizeOfUnit.ItemsSource = ((IListSource)airVentsStandardSize).GetList();
            SizeOfUnit.DisplayMemberPath = "Type";
            SizeOfUnit.SelectedIndex = 0;

            SectionTextBox.ItemsSource = new[]
                {
                    "A", "B", "C", "D", "E", "F", "G", "H",
                    "I","J", "K", "L", "M", "N", "O", "P", 
                    "R", "S","T", "U", "V","W", "X", "Y", "Z"
                };

            #region UNIT50FULL

            Lenght.MaxLength = 5;
            WidthU.MaxLength = 5;
            WidthU.IsReadOnly = true;
            HeightU.MaxLength = 5;
            HeightU.IsReadOnly = true;
            Lenght.MaxLength = 5;

            #endregion

            PanelGrid.Visibility = Visibility.Collapsed;
            InnerGrid.Visibility = Visibility.Collapsed;
            GridMontageFrame.Visibility = Visibility.Collapsed;
            GridRoof.Visibility = Visibility.Collapsed;

            WidthLabel1.Visibility = Visibility.Collapsed;
            WidthRoof.Visibility = Visibility.Collapsed;
            HeightLabel1.Visibility = Visibility.Collapsed;
            LenghtRoof.Visibility = Visibility.Collapsed;


            #region TypeOfUnit50

            ModelOfInnerLabel.Visibility = Visibility.Collapsed;
            ModelOfInner.Visibility = Visibility.Collapsed;
            AddTypeLabel.Visibility = Visibility.Collapsed;
            AddType.Visibility = Visibility.Collapsed;

            #endregion

            #region MontageFrame50 Initialize

            FrameOffset.MaxLength = 5;
            FrameOffset.IsReadOnly = true;
            FrameOffsetLabel.Visibility = Visibility.Collapsed;
            FrameOffset.Visibility = Visibility.Collapsed;
            
            #endregion;

           
        }

        private void SizeOfUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (SizeOfUnit == null || SizeOfUnit.SelectedItem == null)
                {
                    return;
                }
                var id = Convert.ToInt32(((DataRowView)SizeOfUnit.SelectedItem)["SizeID"].ToString());
                var sqlBaseData = new SqlBaseData();
                var standartUnitSizes = sqlBaseData.StandartSize(id, 2);

                if (WidthU == null || HeightU == null)
                {
                    return;
                }
                WidthU.Text = standartUnitSizes[0];
                HeightU.Text = standartUnitSizes[1];
            }
            catch (Exception)
            {
                if (WidthU != null) WidthU.Text = "";
                if (HeightU != null) HeightU.Text = "";
            }


            //if (SizeOfUnit == null)
            //{
            //    return;
            //}
            //var sqlBaseData = new SqlBaseData();
            //var standartUnitSizes =
            //    sqlBaseData.StandartSize(
            //        SizeOfUnit.SelectedItem.ToString().Replace("System.Windows.Controls.ListBoxItem: ", ""));

            //if (WidthU == null || HeightU == null)
            //{
            //    return;
            //}
            //WidthU.Text = standartUnitSizes[0];
            //HeightU.Text = standartUnitSizes[1];
        }

        private void nonstandard_Checked(object sender, RoutedEventArgs e)
        {
            WidthU.IsReadOnly = false;
            HeightU.IsReadOnly = false;
            WidthLabel.Visibility = Visibility.Visible;
            WidthU.Visibility = Visibility.Visible;
            HeightLabel.Visibility = Visibility.Visible;
            HeightU.Visibility = Visibility.Visible;
        }
       
        private void nonstandard_Unchecked(object sender, RoutedEventArgs e)
        {
            WidthU.IsReadOnly = true;
            HeightU.IsReadOnly = true;
        }

        private void BUILDING_Click(object sender, RoutedEventArgs e)
        {
            var sw = new ModelSw();

            try
            {
                if (FrameOffset.Text == "")
                {
                        FrameOffset.Text = Convert.ToString(Convert.ToDouble(Lenght.Text)/2);
                }

                var paneltype = new[] { null, TypeOfPanel50.Text };
                var m1 = MaterialP1.Text;
                var m2 = MaterialP2.Text;
                if (Panel50.IsChecked != true)
                {
                     paneltype[1] = "";
                     m1 = "";
                     m2 = "";
                }

                var mfthikness = Thikness.Text;
                var typeofframe = TypeOfFrame.Text;
                var frameoffcet = FrameOffset.Text;
                if (MontageFrame50.IsChecked != true)
                {
                    mfthikness = "";
                }

                var roofType = TypeOfRoof.Text;
                if (RoofOfUnit50.IsChecked != true)
                {
                    roofType = "";
                }
               // MessageBox.Show(roofType);
                var size = SizeOfUnit.Text;
                if (Nonstandard.IsChecked == true)
                {
                   // size = size + "N";
                }
               // Dispatcher.Invoke(() =>
                sw.UnitAsmbly(((DataRowView)SizeOfUnit.SelectedItem)["Type"].ToString(), OrderTextBox.Text, SideService.Text, WidthU.Text, HeightU.Text,
                        Lenght.Text, mfthikness, typeofframe,
                        frameoffcet, paneltype, m1, m2, roofType, "Section " + SectionTextBox.Text);

                //sw.AssemblyUnits(size, typeofframe, SideService.Text,
                //    sw.UnitAsmblyStr(size, TypeOfUnit.Text, SideService.Text, WidthU.Text, HeightU.Text,
                //    Lenght.Text, mfthikness, typeofframe, frameoffcet, paneltype, m1, m2, roofType),
                //    sw.UnitAsmblyStr(size, TypeOfUnit.Text, SideService.Text, WidthU.Text, HeightU.Text,
                //    "1500", mfthikness, typeofframe, frameoffcet, paneltype, m1, m2, roofType));
                // );TypeOfUnit.ToolTip.ToString()
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                //  sw.Logger.Log(LogLevel.Error, string.Format("Ошибка во время генерации блока {0}, время - {1}", DateTime.Now), exception);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void HeightU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        private void WidthU_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        private void TypeOfFrame_Copy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var typeOfFrameCopyValue =
               TypeOfFrame.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            if (FrameOffsetLabel == null || FrameOffset == null)
            {
                return;
            }
            if (typeOfFrameCopyValue != "3")
            {
                FrameOffsetLabel.Visibility = Visibility.Collapsed;
                FrameOffset.Visibility = Visibility.Collapsed;
            }
            else
            {
                FrameOffset.IsReadOnly = false;
                FrameOffsetLabel.Visibility = Visibility.Visible;
                FrameOffset.Visibility = Visibility.Visible;
            }
        }

        private void Thikness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Thikness == null || MaterialMontageFrame == null)
            {
                return;
            }
            switch (Thikness.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "2":
                    MaterialMontageFrame.SelectedIndex = 0;
                    break;
                case "3":
                case "4":
                    MaterialMontageFrame.SelectedIndex = 1;
                    break;
            }
        }

        private void FrameOffset_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        private void NonstandardMontageFrame_Initialized(object sender, EventArgs e)
        {
            NonstandardMontageFrame.Unchecked += NonstandardMontageFrameOnUnchecked;
            NonstandardMontageFrame.Checked += NonstandardMontageFrameOnChecked;
        }

        private void NonstandardMontageFrameOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Thikness == null || MaterialMontageFrame == null)
            {
                return;
            }
            switch (Thikness.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "2":
                    MaterialMontageFrame.SelectedIndex = 1;
                    break;
                case "3":
                case "4":
                    MaterialMontageFrame.SelectedIndex = 0;
                    break;
            }
        }

        private void NonstandardMontageFrameOnUnchecked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (Thikness == null || MaterialMontageFrame == null)
            {
                return;
            }
            switch (Thikness.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "2":
                    MaterialMontageFrame.SelectedIndex = 0;
                    break;
                case "3":
                case "4":
                    MaterialMontageFrame.SelectedIndex = 1;
                    break;
            }
        }

        private void TypeOfUnit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(TypeOfUnit != null & ModelOfInnerLabel != null & AddTypeLabel != null)) return;
            switch (TypeOfUnit.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", ""))
            {
                case "Пустой":
                    ModelOfInnerLabel.Visibility = Visibility.Collapsed;
                    ModelOfInner.Visibility = Visibility.Collapsed;
                    AddTypeLabel.Visibility = Visibility.Collapsed;
                    AddType.Visibility = Visibility.Collapsed;
                    break;
                case "Вентилятора":
                    break;
                case "Фильтра":
                    break;
                default:
                    ModelOfInnerLabel.Visibility = Visibility.Collapsed;
                    ModelOfInner.Visibility = Visibility.Collapsed;
                    AddTypeLabel.Visibility = Visibility.Collapsed;
                    AddType.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void Lenght_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        private void LenghtRoof_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }

        private void WidthRoof_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BUILDING_Click(this, new RoutedEventArgs());
            }
        }
       

        private void RoofOfUnit50_Checked(object sender, RoutedEventArgs e)
        {
            GridRoof.Visibility = Visibility.Visible;
        }

        private void RoofOfUnit50_Unchecked(object sender, RoutedEventArgs e)
        {
            GridRoof.Visibility = Visibility.Collapsed;
        }

        private void InnerOfUnit50_Checked(object sender, RoutedEventArgs e)
        {
            InnerGrid.Visibility = Visibility.Visible;
        }

        private void InnerOfUnit50_Unchecked(object sender, RoutedEventArgs e)
        {
            InnerGrid.Visibility = Visibility.Collapsed;
        }

        private void Panel50_Checked(object sender, RoutedEventArgs e)
        {
            PanelGrid.Visibility = Visibility.Visible;
        }

        private void Panel50_Unchecked(object sender, RoutedEventArgs e)
        {
            PanelGrid.Visibility = Visibility.Collapsed;
        }

        private void MontageFrame50_Checked(object sender, RoutedEventArgs e)
        {
            GridMontageFrame.Visibility = Visibility.Visible;
        }

        private void MontageFrame50_Unchecked(object sender, RoutedEventArgs e)
        {
            GridMontageFrame.Visibility = Visibility.Collapsed;
        }

    }
}
