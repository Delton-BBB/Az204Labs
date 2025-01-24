Param
(
    [Parameter (Mandatory = $true)]
    [string] $currentStorageAccountKey,
    
    [Parameter (Mandatory = $true)]
    [string] $objectid
)



$parameters = @{
    "objectid"= $objectid
}

$userAssignedManagedIdentityName = "deltonMI"
$newResourceGroupName = "delton-rg"
$location = "uksouth"
$fileShareName = "armtemplate"
$fileName = "azuredeploy.json"
$currentResourceGroup = "AzureAutomationRG"
$currentStorageAccountName = "azureautomationstore"




# Ensures you do not inherit an AzContext in your runbook
Disable-AzContextAutosave -Scope Process

# Connect to Azure with user-assigned managed identity
$azureContext = (Connect-AzAccount -Identity).context
$identity = Get-AzUserAssignedIdentity -ResourceGroupName $currentResourceGroup `
    -Name $userAssignedManagedIdentityName `
    -DefaultProfile $azureContext
$azureContext = (Connect-AzAccount -Identity -AccountId $identity.ClientId).context
# set and store context
$azureContext = Set-AzContext -SubscriptionName $azureContext.Subscription `
    -DefaultProfile $azureContext
$azureContext

$StorageContext = New-AzStorageContext -StorageAccountName $currentStorageAccountName -StorageAccountKey $currentStorageAccountKey
$StorageContext
Get-AzStorageFileContent -ShareName $fileShareName -Context $StorageContext -path $fileName -Destination 'C:\' -Force
Dir

# Create the rg to deploy arm template

New-AzResourceGroup -Name $newResourceGroupName -Location $location -DefaultProfile $azureContext -Force
$templateFile = Join-Path -Path 'C:\' -ChildPath $fileName

New-AzResourceGroupDeployment -TemplateFile $templateFile -ResourceGroupName $newResourceGroupName -Name "PS-deployment" -TemplateParameterObject $parameters


