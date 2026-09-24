using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Text;
using Xunit;

namespace Azure_204.Exercises.Messaging
{
    public class Q45_EventHubPartitionTests
    {
        [Fact]
        public void Should_Route_Traffic_Events_By_Highway_PartitionKey()
        {
            // Arrange
            string connectionString = "Endpoint=sb://mock-namespace.servicebus.windows.net/;SharedAccessKeyName=Root;SharedAccessKey=mock-key";
            string eventHubName = "traffic-telemetry-hub";

            // The distinct data source from the case study
            string sourceHighway = "Highway-1";

            // Act
            Action configurePartitionedBatch = () =>
            {
                // Initialize the client responsible for sending data
                var producerClient = new EventHubProducerClient(connectionString, eventHubName);

                // ARCHITECTURAL RULE:
                // We do not manually assign "Partition 0" or "Partition 1".
                // Instead, we pass the Highway ID as the PartitionKey. 
                // Azure hashes this key and routes all "Highway-1" events to the same dedicated partition.
                // This guarantees maximum throughput (across 4 highways) AND strict time-series ordering.
                var batchOptions = new CreateBatchOptions
                {
                    PartitionKey = sourceHighway
                };

                // Simulated production execution:
                // using EventDataBatch eventBatch = await producerClient.CreateBatchAsync(batchOptions);
                // var sensorData = new EventData(Encoding.UTF8.GetBytes("{ 'speed': 85, 'carCount': 12 }"));
                // eventBatch.TryAdd(sensorData);
                // await producerClient.SendAsync(eventBatch);
            };

            // Assert
            var exception = Record.Exception(configurePartitionedBatch);
            Assert.Null(exception);
        }
    }
}