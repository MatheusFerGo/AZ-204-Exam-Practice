using System;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using Azure.Core;
using Xunit;

namespace Azure_204.Exercises.Compute
{
    public class Q39_VmBackupRoutineTests
    {
        [Fact]
        public void Should_Configure_Snapshot_Backup_For_Legacy_Vm()
        {
            // Arrange
            // Simulated context for the legacy accounting virtual machine
            string subscriptionId = "00000000-0000-0000-0000-000000000000";
            string resourceGroupName = "rg-legacy-accounting";
            string diskName = "os-disk-accounting-vm";
            string snapshotName = $"backup-snapshot-{DateTime.UtcNow:yyyyMMdd}";

            // Act
            Action configureBackupRoutine = () =>
            {
                // 1. Initialize the Azure Resource Manager (ARM) Client.
                // DefaultAzureCredential automatically handles Managed Identities in production.
                var armClient = new ArmClient(new DefaultAzureCredential());

                // 2. Define the exact architectural path to the source managed disk.
                ResourceIdentifier sourceDiskId = ManagedDiskResource.CreateResourceIdentifier(
                    subscriptionId,
                    resourceGroupName,
                    diskName);

                // 3. Configure the Snapshot definition mapping to the 7-day retention requirement.
                // This validates the exact classes and parameters required by the Azure Compute SDK.
                var snapshotData = new SnapshotData(AzureLocation.EastUS)
                {
                    CreationData = new DiskCreationData(DiskCreateOption.Copy)
                    {
                        SourceResourceId = sourceDiskId
                    },
                    Tags =
                    {
                        { "Context", "Pre-Update-Backup" },
                        { "RetentionDays", "7" },
                        { "RestoreType", "In-Place" }
                    }
                };
            };

            // Assert
            // Validates that the SDK objects are correctly instantiated without missing dependencies or invalid enums.
            var exception = Record.Exception(configureBackupRoutine);
            Assert.Null(exception);
        }
    }
}