# arcadia-network-example
Small example on how to mix UNet and Arcadia

Start a repl, load the namespace `game.core`. Then you can send and receive edn messages.
```
game.core=> (send-edn "{:data 123}")
true
game.core=> @data
{:data 123}
```

`Assets/csharp/EdnMsg.cs` contains a small class for transferring edn data.
`Assets/csharp/MyServer.cs` contains the code that calls `game.core/got-edn` when it gets an `EdnMsg`.
`Assets/game/core.clj` contains the clojure functions to send and receive messages.
