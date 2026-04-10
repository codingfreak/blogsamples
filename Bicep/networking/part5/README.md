# Bicep VM

<div align="left">
      <a href="https://www.youtube.com/watch?v=TODO">
         <img src="https://img.youtube.com/vi/TODO/0.jpg" style="width:50%;">
      </a>
</div>

## Summary

Here I fixed the Bicep error from part 4 which was finally tracked down to be an issue with circular references. It was fixed
by extracting the NSG rules from the NSG deployment so that Bicep could figure out the NSG resource id on the subnet definitions.

The problem was really that the error message was pretty misleading.
