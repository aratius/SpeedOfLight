using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.Custom;

namespace Unity.Custom
{

  public class IntValueUI : MonoBehaviour
  {

    [SerializeField] Slider m_Slider;
    [SerializeField] TMP_InputField m_Input;
    [SerializeField] int m_Min = 1;
    [SerializeField] int m_Max = 200;
    [SerializeField] int m_InitialValue = 30;
    int m_Value = -999;
    int m_LastValue = -999;
    UnityEvent m_OnChange = new UnityEvent();

    public int value
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
      m_Slider.onValueChanged.AddListener(OnSliderValueChanged);
      m_Input.onValueChanged.AddListener(OnInputValueChanged);
      m_Value = m_InitialValue;
    }

    void Update()
    {
      if (m_Value != m_LastValue)
      {
        m_Slider.value = Utils.LinearEquationThrough2Points((float)m_Min, (float)m_Max, 0, 1f)((float)m_Value);
        m_Input.text = m_Value.ToString();
        m_OnChange.Invoke();
      }
      m_LastValue = m_Value;
    }

    public void Reset()
    {
      m_Value = m_InitialValue;
    }

    void OnSliderValueChanged(float value)
    {
      m_Value = (int)Utils.LinearEquationThrough2Points(0, 1f, (float)m_Min, (float)m_Max)(value);
    }

    void OnInputValueChanged(string text)
    {
      int value;
      bool s = int.TryParse(text, out value);
      if (s)
      {
        m_Value = Mathf.Clamp(value, m_Min, m_Max);
      }
      else
      {
        m_Input.text = m_Value.ToString();
      }
    }

  }
}