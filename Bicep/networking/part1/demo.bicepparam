using 'main.bicep'

param location = 'westeurope'

param projectName = 'cf'

param stage = 'demo'

param addressPrefix = '10.0.0.0/16'

param subnets = ['VmSubnet', 'JumphostSubnet']
