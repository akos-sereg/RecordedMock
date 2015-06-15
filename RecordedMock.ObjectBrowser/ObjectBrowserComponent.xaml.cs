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
    /// Interaction logic for ObjectBrowser.xaml
    /// </summary>
    public partial class ObjectBrowserComponent : UserControl
    {
        private List<HttpProcessingTestCase> TestCases { get; set; }

        public bool IsObjectListLoaded { get; set; }

        public ObjectBrowserComponent()
        {
            InitializeComponent();
            this.TestCases = new List<HttpProcessingTestCase>();
        }

        public void Load(string fileName)
        {
            string serializedObjects = string.Format("[ {0} ]", File.ReadAllText(fileName));

            List<HttpProcessingModel> requests = JsonConvert.DeserializeObject<List<HttpProcessingModel>>(serializedObjects);
            List<InvocationModel> invocations = JsonConvert.DeserializeObject<List<InvocationModel>>(serializedObjects);

            if (requests.Count > 0 && requests.First().Type == typeof(HttpProcessingModel).ToString())
            {
                List<HttpProcessingTestCase> testCases = new List<HttpProcessingTestCase>();
                requests.ForEach(x => this.TestCases.Add(new HttpProcessingTestCase(x)));
                this.requestGrid.ItemsSource = TestCases;
                this.invocationsTab.IsEnabled = false;
                this.requestsTab.IsSelected = true;
            }
            else if (invocations.Count > 0 && invocations.First().Type == typeof(InvocationModel).ToString())
            {
                this.invocationGrid.ItemsSource = invocations;
                this.requestsTab.IsEnabled = false;
                this.invocationsTab.IsSelected = true;
            }

            this.IsObjectListLoaded = true;
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

        private void RunAll_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (HttpProcessingTestCase testCase in this.requestGrid.Items)
            {
                testCase.Run(); // fire and forget
            }
        }
    }
}
