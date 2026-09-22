using System;
using Xunit;

namespace Azure_204.Exercises.Compute
{
    public class Q42_ContainerRegistryTaggingTests
    {
        [Fact]
        public void Should_Format_Docker_Tag_For_Azure_Container_Registry()
        {
            // Arrange
            // Simulated context from the case study requirements
            string localImageName = "bhuvanapp-image";
            string registryLoginServer = "bhuvanappregistry.azurecr.io";

            // Act
            // Simulating the local string manipulation required by a CI/CD pipeline 
            // before executing the standard 'docker tag' process to the host OS.
            // Notice that the Azure CLI (az) is NOT used for local alias creation.
            string targetAcrTag = $"{registryLoginServer}/{localImageName}";

            string simulatedCommandToHost = $"docker tag {localImageName} {targetAcrTag}";

            // Assert
            // Validating the architectural rule: the target must contain the full registry URI,
            // and the toolset must be the native Docker CLI.
            Assert.Equal("bhuvanappregistry.azurecr.io/bhuvanapp-image", targetAcrTag);
            Assert.StartsWith("docker tag", simulatedCommandToHost);

            // Explicitly preventing the conceptual error from the exam
            Assert.DoesNotContain("az tag", simulatedCommandToHost);
            Assert.DoesNotContain("acr tag", simulatedCommandToHost);
        }
    }
}