using Microsoft.Win32;
using Newtonsoft.Json;
using RecordedMock.Client.Model;
using RecordedMock.ObjectBrowser.Model;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace RecordedMock.ObjectBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                objectFilePath.Text = openFileDialog.FileName;

                string serializedObjects = string.Format("[ {0} ]", File.ReadAllText(openFileDialog.FileName));

                List<HttpRequestModel> requests = JsonConvert.DeserializeObject<List<HttpRequestModel>>(serializedObjects);
                List<InvocationModel> invocations = JsonConvert.DeserializeObject<List<InvocationModel>>(serializedObjects);

                if (requests.Count > 0 && requests.First().Type == typeof(HttpRequestModel).ToString()) 
                {
                    this.objectGrid.ItemsSource = requests;
                }
                else if (invocations.Count > 0 && invocations.First().Type == typeof(InvocationModel).ToString()) 
                {
                    this.invocationGrid.ItemsSource = invocations;
                }
            }
        }

        private void objectGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ObjectNode> nodes = new List<ObjectNode>();
            nodes.Add(new ObjectNode("Object", this.objectGrid.SelectedItem));
            this.objectTreeView.ItemsSource = nodes;
        }

        private void invocationGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ObjectNode> nodes = new List<ObjectNode>();
            nodes.Add(new ObjectNode("Object", this.invocationGrid.SelectedItem));
            this.invocationTreeView.ItemsSource = nodes;
        }
    }
}
