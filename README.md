# arcadia-network-example
Small example on how to mix UNet and Arcadia

## Dependencies
You have to clone https://github.com/arcadia-unity/Arcadia into `Assets/`.
I used this parcticular commit: dbfbd785b9e18ed9f83996b2608169991b4ada76 (https://github.com/arcadia-unity/Arcadia/commit/dbfbd785b9e18ed9f83996b2608169991b4ada76)


## How to
### Clicking a button
Hit play in unity, then click "LAN Host(H)".
To test if it works, just click the button. You should see some messages in the Unity Console.

### Using the repl
If you want to try sending custom stuff, do the following:
Start a repl, load the namespace `game.core`. Then you can send and receive edn messages.
```
game.core=> (send-edn-to-all "{:data 123}")
true
game.core=> @data
{:data 123}
```

### Communicating with a standalone client
1. If you want to, you can edit `Assets/game/core.clj` and remove a comment to enable a (basic) repl in the standalone client.
2. If you build a binary (by clicking `Arcadia/Build/Prepare Export`, then `File/Build & Run`), then you can run it, either as a host or a client, and then use the Unity Editor as the opposite (e.g. if the standalone is the host, the editor should be client).
3. If you then click the button in the client, some messages should appear in the Unity Console.
4. If you enabled the repl, you can connect to it as usual (but with port 6015, or whatever you set it to).
5. Then you can load the namespace `(in-ns 'game.core)`, and checkout the data by running `@data`, or you could communicate a bit more, e.g. with `(send-edn-to-all "{:data 123}")`.
6. Play around a bit! Have fun!

## Definitely check these files out
You'll probably get the most value from checking the source of these files (it's like 150 lines of code)

* `Assets/csharp/EdnMsg.cs` contains a small class for transferring edn data.
* `Assets/csharp/MyServer.cs` contains the code that calls `game.core/got-edn` when it gets an `EdnMsg`.
* `Assets/game/core.clj` contains the clojure functions to send and receive messages.
