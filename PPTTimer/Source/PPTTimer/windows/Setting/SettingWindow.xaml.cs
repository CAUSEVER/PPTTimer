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

namespace PPTTimer.windows.Setting
{
    public partial class SettingWindow : Window
    {
        public delegate void SendMessage(string defaulttime, string defaultwarntime);
        public SendMessage sendMessage;

        public SettingWindow(MainWindow mainwindow)
        {
            InitializeComponent();
            countertime.Text = (mainwindow.defaulttime).ToString();
            questiontime.Text=(mainwindow.defaultwarntime).ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sendMessage(countertime.Text, questiontime.Text);
        }
    }
}
