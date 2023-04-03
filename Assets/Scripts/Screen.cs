using UnityEngine;

namespace Unity.Custom
{

  public enum OutlineType
  {
    Focus,
    Null,
    Camera
  }

  public class Screen : MonoBehaviour
  {

    [SerializeField] GameObject m_ScreenBody;
    [SerializeField] Camera m_Camera;
    Outline m_Outline;
    GameObject? m_Tracker;
    Vector3 m_Offset;
    string m_Key;

    public bool cameraEnabled
    {
      get => m_Camera.enabled;
    }

    void Start()
    {
      m_Outline = GetComponent<Outline>();
    }

    public string key => m_Key;

    public void Init(string key, Vector3 size, GameObject tracker, Vector3 offset)
    {
      m_Key = key;
      m_Tracker = tracker;
      UpdateInfo(size, offset);
      ApplyTransform();
    }

    public void UpdateInfo(Vector3 size, Vector3 offset)
    {
      m_ScreenBody.transform.localScale = size;
      m_Offset = offset;
      // TODO: containなサイズセット（その場合ディスプレイアスペクトもセットする必要がある）とりあえず縦フィットで
      m_Camera.orthographicSize = size.y / 2f;
      ApplyTransform();
    }

    public void SetCamera(int displayIndex)
    {
      if(displayIndex == 0)
      {
        m_Camera.enabled = false;
      }
      else
      {
        m_Camera.enabled = true;
        m_Camera.targetDisplay = displayIndex;
      }
    }

    public void SetOutline(OutlineType type)
    {
      if(type == OutlineType.Focus)
      {
        m_Outline.OutlineWidth = 30;
        m_Outline.OutlineColor = Color.red;
      }
      else if(type == OutlineType.Null)
      {
        m_Outline.OutlineWidth = 0;
      }
      else if(type == OutlineType.Camera)
      {
        m_Outline.OutlineWidth = 30;
        m_Outline.OutlineColor = Color.HSVToRGB((float)m_Camera.targetDisplay / 8f, 1f, 1f);
      }
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