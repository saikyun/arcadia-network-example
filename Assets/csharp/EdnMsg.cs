using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class EdnMsg : MessageBase
{
  public static short MsgType = 300;
  public string edn;

  public EdnMsg() {}

  public EdnMsg(string edn) {
    this.edn = edn;
  }
}

