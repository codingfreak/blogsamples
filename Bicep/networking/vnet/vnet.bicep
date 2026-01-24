targetScope = 'resourceGroup'

@description('The name of the project.')
param projectName string

@description('The Azure region where to deploy to.')
param location string

@description('The name of the stage.')
param stage string

@description('The address prefix for the VNET.')
param addressPrefix string

@description('The names for all subnets which should take in resources. The GatewaySubnet will be deployed additionally!')
param subnets string[]

var suffix = '${toLower(projectName)}-${toLower(stage)}'
var workloadSubnets = map(subnets, (s, i) => {
  name: s
  properties: {
    addressPrefix: cidrSubnet(addressPrefix, 24, i + 1)
  }
})
var resolvedSubnets = union(
  [
    {
      name: 'GatewaySubnet'
      properties: {
        addressPrefix: cidrSubnet(addressPrefix, 29, 0)
      }
    }
  ],
  workloadSubnets
)

resource vnet 'Microsoft.Network/virtualNetworks@2025-01-01' = {
  name: 'vnet-${suffix}'
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        addressPrefix
      ]
    }
    subnets: resolvedSubnets
  }
}

output subnets object[] = vnet.properties.subnets
