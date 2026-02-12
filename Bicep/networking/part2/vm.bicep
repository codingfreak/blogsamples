targetScope = 'resourceGroup'

@description('The name of the project.')
param projectName string

@description('The Azure region where to deploy to.')
param location string

@description('The name of the stage.')
param stage string

@description('The resource id of the network interface card.')
param nicId string

@description('The user name of the VM admin.')
param adminUsername string

@secure()
@description('The password of the VM admin.')
param adminPassword string

var timeZone string = 'W. Europe Standard Time'

resource vm 'Microsoft.Compute/virtualMachines@2025-04-01' = {
  name: 'vm-${toLower(projectName)}-one-${toLower(stage)}'
  location: location
  properties: {
    hardwareProfile: {
      vmSize: 'Standard_D4s_v3'
    }
    storageProfile: {
      imageReference: {
        publisher: 'MicrosoftWindowsDesktop'
        offer: 'windows-11'
        sku: 'win11-25h2-pro'
        version: 'latest'
      }
      osDisk: {
        osType: 'Windows'
        createOption: 'FromImage'
        caching: 'ReadWrite'
        name: 'disc-os-${toLower(projectName)}-one-${toLower(stage)}'
      }
    }
    osProfile: {
      windowsConfiguration: {
        provisionVMAgent: true
        enableAutomaticUpdates: false
        timeZone: timeZone
      }
      adminUsername: adminUsername
      adminPassword: adminPassword
      computerName: 'cfone'
    }
    networkProfile: {
      networkInterfaces: [
        {
          id: nicId
        }
      ]
    }
  }
}

resource autoShutdown 'Microsoft.DevTestLab/schedules@2018-09-15' = {
  name: 'shutdown-computevm-${vm.name}'
  location: location
  properties: {
    status: 'Enabled'
    taskType: 'ComputeVmShutdownTask'
    dailyRecurrence: {
      time: '1900'
    }
    timeZoneId: timeZone
    notificationSettings: {
      status: 'Disabled'
    }
    targetResourceId: vm.id
  }
}
