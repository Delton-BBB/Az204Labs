$userAssignedManagedIdentity = "deltonMI"
$resourceGroupName = "delton-rg"
$resourceGroup = "AzureAutomationRG"

# Connect to Azure with user-assigned managed identity
$azureContext = (Connect-AzAccount -Identity).context
$identity = Get-AzUserAssignedIdentity -ResourceGroupName $resourceGroup `
    -Name $userAssignedManagedIdentity `
    -DefaultProfile $azureContext
$azureContext = (Connect-AzAccount -Identity -AccountId $identity.ClientId).context
# set and store context
$azureContext = Set-AzContext -SubscriptionName $azureContext.Subscription `
    -DefaultProfile $azureContext
$azureContext


Remove-AzResourceGroup -Name $resourceGroupName -Force