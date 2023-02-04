using UnityEngine;
using System.Collections.Generic;
using Unity.Custom;

namespace Unity.Custom
{

  public class ScreenManager : MonoBehaviour
  {

    [SerializeField] List<Screen> m_Displays = new List<Screen>();

    void Awake()
    {

    }

  }
}