using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.Custom;

namespace Unity.Custom
{

  public class FloatValueUI : MonoBehaviour
  {

    [SerializeField] Slider m_Slider;
    [SerializeField] TMP_InputField m_Input;
    [SerializeField] float m_Min = 1;
    [SerializeField] float m_Max = 200;
    [SerializeField] float m_InitialValue = 30;
    float m_Value = -999;
    float m_LastValue = -999;
    UnityEvent m_OnChange = new UnityEvent();

    public float value
    {
      set => m_Value = Mathf.Clamp(value, m_Min, m_Max);
      get => m_Value;
    }

    public UnityEvent onChange
    {
      get => m_OnChange;
    }

    void Awake()
    {
      m_Value = m_InitialValue;
      m_Slider.minValue = m_Min;
      m_Slider.maxValue = m_Max;
      m_Slider.onValueChanged.AddListener(OnValueChangedSlider);
      m_Input.onEndEdit.AddListener(OnEndEditInput);
    }

    void Update()
    {
      if (m_Value != m_LastValue)
      {
        m_Slider.value = m_Value;
        m_Input.text = (Mathf.Floor(m_Value * 10f) / 10f).ToString();
        m_OnChange.Invoke();
      }
      m_LastValue = m_Value;
    }

    public void Reset()
    {
      m_Value = m_InitialValue;
    }

    void OnValueChangedSlider(float value)
    {
      m_Value = value;
    }

    void OnEndEditInput(string text)
    {
      float value;
      bool s = float.TryParse(text, out value);
      if (s)
      {
        m_Value = Mathf.Clamp(value, m_Min, m_Max);
      }
    }

  }
}