@description('Resolves a subnet by searching for its name in a list of submets.')
@export()
func resolveSubnetByName(subnets object[], subnetName string) object =>
  filter(map(subnets, (s, i) => { index: i, subnet: s }), (n, i) => n.subnet.name == subnetName)[0].subnet
