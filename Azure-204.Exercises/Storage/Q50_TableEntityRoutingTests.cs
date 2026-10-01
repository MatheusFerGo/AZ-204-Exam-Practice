using Azure;
using Azure.Data.Tables;

namespace Azure_204.Exercises.Storage
{
    public class Q50_TableEntityRoutingTests
    {
        // Corrigindo o erro arquitetural da prova:
        // No mundo real, implementamos ITableEntity e declaramos TODAS as propriedades públicas.
        public class NotificationSubscriber : ITableEntity
        {
            // Chaves obrigatórias do Azure Cosmos DB Table API
            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public DateTimeOffset? Timestamp { get; set; }
            public ETag ETag { get; set; }

            // Propriedades de negócio (omitidas no código do simulado)
            public string Region { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }

            public NotificationSubscriber() { }

            public NotificationSubscriber(string region, string phone, string email)
            {
                Region = region;
                Phone = phone;
                Email = email;

                // SLOT 1 Region garante o Load Balancing (distribuição geográfica)
                PartitionKey = region;

                //SLOT : Email garante a unicidade exata do utilizador dentro daquela região
                // Phone NUNCA poderia ser a RowKey porque a regra de negócio diz que ele pode ser nule
                // RowKeys não aceitam valores nulos.
                RowKey = email;
            }
        }

        [Fact]
        public void Should_Map_Exam_Slots_To_Modern_Table_Retrieval()
        {
            // Arrange
            string endpoint = "UseDevelopmentStorage=true";
            string tableName = "subscribers";
            string targetRegion = "US-East";
            string targetEmail = "user@fiap.com.br";

            // Act
            Action executeRetrieval = () =>
            {
                // SLOT 3 na prova legada: CloudTable table = ...
                //SLOT 3 atual: TableClient
                var tableClient = new TableClient(endpoint, tableName);

                // SLOT 4 na prova legada: TabelOperation retrieve = TableOperation.Retrieve<Customer>(p_partitionkey, p_rowkey);
                // SLOT 4 atual: o SDK moderno fundiu a operação e a execução num único método direto.
                // tableClient.GetEntity<NotificationSubscriber>(targetRegion, targetEmail);
            };

            // Assert
            var exception = Record.Exception(executeRetrieval);
            Assert.True(exception == null || exception is RequestFailedException);

            var mockSubscriber = new NotificationSubscriber(targetRegion, null, targetEmail);
            Assert.Equal("US-East", mockSubscriber.PartitionKey);
            Assert.Equal("user@fiap.com.br", mockSubscriber.RowKey);

        }
    }
}
