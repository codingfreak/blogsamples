using 'main.bicep'

param location = 'westeurope'

param projectName = 'cf'

param stage = 'demo'

param addressPrefix = '10.0.0.0/16'

param subnets = ['VmSubnet', 'TestSubnet', 'JumphostSubnet']

param adminUsername = 'codingfreaks'

param adminPassword = az.getSecret(
  'c1907265-583f-42c3-a236-9ecf192794e7',
  'rg-management',
  'akv-dd-test-mgmt',
  'codingfreaks-vm-admin-password'
)

param deployJumpHost = false
