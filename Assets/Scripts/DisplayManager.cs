using UnityEngine;
using System.Collections.Generic;
using Unity.Custom;

namespace Unity.Custom
{

  public class DisplayManager : MonoBehaviour
  {

    [SerializeField] List<Display> m_Displays = new List<Display>();

    void Awake()
    {

    }

  }
}