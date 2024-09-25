targetScope = 'subscription'

param location string

param name string

resource group 'Microsoft.Resources/resourceGroups@2024-03-01' = {
  name: 'rg-${name}'
  location: location
}

module resources 'resources.bicep' = {
  name: 'resources'
  scope: group
  params: {
    name: name
    location: location
  }
}
