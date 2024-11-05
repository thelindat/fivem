---
ns: CFX
apiset: server
---
## GET_PLAYER_IDENTIFIER

```c
char* GET_PLAYER_IDENTIFIER(char* playerSrc, int identIndex);
```

## Parameters
* **playerSrc**: The player to get the identifier of
* **identIndex**: The identifier index to get, see [GET_NUM_PLAYER_IDENTIFIERS](#_0xFF7F66AB) for an example

## Return value
Returns the current identifier at `identIndex` or `null` if there wasn't one.
