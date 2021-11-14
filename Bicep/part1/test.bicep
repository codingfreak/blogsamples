targetScope = 'subscription'

// parameters (coming from outside)
@maxLength(15)
param name string

// variables (calculated/defined here)
var resolvedName = 'rg-${toLower(name)}'

// resource definitions
resource group 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: resolvedName
  location: 'West Europe'
  tags: {
    purpose: 'demo'
    foo: 'bar'
    x: 'Y'
  }
}
