from azure.identity import DefaultAzureCredential
from azure.storage.blob import BlobServiceClient, BlobClient, ContainerClient

azureStorageAccountName = "deltonstorageaccount"
azureStorageAccountConnString = ""
azureContainerName = ""
azureBlobName = ""


def getBlobs(containerName):
    azureContainerName = containerName
    containerClient = BlobServiceClient.from_connection_string(azureStorageAccountConnString).get_container_client(azureContainerName)
    
    if not containerClient.exists():
        containerClient.create_container()
        print(f"Container {containerClient} Created")
    
    blobs = containerClient.list_blob_names()
    
    for blob in blobs:
        print(f"\n-> {blob}")
    print("\n")



def uploadBlob(filePath,containerName,blobName):
    azureContainerName = containerName
    azureBlobName = blobName
    containerClient = BlobServiceClient.from_connection_string(azureStorageAccountConnString).get_container_client(azureContainerName)

    if not containerClient.exists():
        containerClient.create_container()
        print(f"Container {containerClient} Created")
        
    with open(filePath, "rb") as data:
        blobClient = containerClient.get_blob_client(azureBlobName)
        blobClient.upload_blob(data)
        print(f"Blob Uploaded from {filePath} to {containerName}/{azureBlobName}")

def deleteBlob(containerName, blobName):
    azureContainerName = containerName
    azureBlobName = blobName
    blobClient = BlobServiceClient.from_connection_string(azureStorageAccountConnString).get_container_client(azureContainerName).get_blob_client(azureBlobName)
    blobClient.delete_blob()
    print(f"{azureBlobName} blob Deleted")
    
    
    
    
try:

    filePath = "./test.jpg"

    # uploadBlob(filePath,"test","test2")
    getBlobs("test")
    # deleteBlob("test","test")

except Exception as ex:
    print('Exception:')
    print(ex)