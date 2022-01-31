targetScope = 'resourceGroup'

param adminUser string

@secure()
param adminPass string

var options = json(loadTextContent('settings.json'))

var myGuid = guid(resourceGroup().id, 'bla-bla')

// module test 'myModule.bicep' = {
//   name: 'test'
//   params: {
//     options: options
//   }
// }

output foo string = myGuid
output optionsResult object = options
output username string = adminUser
// !!!! Never DO THIS! 🔥🔥🔥🔥🔥🔥
// output password string = adminPass
