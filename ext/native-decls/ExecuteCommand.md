---
ns: CFX
apiset: shared
---
## EXECUTE_COMMAND

```c
void EXECUTE_COMMAND(char* commandString);
```


## Parameters
* **commandString**: The entire command to execute

## Examples
```lua
RegisterCommand("leave_vehicle", function(source, args)
    local shouldLeaveInstantly = args[1] == "now"
    local ped = PlayerPedId()
    local vehicle = GetVehiclePedIsIn(ped)

    -- we're not in a vehicle we shouldn't do anything.
    if vehicle == 0 then return end

    if not shouldLeaveInstantly then
        TaskLeaveVehicle(PlayerPedId(), vehicle, 0)
    else
        ClearPedTasksImmediately(PlayerPedId())
    end
end, false)

ExecuteCommand("leave_vehicle now")
```

