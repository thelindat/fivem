---
ns: CFX
apiset: shared
---
## GET_REGISTERED_COMMANDS

```c
object GET_REGISTERED_COMMANDS();
```


The data returned adheres to the following layout:
```
[
{
"name": "cmdlist",
"arity": -1,
},
{
"name": "command1",
"arity": -1,
}
]
```

## Return value
Returns all commands that are registered in the command system.
