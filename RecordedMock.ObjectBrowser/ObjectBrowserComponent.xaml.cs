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
        private string TestCasesSourceFile { get; set; }

        private List<HttpProcessingTestCase> TestCases { get; set; }

        public bool IsObjectListLoaded { get; set; }

        public ObjectBrowserComponent()
        {
            InitializeComponent();
            this.TestCases = new List<HttpProcessingTestCase>();
        }

        public void Load(string fileName)
        {
            string fileContent = File.ReadAllText(fileName);
            string serializedObjects = fileContent;
            if (!fileContent.StartsWith("["))
            {
                serializedObjects = string.Format("[ {0} ]", File.ReadAllText(fileName));
            }

            // Try parsing file as HttpProcessingModel list
            List<HttpProcessingModel> requests = null;
            try
            {
                requests = JsonConvert.DeserializeObject<List<HttpProcessingModel>>(serializedObjects);
            }
            catch { }

            // Try parsing file as InvocationModel list
            List<InvocationModel> invocations = null;
            try
            {
                invocations = JsonConvert.DeserializeObject<List<InvocationModel>>(serializedObjects);
            }
            catch { }

            if (requests != null && requests.Count > 0 && requests.First().Type == typeof(HttpProcessingModel).ToString())
            {
                List<HttpProcessingTestCase> testCases = new List<HttpProcessingTestCase>();
                requests.ForEach(x => this.TestCases.Add(new HttpProcessingTestCase(x)));
                this.requestGrid.ItemsSource = TestCases;
                this.invocationsTab.IsEnabled = false;
                this.requestsTab.IsSelected = true;
                this.TestCasesSourceFile = fileName;
                this.UpdateStatusLabel();
            }
            else if (invocations != null && invocations.Count > 0 && invocations.First().Type == typeof(InvocationModel).ToString())
            {
                this.invocationGrid.ItemsSource = invocations;
                this.requestsTab.IsEnabled = false;
                this.invocationsTab.IsSelected = true;
            }
            else
            {
                MessageBox.Show("Unable to parse object file", "Parse object file", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        #region Request Grid - Context menu items

        private void ResendAll_Clicked(object sender, RoutedEventArgs e)
        {
            foreach (HttpProcessingTestCase testCase in this.requestGrid.Items)
            {
                testCase.Run(); // fire and forget
            }
        }

        private async void Resend_Clicked(object sender, RoutedEventArgs e)
        {
            HttpProcessingTestCase selectedTestCase = (HttpProcessingTestCase)this.requestGrid.SelectedItem;
            await selectedTestCase.Run();
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (this.requestGrid.SelectedItems.Count == 0)
            {
                return;
            }

            foreach (HttpProcessingTestCase selectedItem in this.requestGrid.SelectedItems) 
            {
                this.TestCases.Remove(selectedItem);
            }

            this.requestGrid.ItemsSource = null;
            this.requestGrid.ItemsSource = TestCases;
            this.UpdateStatusLabel();
        }

        private void Sync_Clicked(object sender, RoutedEventArgs e)
        {
            string backupFilePath = string.Format("{0}.backup", this.TestCasesSourceFile);

            try
            {
                File.Copy(this.TestCasesSourceFile, backupFilePath);

                List<HttpProcessingModel> requests = new List<HttpProcessingModel>();
                this.TestCases.ForEach(x => requests.Add(x.RecordedProcessing));

                string serializedList = JsonConvert.SerializeObject(requests);
                if (serializedList.Length > 0)
                {
                    serializedList = serializedList.Substring(1, serializedList.Length - 2);
                }

                File.Delete(this.TestCasesSourceFile);
                File.AppendAllText(this.TestCasesSourceFile, serializedList);

                MessageBox.Show(string.Format("Sync completed, {0} requests written into file {1}.", requests.Count, this.TestCasesSourceFile), "Sync to file", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception error)
            {
                MessageBox.Show(string.Format("Could not sync requests to file: {0}.", error.Message), "Sync to file", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (File.Exists(backupFilePath)) 
                {
                    File.Copy(backupFilePath, this.TestCasesSourceFile);
                }
            }
            finally
            {
                if (File.Exists(backupFilePath))
                {
                    File.Delete(backupFilePath);
                }
            }
        }

        #endregion

        private void UpdateStatusLabel()
        {
            this.requestGridStatusLabel.Content = string.Format("Rows: {0}", this.TestCases != null ? this.TestCases.Count.ToString() : "not loaded");
        }
    }
}
