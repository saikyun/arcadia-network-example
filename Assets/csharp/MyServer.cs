using UnityEngine;
using UnityEngine.Networking;
using clojure.lang;

public class MyServer : NetworkManager
{
  public static IFn gotEdnFn;

  public void Start() {
    gotEdnFn = RT.var("game.core", "got-edn");
  }
  
  public override void OnStartServer()
  {
    NetworkServer.RegisterHandler(EdnMsg.MsgType, OnServerGotEdn);
    base.OnStartServer();
  }
  
  public override void OnStartClient(NetworkClient client)
  {
    client.RegisterHandler(EdnMsg.MsgType, OnClientGotEdn);
    base.OnStartClient(client);
  }
  
  public void OnServerGotEdn(NetworkMessage netMsg)  
  {  
    var msg = netMsg.ReadMessage<EdnMsg>();  
    Debug.Log("Server received message");  
    Debug.Log(msg.edn);
    gotEdnFn.invoke(msg.edn);
  } 
  
  public void OnClientGotEdn(NetworkMessage netMsg)  
  {  
    var msg = netMsg.ReadMessage<EdnMsg>();  
    Debug.Log("Client received message");  
    Debug.Log(msg.edn);  
    gotEdnFn.invoke(msg.edn);
  }
}
