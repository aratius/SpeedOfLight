using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Custom;

public class BoxManager : MonoBehaviour
{
  [SerializeField] ScreenManager m_Screen;
  [SerializeField] GameObject m_BoxPrefab;
  [SerializeField] Camera m_CaptureCamera;
  [SerializeField] Camera m_DrawCamera;
  private List<Box> m_Boxes = new List<Box>();

  void Update()
  {
    if (m_Screen.length > m_Boxes.Count) FillBoxes();
  }

  void FillBoxes()
  {
    while (m_Screen.length > m_Boxes.Count)
    {
      GameObject go = Instantiate(m_BoxPrefab, transform);
      Box box = go.GetComponent<Box>();
      m_Boxes.Add(box);
      box.Init(m_Screen.Get(m_Boxes.Count - 1).body, m_CaptureCamera, m_DrawCamera);
    }
  }

}
