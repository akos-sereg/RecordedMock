﻿using RecordedMock.Client.Model;
using RecordedMock.ObjectBrowser.Exception;
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
        #region Properties
        
        /// <summary>
        /// Details of captured request and response objects.
        /// </summary>
        public HttpProcessingModel RecordedProcessing { get; set; }

        /// <summary>
        /// Details of executed (tested) request and its response.
        /// </summary>
        public HttpProcessingModel TestProcessing { get; set; }

        /// <summary>
        /// Provides derived properties based on current instance - used by Grid component.
        /// </summary>
        public Display Display { get; set; }

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

        public string ValidationError { get; set; }

        public string TestStatus
        {
            get
            {
                return (this.Successful.HasValue ? (this.Successful == true ? 
                    "Passed" : 
                    string.Format("Failed: {0}", this.ValidationError)) 
                        : (this.IsRunningTest ? "Running ..." : string.Empty));
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

        #endregion

        public HttpProcessingTestCase(HttpProcessingModel recordedProcessing)
        {
            this.RecordedProcessing = recordedProcessing;
            this.Display = new Display(this);
        }

        public async Task Run()
        {
            this.IsRunningTest = true;
            this.ValidationError = null;

            HttpClient client = new HttpClient();

            // Arrange test processing context
            this.TestProcessing = new HttpProcessingModel();
            this.TestProcessing.Request = new HttpRequestModel(new RequestBuilder(this.RecordedProcessing.Request).Build());

            // Get "actual" response
            try
            {
                HttpResponseMessage response = await client.SendAsync(new RequestBuilder(this.RecordedProcessing.Request).Build());
                this.TestProcessing.Response = new HttpResponseModel(response);

                new HttpProcessingValidator(this.TestProcessing.Response, this.RecordedProcessing.Response).Validate();
                this.Successful = true;
            }
            catch (HttpProcessingValidationException error)
            {
                this.ValidationError = error.Message;
                this.Successful = false;
            }
            catch (System.Exception error)
            {
                this.ValidationError = string.Format("Unable to execute: {0}", error.Message);
                this.Successful = false;
            }
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
