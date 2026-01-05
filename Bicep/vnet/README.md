# Bicep VNET

## Summary

It's been a while since my latest Bicep tutorial. This time I want to show how to create a VNET with some resources in 2026.

The current Bicep CLI version is 0.39.26.

## Episode 1

<div align="left">
      <a href="https://www.youtube.com/watch?v=VnFkay7gtzo">
         <img src="https://img.youtube.com/vi/VnFkay7gtzo/0.jpg" style="width:50%;">
      </a>
</div>

This part covers the creation of the VNET including some subnets. In the video I show how to create a VNET in Bicep the
naive way first. After that I explain how to utilize Bicep functions like `cidrAddress`, `union`, `map` and `filter` in order to achieve a more sustainable and re-usable result.

The video covers a lot of other topics. Mostly I try to explain concepts like gateways, CIDR notations and thoughts during
network planning.
