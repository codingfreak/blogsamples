targetScope = 'resourceGroup'

var salt = substring(tenant().tenantId, 0, 10)

resource keyVault 'Microsoft.KeyVault/vaults@2024-11-01' = {
  name: 'kv-cf-sample-${salt}'
  location: resourceGroup().location
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }
    tenantId: tenant().tenantId
    enableRbacAuthorization: true
    enablePurgeProtection: false
  }
}

resource appConfig 'Microsoft.AppConfiguration/configurationStores@2024-06-01' = {
  name: 'appcs-cf-sample-${salt}'
  location: resourceGroup().location
  sku: {
    name: 'Standard'
  }
  properties: {
    enablePurgeProtection: false
  }
}

resource servicePlan 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: 'asp-cf-sample-${salt}'
  location: resourceGroup().location
  kind: 'windows'
  sku: {
    name: 'F1'
    tier: 'Free'
    capacity: 0
  }
}

resource webApp 'Microsoft.Web/sites@2024-11-01' = {
  name: 'api-cf-sample-${salt}'
  location: resourceGroup().location
  kind: 'app'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: servicePlan.id
    httpsOnly: true
    sshEnabled: false
    siteConfig: {
      alwaysOn: false
      ftpsState: 'Disabled'
      netFrameworkVersion: '9.0'
      minTlsVersion: '1.3'
      windowsFxVersion: 'DOTNETCORE|9.0'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Test'
        }
      ]
    }
  }
}


output appName string = webApp.name

output keyVaultUri string = keyVault.properties.vaultUri

output appConfigUri string = appConfig.properties.endpoint

output appPrincipalId string = webApp.identity.principalId
