using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_204.Exercises.Storage
{
    public class Q44_BlobSoftDeleteRecoveryTests
    {
        [Fact]
        public void Should_Undelete_Base_Blob_To_Recover_Soft_Deleted_Snapshots()
        {
            // Arrange
            var blobServiceClient = new BlobServiceClient("UseDevelopmentStorage=true");
            var containerClient = blobServiceClient.GetBlobContainerClient("legacy-images");

            // We instantiate the client for the Base Blob, NOT the snapshot.
            var baseBlobClient = containerClient.GetBlobClient("img1.jpg");

            // Act
            Action recoverSnapshots = () =>
            {
                // ARCHITECTURAL RULE (The exact reason Q44 is YES and Q41 is NO):
                // 1. You CANNOT CALL an Undelete method on a specifc Snapshot Client.
                // Attempting to restore a snapshot independently while the base is deleted is impossible (Q41 context).

                // 2. To get Snapshot 1 bacl (Q44 context), you MUST call undelete on the BASE BLOB.
                // This single command tells Azure: "Bring the base blob back to life AND pull all
                // soft-deleted snapshots (like Snapshot 1) out of the retention bin with it."
                
                // baseBlobClient.Undelete();
            };

            var exception = Record.Exception(recoverSnapshots);
            Assert.Null(exception);
        }
    }
}
