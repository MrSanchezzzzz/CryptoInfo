using CryptoInfo.Models;

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

namespace CryptoInfo.Views
{
    /// <summary>
    /// Логика взаимодействия для AssetDetailsWindow.xaml
    /// </summary>
    public partial class AssetDetailsWindow : Window
    {
        public AssetDetailsWindow(string assetId)
        {
            InitializeComponent();
            this._assetId=assetId;
        }
        string _assetId;
        Asset asset;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetAsset(_assetId);

            NameLabel.Content = asset.name == String.Empty ? asset.asset_id : $"{asset.name} ({asset.asset_id})";

            Change1HLabel.Content = asset.change_1h;
            Change1HLabel.Foreground = asset.change_1h > 0 ? Brushes.DarkGreen : Brushes.Maroon;

            Change24HLabel.Content = asset.change_24h;
            Change24HLabel.Foreground = asset.change_24h > 0 ? Brushes.DarkGreen : Brushes.Maroon;

            Change7DLabel.Content = asset.change_7d;
            Change7DLabel.Foreground = asset.change_7d > 0 ? Brushes.DarkGreen : Brushes.Maroon;

            PriceLabel.Content = asset.price;
        }

        private void GetAsset(string assetId="BTC")
        {
            asset = Asset.FromIdAsync(assetId).Result;
            Console.WriteLine(asset.asset_id);
            return;
        }
    }
}
