using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_FileToAzureBlob.Model
{
    internal class MyBlobModel
    {
        public string BlobStorageConnectionString;
        public string BlobStorageContainerName;
        public CloudBlobClient backupBlobClient;
        public CloudBlobContainer backupContainer;
        public BlobContainerClient container;
        public MyBlobModel()
        {
            BlobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=reml;AccountKey=1UVBOD+dzMKVm99zryAK2Q+QpCrCC7HyAootXMkoa9nR9NHwNQJeIKbwYlcOVealsGEddqmDjitr+AStk0zfEQ==;EndpointSuffix=core.windows.net";
            BlobStorageContainerName = "$web";
            backupBlobClient = CloudStorageAccount.Parse(BlobStorageConnectionString).CreateCloudBlobClient();
            backupContainer = backupBlobClient.GetContainerReference(BlobStorageContainerName);
            container = new BlobContainerClient(BlobStorageConnectionString, BlobStorageContainerName);
        }
    }
}
