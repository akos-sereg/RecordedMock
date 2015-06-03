using RecordedMock.Client.Model;
using RecordedMock.ObjectBrowser.Properties;
using RecordedMock.ObjectBrowser.Resend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RecordedMock.ObjectBrowser.Model
{
    public class HttpProcessingTestCase : INotifyPropertyChanged
    {
        public HttpProcessingModel RecordedProcessing { get; set; }

        public HttpResponseModel TestResponse { get; set; }

        private bool? successful;
        public bool? Successful
        {
            get
            {
                return this.successful;
            }
            set
            {
                this.successful = value;
                this.IsRunningTest = false;

                this.OnPropertyChange("Successful");
                this.OnPropertyChange("TestStatus");

                if (this.successful == true)
                {
                    this.Icon = this.ToBitmapImage((Bitmap)Resources.success);
                }
                else if (this.successful == false)
                {
                    this.Icon = this.ToBitmapImage((Bitmap)Resources.failed);
                }
            }
        }

        private bool isRunningTest;
        public bool IsRunningTest
        {
            get
            {
                return this.isRunningTest;
            }
            set
            {
                this.isRunningTest = value;
                this.OnPropertyChange("TestStatus");
            }
        }

        public string TestStatus
        {
            get
            {
                return (this.Successful.HasValue ? (this.Successful == true ? "Passed" : "Failed") : (this.IsRunningTest ? "Running ..." : string.Empty));
            }
        }

        private BitmapImage icon;
        public BitmapImage Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = value;
                this.OnPropertyChange("Icon");
            }
        }

        public async Task Run()
        {
            this.IsRunningTest = true;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(new RequestBuilder(this.RecordedProcessing.Request).Build());
            this.TestResponse = new HttpResponseModel(response);

            this.Successful = new HttpProcessingValidator(response, this.RecordedProcessing.Response).Validate();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Image helper

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }

        #endregion
    }
}
