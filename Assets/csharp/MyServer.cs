using UnityEngine;
using UnityEngine.Networking;
using clojure.lang;

public class MyServer : NetworkManager
{
  public static IFn gotEdnFn;
  public static IFn sendEdnToAllFn;
  
  public void Start() {
    gotEdnFn = RT.var("game.core", "got-edn");    
    sendEdnToAllFn = RT.var("game.core", "send-edn-to-all");    
  }
  
  // This is needed when the VM restarts
  public void Update() {
    if (gotEdnFn == null) { gotEdnFn = RT.var("game.core", "got-edn"); }
    if (sendEdnToAllFn == null) { sendEdnToAllFn = RT.var("game.core", "send-resp"); }
  }
  
  public override void OnStartServer()
  {
    Debug.Log("starting server...");
    NetworkServer.RegisterHandler(EdnMsg.MsgType, OnServerGotEdn);
    base.OnStartServer();
  }
  
  public override void OnStartClient(NetworkClient client)
  {
    Debug.Log("starting client...");
    client.RegisterHandler(EdnMsg.MsgType, OnClientGotEdn);
    base.OnStartClient(client);
  }
  
  public void OnServerGotEdn(NetworkMessage netMsg)  
  {  
    var msg = netMsg.ReadMessage<EdnMsg>();  
    Debug.Log("Server received message from client " + netMsg.conn.connectionId);  
    Debug.Log(msg.edn);
    gotEdnFn.invoke(msg.edn);
    sendEdnToAllFn.invoke("to server, a message came from " + netMsg.conn.connectionId);
  }
  
  public void OnClientGotEdn(NetworkMessage netMsg)  
  {  
    var msg = netMsg.ReadMessage<EdnMsg>();
    Debug.Log("Client received message");
    Debug.Log("msg: " + msg.edn);
    gotEdnFn.invoke(msg.edn);
  }
}
