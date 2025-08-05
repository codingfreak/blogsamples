targetScope = 'resourceGroup'

param appPrincipalId string

var roleDefinitionIdKeyVaultSecretsUser = ''

var roleDefinitionIdAppConfigDataReader = ''

resource appServiceKeyVaultSecretReader 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('app', 'keyvault', appPrincipalId, roleDefinitionIdKeyVaultSecretsUser)
  scope: resourceGroup()
  properties: {
    principalId: appPrincipalId
    roleDefinitionId: roleDefinitionIdKeyVaultSecretsUser
  }
}

resource appServiceAppConfigDataReader 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid('app', 'appconfig', appPrincipalId, roleDefinitionIdAppConfigDataReader)
  scope: resourceGroup()
  properties: {
    principalId: appPrincipalId
    roleDefinitionId: roleDefinitionIdAppConfigDataReader
  }
}
