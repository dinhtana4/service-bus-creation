using Azure.Messaging.ServiceBus.Administration;

Console.WriteLine("Service Bus - Queue Creation");

const string connectionString = "Service_Bus_Connection_String";

// Change your employee code here
var codes = new string[] { "0279" };

var features = new string[]
{
    "exportwp",
    "reportcreate"
};

var adminClient = new ServiceBusAdministrationClient(connectionString);
var queuePagable = adminClient.GetQueuesAsync();
var queues = new List<string>();

var number = 1;

var queuesDic = queues.ToHashSet();

foreach (var code in codes)
{
    foreach (var feature in features)
    {
        var queue = $"ms_{code}_{feature}";

        var existing = queuesDic.Contains(queue);

        if (!existing)
        {
            try
            {
                var exising2 = await adminClient.QueueExistsAsync(queue);

                if (!exising2)
                {
                    await adminClient.CreateQueueAsync(queue);

                    queuesDic.Add(queue);

                    Console.WriteLine($"creating {queue} - number: {number}");
                    number++;
                }
                else
                {
                    Console.WriteLine($"IGNORE {queue} - number: {number}");
                }    
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            Console.WriteLine($"IGNORE {queue} - number: {number}");
        }

        if (number % 5 == 0)
        {
            await Task.Delay(5 * 1000);
        }
    }
}

Console.WriteLine("==================End!========================");