targetScope = 'subscription'

@description('The region for the Azure resources.')
param location string

resource group 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'rg-codingfreaks-test'
  location: location
  tags: {
    purpose: 'demo'
    source: 'codingfreaks'
  }
}

module resources 'resources.bicep' = {
  name: 'main-resources'
  scope: group
}

module roleAssignments 'roleAssignments.bicep' = {
  name: 'role-assignments'
  scope: group
  params: {
    appPrincipalId: resources.outputs.appPrincipalId
  }
}

output keyVaultUri string = resources.outputs.keyVaultUri

output appConfigUri string = resources.outputs.appConfigUri
