using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OscJack;
using Unity.Custom;
using Tiutilities;

namespace Unity.Custom
{

  public class OscReceiver : SingletonMonoBehaviour<OscReceiver>
  {

    [SerializeField] int m_Port;

    OscServer m_Server;

    void Awake()
    {
      m_Server = new OscServer(m_Port);
    }

    public void AddCallback(string address, UnityAction<string, OscDataHandle> callback)
    {
      m_Server.MessageDispatcher.AddCallback(address, (string address, OscDataHandle data) => callback(address, data));
    }

    public void RemoveCallback(string address, UnityAction<string, OscDataHandle> callback)
    {
      //   m_Server.MessageDispatcher.RemoveCallback(address, (string address, OscDataHandle data) => callback(data));
    }

    void OnDestroy()
    {
      m_Server.Dispose();
      Debug.Log("Dispose");
    }

  }

}
