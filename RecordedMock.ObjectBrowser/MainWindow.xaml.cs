using Microsoft.Win32;
using Newtonsoft.Json;
using RecordedMock.Client.Model;
using RecordedMock.ObjectBrowser.Model;
using RecordedMock.ObjectBrowser.Resend;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
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

namespace RecordedMock.ObjectBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TabItem> _tabItems;

        public MainWindow()
        {
            InitializeComponent();

            // initialize tabItem array
            _tabItems = new List<TabItem>();

            // add a tabItem with + in header 
            //TabItem tabAdd = new TabItem();
            //tabAdd.Header = "+";
            //_tabItems.Add(tabAdd);

            // add first tab
            //this.AddTabItem();

            // bind tab control
            this.ObjectTabControl.DataContext = _tabItems;
            this.ObjectTabControl.SelectedIndex = 0;
        }

        private TabItem AddTabItem(string filePath = null)
        {
            int count = _tabItems.Count;

            // create new tab item
            TabItem tab = new TabItem();
            tab.Header = "n/a";
            tab.Name = string.Format("tab{0}", count);
            tab.HeaderTemplate = this.ObjectTabControl.FindResource("TabHeader") as DataTemplate;

            // add controls to tab item, this case I added just a textbox
            ObjectBrowserComponent objectBrowser = new ObjectBrowserComponent();
            objectBrowser.Name = "objectBrowser";
            tab.Content = objectBrowser;

            if (count == 0)
            {
                TabItem tabAdd = new TabItem();
                tabAdd.Header = "+";
                _tabItems.Add(tabAdd);
                count++;
            }

            // insert tab item right before the last (+) tab item
            _tabItems.Insert(count - 1, tab);

            return tab;
        }

        private void tabDynamic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tab = this.ObjectTabControl.SelectedItem as TabItem;

            if (tab != null && tab.Header != null)
            {
                if (tab.Header.Equals("+"))
                {
                    this.MenuItem_Click_1(null, null);
                }
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            TabItem tabItem = this._tabItems.FirstOrDefault(x => x.Header.ToString() != "x" && x.Content != null && !((ObjectBrowserComponent)x.Content).IsObjectListLoaded);
            if (tabItem == null)
            {
                this.ObjectTabControl.DataContext = null;
                tabItem = this.AddTabItem(openFileDialog.FileName);
                this.ObjectTabControl.DataContext = _tabItems;
                this.ObjectTabControl.SelectedItem = tabItem;
            }
            
            tabItem.Header = System.IO.Path.GetFileName(openFileDialog.FileName);
            
            ((ObjectBrowserComponent)tabItem.Content).Load(openFileDialog.FileName);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabName = (sender as Button).CommandParameter.ToString();

            var item = this.ObjectTabControl.Items.Cast<TabItem>().Where(i => i.Name.Equals(tabName)).SingleOrDefault();

            TabItem tab = item as TabItem;

            if (tab != null)
            {
                if (_tabItems.Count < 3)
                {
                    MessageBox.Show("Cannot remove last tab.");
                }
                else if (MessageBox.Show(string.Format("Are you sure you want to remove the tab '{0}'?", tab.Header.ToString()),
                    "Remove Tab", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    // get selected tab
                    TabItem selectedTab = this.ObjectTabControl.SelectedItem as TabItem;

                    // clear tab control binding
                    this.ObjectTabControl.DataContext = null;

                    _tabItems.Remove(tab);

                    // bind tab control
                    this.ObjectTabControl.DataContext = _tabItems;

                    // select previously selected tab. if that is removed then select first tab
                    if (selectedTab == null || selectedTab.Equals(tab))
                    {
                        selectedTab = _tabItems[0];
                    }

                    this.ObjectTabControl.SelectedItem = selectedTab;
                }
            }
        }
    }
}
