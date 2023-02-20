using UnityEngine;

namespace Unity.Custom
{

  public class Screen : MonoBehaviour
  {

    [SerializeField] Camera m_Camera;
    GameObject? m_Tracker;
    Vector3 m_Offset;

    public void Init(Vector3 size, GameObject tracker, Vector3 offset, int displayIndex)
    {
      transform.localScale = size;
      m_Tracker = tracker;
      m_Offset = offset;
      m_Camera.targetDisplay = displayIndex;
      ApplyTransform();
    }

    void Update()
    {
      if (!m_Tracker) return;
      ApplyTransform();
    }

    void ApplyTransform()
    {
      transform.localPosition = m_Tracker.transform.localPosition + m_Offset;
      transform.localRotation = m_Tracker.transform.localRotation;
    }

  }
}