targetScope = 'subscription'

param location string

@description('The array of AAD object ids for secrets reader access to the Key Vault.')
param vaultReaderIds array

@description('The array of AAD object ids for secrets creator access to the Key Vault.')
param vaultCreatorIds array

resource group 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rg-appconfig-demo'
  location: location
}

module resources 'main.resources.bicep' = {
  scope: group
  name: 'resources'
  params: {
    location: location
    creatorIds: vaultCreatorIds
    readerIds: vaultReaderIds
  }
}
