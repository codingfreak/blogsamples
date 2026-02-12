targetScope = 'resourceGroup'

import { resolveSubnetByName } from 'resolveSubnetByName.bicep'

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
    networkSecurityGroup: {
      id: nsg.id
    }
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

resource nsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${suffix}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'allow-rdp'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 100
          protocol: 'Tcp'
          destinationPortRange: '3389'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'allow-icmp'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 110
          protocol: 'Icmp'
          destinationPortRange: '*'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}

resource pipVm 'Microsoft.Network/publicIPAddresses@2025-01-01' = {
  name: 'pip-${toLower(projectName)}-one-${toLower(stage)}'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

resource pipJump 'Microsoft.Network/publicIPAddresses@2025-01-01' = {
  name: 'pip-${toLower(projectName)}-jump-${toLower(stage)}'
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    publicIPAllocationMethod: 'Static'
  }
}

resource nicVm 'Microsoft.Network/networkInterfaces@2025-01-01' = {
  name: 'nic-${suffix}'
  location: location
  properties: {
    enableAcceleratedNetworking: true
    ipConfigurations: [
      {
        name: 'default'
        properties: {
          publicIPAddress: {
            id: pipVm.id
          }
          subnet: {
            id: resolveSubnetByName(vnet.properties.subnets, 'VmSubnet').id
          }
        }
      }
    ]
    networkSecurityGroup: {
      id: nsg.id
    }
  }
}

resource nicJump 'Microsoft.Network/networkInterfaces@2025-01-01' = {
  name: 'nic-jump-${suffix}'
  location: location
  properties: {
    enableAcceleratedNetworking: true
    ipConfigurations: [
      {
        name: 'default'
        properties: {
          publicIPAddress: {
            id: pipJump.id
          }
          subnet: {
            id: resolveSubnetByName(vnet.properties.subnets, 'JumphostSubnet').id
          }
        }
      }
    ]
    networkSecurityGroup: {
      id: nsg.id
    }
  }
}

output subnets object[] = vnet.properties.subnets

output nicId string = nicVm.id

output nicJumpId string = nicJump.id
