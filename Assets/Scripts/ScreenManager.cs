using UnityEngine;
using System.Collections.Generic;
using Unity.Custom;
using DG.Tweening;

namespace Unity.Custom
{

  public class ScreenManager : MonoBehaviour
  {

    [SerializeField] TrackerManager m_TrackerManager;
    [SerializeField] List<Screen> m_Screens = new List<Screen>();
    int m_Index = 0;

    void Awake()
    {

    }

    void Update()
    {
      if(Input.GetKey(KeyCode.Alpha0))
      {
        m_Index = 0;
      }
      else if(Input.GetKey(KeyCode.Alpha1))
      {
        m_Index = 1;
      }
      else if(Input.GetKey(KeyCode.Alpha2))
      {
        m_Index = 2;
      }

      foreach (Screen screen in m_Screens)
      {
        Vector3 position = m_TrackerManager.GetPositionFromIndex(m_Index);
        Quaternion rotation = m_TrackerManager.GetRotationFromIndex(m_Index);
        // screen.transform.position = position;
        screen.transform.DOMove(position - Vector3.forward * .25f, .5f).SetEase(Ease.OutSine);
        // screen.transform.rotation = rotation;
        screen.transform.DORotate(rotation.eulerAngles, .5f).SetEase(Ease.OutSine);

      }
    }

  }
}