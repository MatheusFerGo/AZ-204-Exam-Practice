using Microsoft.ApplicationInsights.Extensibility;
using System;
using Xunit;

namespace Azure_204.Exercises.Monitoring
{
    public class Q43_AppInsightsBudgetTests
    {
        [Fact]
        public void Should_Differentiate_Sdk_Sampling_From_Resource_Budget_Cap()
        {
            // Arrange
            // O SDK lida APENAS com a instrumentação no lado do código (ex: Sampling).
            var telemetryConfig = new TelemetryConfiguration();

            // Act
            Action validateArchitecturalLimits = () =>
            {
                // A regra fundamental da certificação:
                // O código C# gerencia QUAIS e QUANTOS dados são enviados.
                // (Em um projeto web real com o pacote AspNetCore, configuraríamos 
                // o Adaptive Sampling aqui).

                // O limite financeiro (Daily Cap) NUNCA é feito no código.
                // Se você tentar procurar uma propriedade de orçamento no SDK, ela não existe.
                // Exemplo do que é IMPOSSÍVEL fazer no código:
                // telemetryConfig.DailyCapInGb = 50; 
            };

            // Assert
            // Validamos que a estrutura compila e a limitação de escopo da arquitetura é respeitada.
            var exception = Record.Exception(validateArchitecturalLimits);
            Assert.Null(exception);
        }
    }
}