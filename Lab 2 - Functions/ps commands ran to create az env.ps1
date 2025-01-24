New-AzStorageAccount -Name deltonrg -Location uksouth
New-AzStorageAccount -ResourceGroup deltonrg -Name deltonfuncstore -Location uksouth -SkuName Standard_LRS
New-AzFunctionApp -ResourceGroupName deltonrg -Name deltonfuncapp -Location uksouth -StorageAccountName deltonfuncstore -OSType Linux -Runtime .Net -RuntimeVersion 6