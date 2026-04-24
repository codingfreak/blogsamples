targetScope = 'resourceGroup'

var roleId = roleDefinitions('Monitoring Metrics Publisher').id

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: 'ai-dd-test'
  location: 'westeurope'
  kind: 'web'
  properties: {
    Application_Type: 'web'
    DisableLocalAuth: true
  }
}

resource webApp 'Microsoft.Web/sites@2025-03-01' = {
  name: 'api-dd-test'
  identity: {
    type: 'SystemAssigned'
  }
  location: 'westeurope'
  properties: {
    siteConfig: {
      appSettings: [
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'APPLICATIONINSIGHTS_AUTHENTICATION_STRING'
          value: 'Authorization=AAD'
        }
      ]
    }
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(roleId, 'webapp', 'ServicePrincipal', 'test')
  scope: appInsights
  properties: {
    roleDefinitionId: roleId
    principalId: webApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
