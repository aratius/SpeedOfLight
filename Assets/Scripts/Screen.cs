using UnityEngine;

namespace Unity.Custom
{

  public class Screen : MonoBehaviour
  {

    [SerializeField] Camera m_Camera;
    GameObject? m_Tracker;
    Vector3 m_Offset;
    string m_Key;

    public string key => m_Key;

    public void Init(string key, Vector3 size, GameObject tracker, Vector3 offset, int displayIndex)
    {
      m_Key = key;
      transform.localScale = size;
      m_Tracker = tracker;
      m_Offset = offset;
      m_Camera.targetDisplay = displayIndex;
      ApplyTransform();
    }

    public void UpdateInfo(Vector3 size, Vector3 offset, int? displayIndex = null)
    {
      transform.localScale = size;
      m_Offset = offset;
      m_Camera.targetDisplay = displayIndex == null ? m_Camera.targetDisplay : (int)displayIndex;
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