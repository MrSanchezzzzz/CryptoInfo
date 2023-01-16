using CryptoInfo.Models;
using CryptoInfo.Utils;
using CryptoInfo.Views;

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


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
        private int currentRow;
       
        AssetOverviewModel.AssetOverview[] assets;
        string searchPattern = String.Empty;
        private async void LoadOverviews()
        {
            assets = await (new AssetOverviewModel()).GetAssetOverviews();
            Console.Error.WriteLine($"Loaded {assets.Length} models");

            CreateRows();
        }

        private void CreateRows()
        {
            if (assets.Length==0)
            {
                MessageBox.Show("Can't load any crypto info!","Error!",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            foreach (AssetOverview asset in assets)
            {
                if (searchPattern != String.Empty)
                {
                    if (!asset.name.ToLower().Contains(searchPattern) && 
                        !asset.asset_id.ToLower().Contains(searchPattern))
                        continue;
                }
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.ColumnDefinitions[0].MinWidth = 150;
                grid.ColumnDefinitions[1].Width = GridLength.Auto;
                grid.ColumnDefinitions[2].Width = new GridLength(50);

                RowDefinition rowDefinition = new RowDefinition();
                rowDefinition.Height = new GridLength(40);
                
                AssetsGrid.RowDefinitions.Add(rowDefinition);

                Label lb1 = new Label();
                lb1.Content = asset.name==String.Empty?asset.asset_id:$"{asset.name} ({asset.asset_id})";
                lb1.SetValue(Grid.ColumnProperty, 0);
                grid.Children.Add(lb1);

                Image image = new Image();
                image.Source = new BitmapImage(new Uri(@"/Images/next.png", UriKind.Relative));
                image.SetValue(Grid.ColumnProperty, 2);
                image.Height = image.Width = 15;
                image.VerticalAlignment = VerticalAlignment.Center;
                image.MouseLeftButtonUp += delegate (object sender, MouseButtonEventArgs e)
                {
                    ShowDetails();
                };

                grid.Children.Add(image);

                Border border= new Border();
                border.BorderThickness = new Thickness(1);
                border.BorderBrush = Brushes.Black;
                border.CornerRadius = new CornerRadius(5);
                border.Child = grid;
                border.SetValue(Grid.RowProperty, AssetsGrid.RowDefinitions.Count - 1);
                border.Margin = new Thickness(0, 0, 0, 5);
                border.Tag = asset.asset_id;

                border.MouseEnter += delegate(object sender,MouseEventArgs e)
                {
                    Border b = (Border)sender;
                    BrushAnimation animation = new BrushAnimation()
                    {
                        From = Brushes.Transparent,
                        To = Brushes.LightSteelBlue,
                        Duration = TimeSpan.FromSeconds(0.125)
                    };
                    b.BeginAnimation(Border.BackgroundProperty, animation);
                };
                border.MouseLeave += delegate (object sender, MouseEventArgs e)
                {
                    Border b = (Border)sender;
                    BrushAnimation animation = new BrushAnimation()
                    {
                        From = Brushes.LightSteelBlue,
                        To = Brushes.Transparent,
                        Duration = TimeSpan.FromSeconds(0.0625)
                    };
                    b.BeginAnimation(Border.BackgroundProperty, animation);
                };
                border.MouseLeftButtonDown += delegate (object sender, MouseButtonEventArgs e)
                {
                    Border b = (Border)sender;
                    currentRow = Array.IndexOf(assets, assets.Where(a => a.asset_id == b.Tag.ToString()).First());
                    if (e.ClickCount == 2)
                    {
                        ShowDetails();
                        
                    }
                    
                };
                AssetsGrid.Children.Add(border);
            }

        }
        private void ClearRows()
        {
            AssetsGrid.RowDefinitions.Clear();
            AssetsGrid.Children.Clear();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchPattern = ((TextBox)sender).Text.ToLower();
            ClearRows();
            CreateRows();
        }
        private void ShowDetails()
        {
            AssetOverview assetOverview = assets[currentRow];
            (new AssetDetailsWindow(assetOverview.asset_id)).Show();
        }
    }
}
