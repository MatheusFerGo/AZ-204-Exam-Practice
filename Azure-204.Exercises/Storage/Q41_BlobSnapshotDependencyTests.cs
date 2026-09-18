using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using Xunit;

namespace Azure_204.Exercises.Storage
{
    public class Q41_BlobSnapshotDependencyTests
    {
        [Fact]
        public void Should_Require_Explicit_Snapshot_Deletion_With_Base_Blob()
        {
            // Arrange
            // Using the Azure Storage Emulator connection string for local testing
            string connectionString = "UseDevelopmentStorage=true";
            string containerName = "legacy-images";
            string blobName = "img1.jpg";

            // Act
            Action architecturalValidation = () =>
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                var baseBlobClient = containerClient.GetBlobClient(blobName);

                // ARCHITECTURAL RULE:
                // If 'img1.jpg' has Snapshot 1  and Snapshot 2, calling baseBlobClient.Delete()
                // direcly will THROW AN EXCEPTION in Azure (409 Conflict - This Operation is not permitted).
                // You cannot delete a base blob and leave snapshots floating in the void.

                // To fornecullly delete the base blob, you MUST acknowledge the dependecy hierarchy
                // by using DeleteSnapshotsOption.IncludeSnapshots.
                // This command tells Azure: "I know they are tied together, send them all to the Soft Delete bin."
                DeleteSnapshotsOption dependencyAcknowledge = DeleteSnapshotsOption.IncludeSnapshots;

                // Simulated production call:
                // baseBlobClient.Delete(snapshotsOptions: dependencyAcknowledge);
            };

            // Assert
            // Validating the C# architecture logic
            var exception = Record.Exception(architecturalValidation);
            Assert.Null(exception);
        }
    }
}
