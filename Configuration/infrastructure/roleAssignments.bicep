targetScope = 'resourceGroup'

param appPrincipalId string

var roleDefinitionIdKeyVaultSecretsUser = '4633458b-17de-408a-b874-0445c86b69e6'
var roleDefinitionIdAppConfigDataReader = '516239f1-63e1-4d78-a4de-a74fb236a071'

resource appServiceKeyVaultSecretReader 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('app', 'keyvault', appPrincipalId, roleDefinitionIdKeyVaultSecretsUser)
  scope: resourceGroup()
  properties: {
    principalId: appPrincipalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionIdKeyVaultSecretsUser)
    principalType: 'ServicePrincipal'
  }
}

resource appServiceAppConfigDataReader 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('app', 'appconfig', appPrincipalId, roleDefinitionIdAppConfigDataReader)
  scope: resourceGroup()
  properties: {
    principalId: appPrincipalId
    roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', roleDefinitionIdAppConfigDataReader)
    principalType: 'ServicePrincipal'
  }
}
