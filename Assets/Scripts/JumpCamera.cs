using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct JumpConfig
{
  public KeyCode key;
  public GameObject target;
}

public class JumpCamera : MonoBehaviour
{

  [SerializeField]
  OrbitControls m_Controls;
  [SerializeField]
  List<JumpConfig> m_ConfigList = new List<JumpConfig>();

  void Update()
  {
    foreach (JumpConfig config in m_ConfigList)
    {
      if (Input.GetKeyDown(config.key))
      {
        m_Controls.enabled = false;
        Debug.Log("hoge");
        Vector3 p = config.target.transform.position;
        Quaternion q = config.target.transform.rotation;
        transform.position = new Vector3(p.x, p.y, p.z);
        transform.rotation = new Quaternion(q.x, q.y, q.z, q.w);
      }
      else if (Input.GetKeyUp(config.key))
      {
        m_Controls.enabled = true;
      }
    }
  }
}
