# Summary

This area contains information used in the Youtube tutorial [Bicep Part 2](https://youtu.be/Noc1ApgTHOk).

The scripts and templates shown here will deploy a Storage Account in Azure as we did in the first part. Now we are adding
containers in this step and thus are looking at dependencies between resources. We also cover parameters in more detail
and show loops.

# Usage

Execute `deploy.ps1` after you've ensured that the Azure subscription is pointing to the one you want (`Get-AzContext`).

Execute `clean.ps1` to get rid of the resource group.
