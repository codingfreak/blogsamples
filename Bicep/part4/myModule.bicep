param options object

resource storage 'Microsoft.Storage/storageAccounts@2021-06-01' = {
  name: 'sto${options.prefix}${options.project}'
  location: options.location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'BlobStorage'
}
