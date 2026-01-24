using 'main.bicep'

param location = 'westeurope'

param projectName = 'cf'

param stage = 'demo'

param addressPrefix = '10.0.0.0/16'

param subnets = ['VmSubnet', 'JumphostSubnet']

param adminUsername = 'codingfreaks'

param adminPassword = az.getSecret(
  'c764670f-e928-42c2-86c1-e984e524018a',
  'rg-management',
  'akv-dd-test-mgmt',
  'codingfreaks-vm-admin-password'
)
