---
ns: CFX
apiset: client
game: gta5
---
## IS_VEHICLE_NEEDS_TO_BE_HOTWIRED

```c
BOOL IS_VEHICLE_NEEDS_TO_BE_HOTWIRED(Vehicle vehicle);
```

TODO: Rename to something more gramatically correct
DOES_VEHICLE_NEED_TO_BE_HOTWIRED?

Getter for [SET_VEHICLE_NEEDS_TO_BE_HOTWIRED](#_0xFBA550EA44404EE6)


## Parameters
* **vehicle**: The vehicle to check

## Return value
Returns true if the vehicle needs to be hotwired before being able to start

## Examples
```lua
local vehicle = GetVehiclePedIsIn(PlayerPedId())
local needs_hotwiring = DoesVehicleNeedToBeHotwired(vehicle)
```
