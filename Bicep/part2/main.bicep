targetScope = 'resourceGroup'

@description('The name of the account without any prefixes and special characters or whitespaces.')
@maxLength(10)
param accountName string

@description('Name of the blob container to deploy.')
@maxLength(30)
param blobContainerNames array

@description('The SKU for the storage account.')
@allowed([
  'Premium_LRS'
  'Standard_LRS'
])
param sku string = 'Premium_LRS'

@description('The access tier for the account.')
@allowed([
  'Hot'
  'Cool'
])
param accessTier string = 'Hot'

var cleanedContainerNames = [for x in blobContainerNames: toLower(x)]

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: 'stodd${accountName}'
  location: resourceGroup().location
  sku: {
    name: sku
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2' // we want to have this set to TLS 1.2 in any account
    accessTier: accessTier
  }
}

resource blobContainers 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-06-01' = [for containerName in cleanedContainerNames: {
  name: '${storageAccount.name}/default/${containerName}'
  dependsOn: [
    storageAccount
  ]
  properties: {
    publicAccess: 'None'
  }
}]

output accountName string = storageAccount.name
output deployedContainers int = length(cleanedContainerNames)
output primaryKey string = listKeys(storageAccount.id, '2021-06-01').keys[0].value
