using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;

class AzureIoTHub
{
    private static void CreateClient()
    {
        if (deviceClient == null)
        {
            deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Mqtt);
        }
    }

    static DeviceClient deviceClient = null;
    const string deviceConnectionString = "HostName=ProjectYsIoTHub.azure-devices.net;DeviceId=HeyHiHello;SharedAccessKey=zjH76Z3cifItfY6WSWujn/Dp0V7kCIMostBpp7vWoWw=";

    public static async Task SendDeviceToCloudMessageAsync()
    {
        CreateClient();
#if WINDOWS_UWP
        var str = "{\"deviceId\":\"HeyHiHello\",\"messageId\":1,\"text\":\"Hello, Cloud from a UWP C# app!\"}";
#else
        var str = "{\"deviceId\":\"HeyHiHello\",\"messageId\":1,\"text\":\"Hello, Cloud from a C# app!\"}";
#endif
        var message = new Message(Encoding.ASCII.GetBytes(str));

        await deviceClient.SendEventAsync(message);
    }

    public static async Task<string> ReceiveCloudToDeviceMessageAsync()
    {
        CreateClient();

        while (true)
        {
            var receivedMessage = await deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                var messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await deviceClient.CompleteAsync(receivedMessage);
                return messageData;
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
