# Bicep VM

<div align="left">
      <a href="https://www.youtube.com/watch?v=9rQH20bLj78">
         <img src="https://img.youtube.com/vi/9rQH20bLj78/0.jpg" style="width:50%;">
      </a>
</div>

## Summary

After we deployed a first VM in the last session we now are deploying another VM to a new resource group and connect
it to the `JumphostSubnet` using a fresh NIC and Public IP.

In the video you'll see me configuring our target VM using the following command:

```cmd
netsh advfirewall firewall add rule name="ICMP Allow incoming V4 echo request" protocol="icmpv4:8,any" dir=in action=allow
```

which opens the Windows Firewall for ICMP on the target machine. This allows me to ping the machine from the Jumphost.

Also important is the command to remove the jumphost resources:

```powershell
Remove-AzResourceGroup -Name rg-cf-jump-demo -Force
```

and the command to stop AND DEALLOCATE our target vm:

```powershell
Stop-AzVM -ResourceGroupName rg-cf-vm-demo -Name vm-cf-one-demo -Force
```
