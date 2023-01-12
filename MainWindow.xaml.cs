using CryptoInfo.Models;
using CryptoInfo.Views;

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
using System.Windows.Navigation;
using System.Windows.Shapes;

using static CryptoInfo.Models.AssetOverviewModel;

namespace CryptoInfo
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadOverviews();

        }

        AssetOverviewModel.AssetOverview[] models;
        private async void LoadOverviews()
        {
            models = await (new AssetOverviewModel()).GetAssetOverviews();
            Console.Error.WriteLine($"Loaded {models.Length} models");
        }

        private void CreateRows()
        {

        }
    }
}
