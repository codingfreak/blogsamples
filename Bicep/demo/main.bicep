targetScope = 'resourceGroup'

// PARAMS
@minLength(3)
@maxLength(10)
@description('The name of the project to be added to the resource names.')
param projectName string

@minLength(3)
@maxLength(10)
@allowed([
  'int'
  'test'
  'prod'
])
@description('The name of the stage to be added to the resource names.')
param stageName string

@minLength(3)
@maxLength(10)
@description('The name of the ARM location')
param location string 

@minLength(3)
@maxLength(100)
@description('The unique identifier of the Log Analytics Workspace subscription.')
param logAnalyticsWorkspaceSubscriptionId string

@minLength(3)
@maxLength(100)
@description('The name of the resource group the Log Analytics Workspace lives in.')
param logAnalyticsWorkspaceResourceGroupName string

@minLength(3)
@maxLength(100)
@description('The unique name of the Log Analytics Workspace')
param logAnalyticsWorkspaceName string

var options = {
  location: location
  suffix: 'dd-${toLower(projectName)}-${toLower(stageName)}'
  stageName: toLower(stageName)
  logAnalyticsId: resourceId(logAnalyticsWorkspaceSubscriptionId, logAnalyticsWorkspaceResourceGroupName, 'Microsoft.OperationalInsights/workspaces', logAnalyticsWorkspaceName)
  defaultDiagnosticName: 'diag-default'
  defaultActionGroup: {
    name: 'agrp-dd-default'
    subscriptionId: '42261c8d-1cdb-41e1-9934-5011fd19bc97'
    resourceGroup: 'rg-monitoring'
  }
}

module storage 'modules/Microsoft.Storage/storageAccounts.bicep' = {
  name: 'storage'
  params: {
    options: options
  }
}
