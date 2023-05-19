using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using OscJack;
using Unity.Custom;

namespace Unity.Custom
{

  public class OscSender : SingletonMonoBehaviour<OscSender>
  {

    [SerializeField] string m_Ip = "127.0.0.1";
    [SerializeField] int m_Port = 10000;

    OscClient m_Client;

    void Awake()
    {
      m_Client = new OscClient(m_Ip, m_Port);
    }

    public void Send(string address, int value)
    {
      m_Client.Send(address, value);
    }

    public void Send(string address, int value1, int value2)
    {
      m_Client.Send(address, value1, value2);
    }

    public void Send(string address, int value1, int value2, int value3)
    {
      m_Client.Send(address, value1, value2, value3);
    }

    void OnDestroy()
    {
      m_Client.Dispose();
      Debug.Log("Dispose");
    }

  }

}
