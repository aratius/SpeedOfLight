using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Unity.Custom;
using TMPro;

namespace Unity.Custom
{

  public struct ScreenInfo
  {
    public string name;
    public float width;
    public float height;
    public float depth;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    public ScreenInfo(
      string _name,
      float _width,
      float _height,
      float _depth,
      float _offsetX,
      float _offsetY,
      float _offsetZ
    )
    {
      name = _name;
      width = _width;
      height = _height;
      depth = _depth;
      offsetX = _offsetX;
      offsetY = _offsetY;
      offsetZ = _offsetZ;
    }
  }

  // TODO: DisplayIndexが要るかな？
  public class ScreenInfoUI : MonoBehaviour
  {

    public static Dictionary<string, ScreenInfo> caches = new Dictionary<string, ScreenInfo>();

    public bool hasAttached = false;
    [SerializeField] FloatValueUI m_Width;
    [SerializeField] FloatValueUI m_Height;
    [SerializeField] FloatValueUI m_Depth;
    [SerializeField] FloatValueUI m_OffsetX;
    [SerializeField] FloatValueUI m_OffsetY;
    [SerializeField] FloatValueUI m_OffsetZ;
    [SerializeField] TMP_Text m_TrackerName;
    [SerializeField] Button m_Button;

    UnityEvent m_OnSubmit = new UnityEvent();
    UnityEvent m_OnChange = new UnityEvent();

    public string name
    {
      set => m_TrackerName.text = value;
      get => m_TrackerName.text;
    }

    public UnityEvent onSubmit => m_OnSubmit;
    public UnityEvent onChange => m_OnChange;
    public float width => m_Width.value * .01f;
    public float height => m_Height.value * .01f;
    public float depth => m_Depth.value * .01f;
    public float offsetX => m_OffsetX.value * .01f;
    public float offsetY => m_OffsetY.value * .01f;
    public float offsetZ => m_OffsetZ.value * .01f;

    void Awake()
    {
      m_Width.onChange.AddListener(OnChange);
      m_Height.onChange.AddListener(OnChange);
      m_Depth.onChange.AddListener(OnChange);
      m_OffsetX.onChange.AddListener(OnChange);
      m_OffsetY.onChange.AddListener(OnChange);
      m_OffsetZ.onChange.AddListener(OnChange);

      m_Button.onClick.AddListener(OnClickButton);
    }

    public void Empty()
    {
      hasAttached = false;
      name = "No Select";
      m_Width.Reset();
      m_Height.Reset();
      m_Depth.Reset();
      m_OffsetX.Reset();
      m_OffsetY.Reset();
      m_OffsetZ.Reset();
    }

    public void Copy(ScreenInfo source)
    {
      hasAttached = true;
      name = source.name;
      m_Width.value = source.width * 100f;
      m_Height.value = source.height * 100f;
      m_Depth.value = source.depth * 100f;
      m_OffsetX.value = source.offsetX * 100f;
      m_OffsetY.value = source.offsetY * 100f;
      m_OffsetZ.value = source.offsetZ * 100f;
    }

    void OnChange()
    {
      m_OnChange.Invoke();
    }

    void OnClickButton()
    {
      m_OnSubmit.Invoke();
    }

  }

}