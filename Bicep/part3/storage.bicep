targetScope = 'resourceGroup'

resource storage 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: 'stoddcodingfreaks'  
  location: resourceGroup().location  
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'BlobStorage'
  properties: {
    accessTier: 'Cool'
  }
}
