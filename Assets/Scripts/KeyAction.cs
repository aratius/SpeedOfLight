using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct KeyActionConfig
{
  public KeyCode key;
  public UnityEvent onDown;
  public UnityEvent onUp;
}

public class KeyAction : MonoBehaviour
{

  [SerializeField]
  List<KeyActionConfig> m_ConfigList = new List<KeyActionConfig>();

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    foreach (KeyActionConfig config in m_ConfigList)
    {
      if (Input.GetKeyDown(config.key)) config.onDown.Invoke();
      else if (Input.GetKeyUp(config.key)) config.onUp.Invoke();
    }
  }
}
