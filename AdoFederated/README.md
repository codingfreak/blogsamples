# Azure DevOps Federated Service-Connections Deep Dive

![YT](https://youtu.be/7BDoQSNX6go)

This folder contains the PowerShell script which will perform the creation of a new federated
service connection in Azure DevOps using a given service principal.

In order to run the sample, follow this guideline:

1. Create a new app registration in Entra ID and take note of its client id.
2. Define RBAC-based rights to a subscription X and take note of the subscription id.
3. Open the script and fill in the values in the block:

```powershell
$tenantId = ''
$servicePrincipalClientId = ''
$serviceConnectionName = ''
$subscriptionName = ''
$subscriptionId = ''
$orgName = ''
$projectName = ''
```

The subscription name and id should match the one from step 2. `servicePrincipalClientId` is the one from step 1.

Now you should be able to run the script.
