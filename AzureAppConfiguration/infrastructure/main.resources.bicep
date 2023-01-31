targetScope = 'resourceGroup'

param location string

@description('The array of AAD object ids for secrets reader access to the Key Vault.')
param readerIds array

@description('The array of AAD object ids for secrets creator access to the Key Vault.')
param creatorIds array

var readerPolicies = [for id in readerIds: {
  objectId: id
  permissions: {
    secrets: [
      'get'
      'list'
    ]
  }
  tenantId: subscription().tenantId
}]

var creatorPolicies = [for id in creatorIds: {
  objectId: id
  permissions: {
    secrets: [
      'all'
    ]
  }
  tenantId: subscription().tenantId
}]

var policies = union(readerPolicies, creatorPolicies)

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2022-05-01' = {
  name: 'acfg-cf-appconfig-demo'
  location: location
  sku: {
    name: 'free'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' = {
  name: 'akv-cf-appconfig-demo'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: policies
    enableRbacAuthorization: true
    enabledForDeployment: true
    enabledForTemplateDeployment: true
    enabledForDiskEncryption: false
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
  }
}
