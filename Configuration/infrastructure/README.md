# IaC (Infrastructure as Code)

## Prerequisites

In order to deploy the stuff in this folder you need a working Azure subscription where you are at least `Contributor`.

The scripts in this directory rely on PowerShell 7 or newer. You can install it on basically every system. See the [Posh GitHub](https://github.com/PowerShell/PowerShell) for details.

After you opened a new posh session you need to ensure that:

1. Az module is installed: `Install-PsResource Az -Force`.
2. Import this module into your context: `Import-Module Az`.

Also ensure that [Bicep CLI](https://github.com/Azure/bicep) is installed on your machine.

## Login to Azure

Execute `Connect-AzAccount -Tenant [YOUR-TENANT-ID] -Subscription [SUBSCRIPTION_ID]`. You could be forced to authenticate with a popup!

## Deploy resources

Execute `./deploy.ps1`.

or `./deploy.ps1 -WhatIf` if you want to simulate the changes in Azure beforehand.