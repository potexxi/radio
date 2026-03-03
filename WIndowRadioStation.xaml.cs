using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Radio
{
    /// <summary>
    /// Interaktionslogik für WIndowRadioStation.xaml
    /// </summary>
    public partial class WIndowRadioStation : Window
    {
        private string volume;
        private string frequency;
        public string name {  get; private set; }
        public bool result { get; private set; }
        public WIndowRadioStation(string volume, string frequency)
        {
            InitializeComponent();
            this.volume = volume;
            this.frequency = frequency;
            LabelFrequency.Content = $"{frequency}MHz";
            LabelVolume.Content = $"{volume}%";
            this.name = "";
            this.result = false;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxStationName.Text == "")
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Der Name des Radiosender wird uebernommen!", "Warning", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (MessageBoxResult.OK == messageBoxResult)
                {
                    this.result = true;
                    this.Close();
                }
                else
                {
                    this.result = false;
                    return;
                }
            }
            this.name = TextBoxStationName.Text;
            this.result = true;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.result = false;
            this.Close();
        }
    }
}
