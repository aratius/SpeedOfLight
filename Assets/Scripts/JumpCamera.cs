using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCamera : MonoBehaviour
{

  [SerializeField]
  OrbitControls m_Controls;
  [SerializeField]


  public void ToTarget(Transform target)
  {
    m_Controls.enabled = false;
    Debug.Log("hoge");
    Vector3 p = target.position;
    Quaternion q = target.rotation;
    transform.position = new Vector3(p.x, p.y, p.z);
    transform.rotation = new Quaternion(q.x, q.y, q.z, q.w);
  }

  public void Unlock()
  {
    m_Controls.enabled = true;
  }

}
