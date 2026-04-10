targetScope = 'subscription'

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

@description('The user name of the VM admin.')
param adminUsername string

@secure()
@description('The password of the VM admin.')
param adminPassword string

@description('Determines if a jumphost is deployed or not.')
param deployJumpHost bool = false

resource vnetGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'rg-${toLower(projectName)}-vnet-${toLower(stage)}'
  location: location
  tags: {
    purpose: 'demo'
    project: projectName
  }
}

resource vmGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'rg-${toLower(projectName)}-vm-${toLower(stage)}'
  location: location
  tags: {
    purpose: 'demo'
    project: projectName
  }
}

module vnet 'vnet.bicep' = {
  name: 'resources-vnet'
  scope: vnetGroup
  params: {
    location: location
    projectName: projectName
    stage: stage
    addressPrefix: addressPrefix
    subnets: subnets
  }
}

module vm 'vm.bicep' = {
  name: 'resources-vm'
  scope: vmGroup
  params: {
    location: location
    projectName: projectName
    stage: stage
    nicId: vnet.outputs.nicId
    adminUsername: adminUsername
    adminPassword: adminPassword
  }
}

resource jumpGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = if (deployJumpHost) {
  name: 'rg-${toLower(projectName)}-jump-${toLower(stage)}'
  location: location
  tags: {
    purpose: 'demo'
    project: projectName
  }
}

module jump 'jump.bicep' = if (deployJumpHost) {
  name: 'resources-jump'
  scope: jumpGroup
  params: {
    location: location
    projectName: projectName
    stage: stage
    nicId: vnet.outputs.nicJumpId
    adminUsername: adminUsername
    adminPassword: adminPassword
  }
}

@description('The address prefix of the VmSubnet.')
output vmPrefix string = vnet.outputs.vmPrefix

@description('The address prefix of the JumphostSubnet.')
output jumpHostPrefix string = vnet.outputs.jumpPrefix
