using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Azure.Devices;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace App1
{

    public sealed partial class MainPage : Page
    {
        const string connectionString = "HostName=ProjectYsIoTHub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=koDZxijqb/4/COCRlSFc2V6J869NVh1gl4JIP/0atPo=";
        public MainPage()
        {
            this.InitializeComponent();
            Task.Run(
                    async () => {
                        while (true)
                        {
                            var message = await AzureIoTHub.ReceiveCloudToDeviceMessageAsync();
                            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High, () => {
                                test.Text += Environment.NewLine + message;
                            });
                        }
                    }
                    );
        }
        private static async Task SendCloudToDeviceMessageAsync()
        {
            var serviceClient = ServiceClient.CreateFromConnectionString(connectionString, TransportType.Amqp);
            var message = new Message(System.Text.Encoding.ASCII.GetBytes("finally"));
            await serviceClient.SendAsync("HeyHiHello", message);
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await SendCloudToDeviceMessageAsync();
        }
    }
}
