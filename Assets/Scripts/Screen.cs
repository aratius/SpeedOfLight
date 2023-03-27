using UnityEngine;

namespace Unity.Custom
{

  public class Screen : MonoBehaviour
  {

    [SerializeField] GameObject m_ScreenBody;
    [SerializeField] Camera m_Camera;
    GameObject? m_Tracker;
    Vector3 m_Offset;
    string m_Key;

    public string key => m_Key;

    public void Init(string key, Vector3 size, GameObject tracker, Vector3 offset, int displayIndex)
    {
      m_Key = key;
      m_Tracker = tracker;
      UpdateInfo(size, offset, displayIndex);
      ApplyTransform();
    }

    public void UpdateInfo(Vector3 size, Vector3 offset, int? displayIndex = null)
    {
      m_ScreenBody.transform.localScale = size;
      m_Offset = offset;
      m_Camera.targetDisplay = displayIndex == null ? m_Camera.targetDisplay : (int)displayIndex;
      // TODO: containなサイズセット（その場合ディスプレイアスペクトもセットする必要がある）とりあえず縦フィットで
      m_Camera.orthographicSize = size.y / 2f;
      ApplyTransform();
    }

    void Update()
    {
      if (!m_Tracker) return;
      ApplyTransform();
    }

    void ApplyTransform()
    {
      Vector3 p = m_Tracker.transform.localPosition;
      Quaternion q = m_Tracker.transform.localRotation;
      Vector3 r = m_Tracker.transform.eulerAngles;
      if (p.x != 0 && p.y != 0 && p.z != 0) transform.localPosition = p;
      if (r.x != 0 && r.y != 0 && r.z != 0) transform.localRotation = q;
      m_ScreenBody.transform.localPosition = m_Offset;
    }

  }
}