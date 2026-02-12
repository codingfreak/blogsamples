@description('Resolves a subnet by searching for its name in a list of subnets.')
@export()
func resolveSubnetByName(
  subnets resourceInput<'Microsoft.Network/virtualNetworks@2025-01-01'>.properties.subnets,
  subnetName string
) object => filter(map(subnets, (s, i) => { index: i, subnet: s }), (n, i) => n.subnet.name == subnetName)[0].subnet

@description('Resolves a subnet offset by searching for its name in a list of subnets.')
@export()
func resolveSubnetOffsetByName(
  subnets resourceInput<'Microsoft.Network/virtualNetworks@2025-01-01'>.properties.subnets,
  subnetName string
) int => filter(map(subnets, (s, i) => { index: i, subnet: s }), (n, i) => n.subnet.name == subnetName)[0].index

@description('Resolves a subnet offset by searching for its name in a list of subnets.')
@export()
func resolveSubnetPrefixByName(
  subnets resourceInput<'Microsoft.Network/virtualNetworks@2025-01-01'>.properties.subnets,
  subnetName string
) string =>
  filter(map(subnets, (s, i) => { index: i, subnet: s }), (n, i) => n.subnet.name == subnetName)[0].subnet.properties.addressPrefix
