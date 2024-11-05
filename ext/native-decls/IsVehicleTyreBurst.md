---
ns: CFX
apiset: server
---
## IS_VEHICLE_TYRE_BURST

```c
BOOL IS_VEHICLE_TYRE_BURST(Vehicle vehicle, int wheelID, BOOL completely);
```

-- TODO: Check that this actually uses the proper wheel ids

## Parameters
* **vehicle**: The vehicle to get the tire of
* **wheelID**: The wheel id
* **completely**: If it should check if it's completely destroyed

## Return value
Returns `true` if the vehicle tire is busted, if `completely` is `true` it will only return if the tire is on its rims.
