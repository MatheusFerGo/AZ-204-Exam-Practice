using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Scripts;

namespace Azure_204.Exercises.Storage
{
    public class Q40_Cosmos_DbPreTriggerTests
    {
        [Fact]
        public void Should_Configure_And_Invoke_PreTrigger_For_Orders()
        {
            // Arrange
            string triggerId = "orderTipTrigger";

            // 1. The JavaScript logic executed server-side by Cosmos DB engine.
            // Notice the correc usage of getContext().getRequest() to intercept the incoming payload.
            string preTriggerJavaScript = @"
                function orderTip() {
                    var context = getContext();
                    var request = context.getRequest();
                    var document = request.getBody();

                    if (!document.hasOwnProperty('OrderTip')){
                        document['OrderTip'] = 0;
                    }

                    request.setBody(document);
            }";

            // 2. Define the trigger metadata for the Cosmos DB .Net SDK
            TriggerProperties triggerProperties = new TriggerProperties
            {
                Id = triggerId,
                Body = preTriggerJavaScript,
                TriggerOperation = TriggerOperation.Create,
                TriggerType = TriggerType.Pre // Explicitly mapping as a Pre-Trigger
            };

            // Act
            // In a production environment, you would first register the script:
            // await container.Scripts.CreateTriggerAsync(triggerProperties);

            // 3. CRITICAL: Cosmos DB triggers do NOT fire automatically.
            // The C# code must explicitly request the trigger execution during the item creation
            var requestOptions = new ItemRequestOptions
            {
               PreTriggers = new List<string> { triggerId }
            };

            // Simulated creation call:
            // await container.CreateItemAsync(newOrder, new PartitionKey(newOrder.StoreId), requestOptions);

            // Assert
            // Validating the architecture setup
            Assert.Equal(TriggerType.Pre, triggerProperties.TriggerType);
            Assert.Contains(triggerId, requestOptions.PreTriggers);
            Assert.Contains("context.getRequest()", triggerProperties.Body);
        }
    }
}
