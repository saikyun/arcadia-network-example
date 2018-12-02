# arcadia-network-example
Small example on how to mix UNet and Arcadia.

![Talking to a server, using a repl connected to a client](https://new.memset.se/5853/ZzRYdnh1VStTUDBJdUE9PQ)\
*Talking to a server, using a repl connected to a client*

## Dependencies
You have to clone https://github.com/arcadia-unity/Arcadia into `Assets/`. I used this parcticular commit: [dbfbd785b9e18ed9f83996b2608169991b4ada76](https://github.com/arcadia-unity/Arcadia/commit/dbfbd785b9e18ed9f83996b2608169991b4ada76)

If you haven't tried Arcadia I'd recommend reading more about it first, maybe following a real [tutorial](https://github.com/arcadia-unity/Arcadia/wiki/Resources#Tutorials) or something.

You're not required to know about UNet to test this example, but if you actually want to create something with it, I recommend reading [the official tutorial on UNet](https://unity3d.com/learn/tutorials/s/multiplayer-networking).

I've only tested it on OSX, but it should work on other platforms.

## How to
### Open a scene
After starting unity, open the scene `Assets/Scenes/SampleScene`.

### Click a button
Hit play in unity, then in game mode, click "LAN Host(H)". This starts hosting using UNets [NetworkManager](https://docs.unity3d.com/ScriptReference/Networking.NetworkManager.html), modified slightly in [`Assets/csharp/MyServer.cs`](https://github.com/Saikyun/arcadia-network-example/blob/master/Assets/csharp/MyServer.cs).

![Menu and button in game mode](https://new.memset.se/5854/U0h0OGMzVU1kOHZ3OUE9PQ)\
*It should look like this*

To test if it works, just click the "Send to Server" button. This sends a pre defined EDN-message from the client to the server. Currently both the server and the single client is run in the Unity Editor. You should see some messages in the Unity Console.

![Network messages](https://new.memset.se/5856/L1M5ekp4Tm1KN1VRUXc9PQ)\
*Some messages*

What happens is that the server gets the message, then it sends a response containing the string "to server, a message came from <connection id>".

### Use the repl
If you want to try sending custom EDN, do the following:

Start a [repl in the way you prefer](https://github.com/arcadia-unity/Arcadia/wiki/REPL), load the namespace `game.core`. Then you can send and receive edn messages.
```clojure
game.core=> (send-edn-to-all "{:data 123}")
;; true
game.core=> @data
;; {:data 123}
```
`send-edn-to-all` looks like this:
```clojure
(defn send-edn-to-all [edn]
  (.. NetworkServer (SendToAll (.. EdnMsg MsgType) (EdnMsg. (prn-str edn)))))
```
It uses a static UNet function to send messages to all network entities it's connected to. In these examples we'll only call it from the client, and the clients are only connected to the server, so the name is a bit misleading.

### Communicate with a standalone game
1. If you want to, you can edit `Assets/game/core.clj` and remove a comment to enable a (basic) repl in the standalone game. If you do, you need to add the comment again before clicking Play, because otherwise the repl will be started in the Unity editor, and the port will already be used when you start the standalone game. If this happens, try exiting and entering play, and the port should be available again.
2. Build a binary (by clicking `Arcadia/Build/Prepare Export`, then `File/Build & Run`), then run the exported binary, then you can use it either as a host or a client, and then use the Unity Editor as the opposite (e.g. if the standalone is the host, the editor should be client).
3. Then click the "Send to Server" button in the standalone game. Some messages should appear in the Unity Console.
4. If you enabled the repl, you can connect to it as usual (but with port 6015, or whatever you set it to).
5. Then you can load the namespace `(in-ns 'game.core)`, and check the data by running `@data`, or you could communicate a bit more, e.g. with `(send-edn-to-all "{:data 123}")`.
6. Play around a bit! Have fun!

## Definitely check these files out
You'll probably get the most value from checking the source of these files (it's like 150 lines of code):

* `Assets/csharp/EdnMsg.cs` contains a small class for transferring edn data.
* `Assets/csharp/MyServer.cs` contains the code that calls `game.core/got-edn` when it gets an `EdnMsg`.
* `Assets/game/core.clj` contains the clojure functions to send and receive messages.

## Questions?
I'm quite often available at https://gitter.im/arcadia-unity/Arcadia :)

I also use [Twitter](https://twitter.com/saikyun).
