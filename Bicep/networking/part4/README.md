# Bicep VM

<div align="left">
      <a href="https://www.youtube.com/watch?v=999uwU0YbKY">
         <img src="https://img.youtube.com/vi/999uwU0YbKY/0.jpg" style="width:50%;">
      </a>
</div>

## Summary

In this video I want to show a current flaw in Bicep where a parameter which is resolved by a user-defined-function
fails to work when passed to a Network Security Group.

I created a [GitHub issue](https://github.com/Azure/bicep/issues/18908) which at the time of writing (2026-03-14) is
still under triage.

## Reproduction

In order to reproduce this issue just place a PowerShell session in this directory and execute:

```powershell
bicep lint ./main.bicep
```

It immediately fails with something like:

```cmd
Unhandled exception. System.Collections.Generic.KeyNotFoundException: The given key 'Bicep.Core.Semantics.ResourceSymbol' was not present in the dictionary.
```

Now edit the `nsg` resource in `vnet.bicep`. Find this block:

```bicep
resource nsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${suffix}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'allow-rdp'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 100
          protocol: 'Tcp'
          destinationPortRange: '3389'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'allow-icmp-from-jump-to-vm'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 110
          protocol: 'Icmp'
          destinationPortRange: '*'
          sourcePortRange: '*'
          sourceAddressPrefix: jumpHostSubnet
          destinationAddressPrefix: vmSubnet
        }
      }
    ]
  }
}
```

and replace `jumphostSubnet` and `vmSubnet` with the value "\*" like so:

```bicep
resource nsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${suffix}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'allow-rdp'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 100
          protocol: 'Tcp'
          destinationPortRange: '3389'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'allow-icmp-from-jump-to-vm'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 110
          protocol: 'Icmp'
          destinationPortRange: '*'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
    ]
  }
}
```

Then run the lint command again and no error occurs. Good but obviously not good really because now we allow traffic to
go uncontrolled for ICMP which is not what we want.

When I run `deploy.ps1` using the "\*" as the subnets the following result is produced:

```cmd
Id                      : /subscriptions/c1907265-583f-42c3-a236-9ecf192794e7/providers/Microsoft.Resources/deployments/deploy-cf-sample
DeploymentName          : deploy-cf-sample
Location                : westeurope
ProvisioningState       : Succeeded
Timestamp               : 14.03.2026 20:17:37
Mode                    : Incremental
TemplateLink            :
Parameters              :
                          Name              Type                       Value
                          ================  =========================  ==========
                          projectName       String                     "cf"
                          location          String                     "westeurope"
                          stage             String                     "demo"
                          addressPrefix     String                     "10.0.0.0/16"
                          subnets           Array                      ["VmSubnet","JumphostSubnet"]
                          adminUsername     String                     "codingfreaks"
                          adminPassword     SecureString               null
                          deployJumpHost    Bool                       false

Outputs                 :
                          Name              Type                       Value
                          ================  =========================  ==========
                          vmPrefix          String                     "10.0.1.0/24"
                          jumpHostPrefix    String                     "10.0.2.0/24"

DeploymentDebugLogLevel :
```

I put the variables which caused the problem earlier to the output which works just fine. So the user defined function works.

So then lets use them directly!

If you pass in the values (which are strings as you can see in the output) directly into the NSG it works:

```bicep
resource nsg 'Microsoft.Network/networkSecurityGroups@2025-01-01' = {
  name: 'nsg-${suffix}'
  location: location
  properties: {
    securityRules: [
      {
        name: 'allow-rdp'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 100
          protocol: 'Tcp'
          destinationPortRange: '3389'
          sourcePortRange: '*'
          sourceAddressPrefix: '*'
          destinationAddressPrefix: '*'
        }
      }
      {
        name: 'allow-icmp-from-jump-to-vm'
        properties: {
          access: 'Allow'
          direction: 'Inbound'
          priority: 110
          protocol: 'Icmp'
          destinationPortRange: '*'
          sourcePortRange: '*'
          sourceAddressPrefix: '10.0.2.0/24'
          destinationAddressPrefix: '10.0.1.0/24'
        }
      }
    ]
  }
}
```

the linting and deployment will work just fine.

## Conclusion

Obviously there is a problem in Bicep or in the NSG resource when you pass in variables which values are not static but generated
by user-defined-functions.

Guess from me: Because Bicep is .NET internally I guess that user-defined-functions are built with something called expressions there. This
will lead in another thing which conceptually is called "deferred execution". Meaning that as soon as the NSG-module wants to know the
IP ranges it needs to "understand" that it needs to execute the UDF in order to retrieve the value which is probably not done.

I just added this as a little test:

```bicep
resource routeTable 'Microsoft.Network/routeTables@2023-04-01' = {
  name: 'myRouteTable'
  location: location
  properties: {
    routes: [
      {
        name: 'sampleRoute'
        properties: {
          addressPrefix: vmPrefix
          nextHopType: 'None'
        }
      }
    ]
  }
}
```

This lints and builds perfectly and proofs that something is broken in the NSG resource module from MS.
