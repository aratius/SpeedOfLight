using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{

  Quaternion m_Target;
  float cnt = 0;

  void Update()
  {
    Quaternion crr = transform.localRotation;
    transform.localRotation = Quaternion.Slerp(crr, m_Target, .1f);
  }

  public void SetRotation(Vector3 eulerAngles)
  {
    m_Target = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    // transform.localRotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z);
    // transform.localEulerAngles = eulerAngles;
  }

}
