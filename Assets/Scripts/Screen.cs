using UnityEngine;

namespace Unity.Custom
{

  public class Screen : MonoBehaviour
  {

    GameObject? m_Tracker;
    Vector3 m_Offset;

    public void Init(Vector3 size, GameObject tracker, Vector3 offset)
    {
      transform.localScale = size;
      m_Tracker = tracker;
      m_Offset = offset;
    }

    void Update()
    {
      if(!m_Tracker) return;
      transform.localPosition = m_Tracker.transform.localPosition + m_Offset;
      transform.localRotation = m_Tracker.transform.localRotation;
    }

  }
}