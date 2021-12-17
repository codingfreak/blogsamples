
targetScope = 'subscription'

resource group 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-sometest'
  location: 'westeurope'
  tags: {
    purpose: 'demo'
  }
}

module storage 'storage.bicep' = {
  scope: group  
  name: 'storage'
}
