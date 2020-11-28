//  ---------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// 
//  The MIT License (MIT)
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.
//  ---------------------------------------------------------------------------------

using HueLibrary;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Radios;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace HueApp
{
    /// <summary>
    /// The main page for the Hue app controls.
    /// </summary>
    internal sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private Bridge _bridge;
        private ObservableCollection<Light> _lights;
        private ObservableCollection<Light> Lights
        {
            get { return _lights; }
            set
            {
                if (_lights != value)
                {
                    _lights = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lights)));
                }
            }
        }
        private IBackgroundTaskRegistration _taskRegistration;
        private BluetoothLEAdvertisementWatcherTrigger _trigger;
        private const string _taskName = "HueBackgroundTask";

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor for MainPage.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Fires when the page is navigated to, which occurs after the extended
        /// splash screen has finished loading all Hue resources. 
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            HuePayload args = e.Parameter as HuePayload; 
            if (null != args)
            {
                _bridge = args.Bridge;
                Lights = new ObservableCollection<Light>(args.Lights); 
            }
        }

        /// <summary>
        /// Refreshes the UI to match the actual state of the lights.
        /// </summary>
        private async void LightRefresh_Click(object sender, RoutedEventArgs e)
        {
            Lights = new ObservableCollection<Light>(await _bridge.GetLightsAsync());
        }
    }
}