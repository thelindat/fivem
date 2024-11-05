---
ns: CFX
apiset: shared
---
## IS_ACE_ALLOWED

```c
BOOL IS_ACE_ALLOWED(char* object);
```

## Parameters
* **object**: The permission to check for, i.e. "command.ensure"

## Return value
For the server returns `true` if the specific ScRT has access to the specified ace permission
For the client returns `true` if the player has the specified ace permission
