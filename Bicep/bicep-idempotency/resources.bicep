targetScope = 'resourceGroup'

param location string

param name string

resource asp 'Microsoft.Web/serverfarms@2023-01-01' = {
  name: 'asp-dd-${name}'
  location: location
  sku: {
    name: 'S1'
    tier: 'Standard'
  }
  kind: 'windows'
  properties: {}
}

resource site 'Microsoft.Web/sites@2022-09-01' = {
  name: 'api-dd-${name}'
  location: location
  dependsOn: [memoryAlert, cpuAlert]
  kind: 'windows'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: asp.id
  }
}

resource memoryAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${asp.name}-memory'
  location: 'global'
  properties: {
    description: 'Alert for unusual memory in ${asp.name}. Upscaling might be neccessary.'
    severity: 2
    enabled: true
    scopes: [
      asp.id
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT1H'
    targetResourceType: 'Microsoft.Web/serverfarms'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.MultipleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'MemoryPercantage'
          timeAggregation: 'Average'
          metricNamespace: 'Microsoft.Web/serverfarms'
          metricName: 'MemoryPercentage'
          operator: 'GreaterThan'
          criterionType: 'DynamicThresholdCriterion'
          alertSensitivity: 'Medium'
          failingPeriods: {
            numberOfEvaluationPeriods: 4
            minFailingPeriodsToAlert: 4
          }
        }
      ]
    }
  }
}

resource cpuAlert 'Microsoft.Insights/metricAlerts@2018-03-01' = {
  name: 'alert-${asp.name}-cpu'
  location: 'global'
  #disable-next-line no-unnecessary-dependson
  dependsOn: [asp]
  properties: {
    description: 'Alert for unusual CPU in ${asp.name}. Upscaling might be neccessary.'
    severity: 2
    enabled: true
    scopes: [
      asp.id
    ]
    evaluationFrequency: 'PT5M'
    windowSize: 'PT1H'
    targetResourceType: 'Microsoft.Web/serverfarms'
    criteria: {
      'odata.type': 'Microsoft.Azure.Monitor.MultipleResourceMultipleMetricCriteria'
      allOf: [
        {
          name: 'CpuPercentage'
          timeAggregation: 'Average'
          metricNamespace: 'Microsoft.Web/serverfarms'
          metricName: 'CpuPercentage'
          operator: 'GreaterThan'
          criterionType: 'DynamicThresholdCriterion'
          alertSensitivity: 'Medium'
          failingPeriods: {
            numberOfEvaluationPeriods: 4
            minFailingPeriodsToAlert: 4
          }
        }
      ]
    }
  }
}
