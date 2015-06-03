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
        private List<HttpProcessingTestCase> TestCases { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.TestCases = new List<HttpProcessingTestCase>();
        }

        private void Open_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                objectFilePath.Text = openFileDialog.FileName;

                string serializedObjects = string.Format("[ {0} ]", File.ReadAllText(openFileDialog.FileName));

                List<HttpProcessingModel> requests = JsonConvert.DeserializeObject<List<HttpProcessingModel>>(serializedObjects);
                List<InvocationModel> invocations = JsonConvert.DeserializeObject<List<InvocationModel>>(serializedObjects);

                if (requests.Count > 0 && requests.First().Type == typeof(HttpProcessingModel).ToString()) 
                {
                    List<HttpProcessingTestCase> testCases = new List<HttpProcessingTestCase>();
                    requests.ForEach(x => this.TestCases.Add(new HttpProcessingTestCase(x)));
                    this.requestGrid.ItemsSource = TestCases;
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
            nodes.Add(new ObjectNode("Object", this.requestGrid.SelectedItem));
            this.objectTreeView.ItemsSource = nodes;
        }

        private void invocationGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<ObjectNode> nodes = new List<ObjectNode>();
            nodes.Add(new ObjectNode("Object", this.invocationGrid.SelectedItem));
            this.invocationTreeView.ItemsSource = nodes;
        }

        private async void Resend_Clicked(object sender, RoutedEventArgs e)
        {
            HttpProcessingTestCase selectedTestCase = (HttpProcessingTestCase)this.requestGrid.SelectedItem;
            await selectedTestCase.Run();
        }
    }
}
