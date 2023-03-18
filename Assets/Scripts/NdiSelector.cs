using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Klak.Ndi;
using Cysharp.Threading.Tasks;

public class NdiSelector : MonoBehaviour
{

  [SerializeField] NdiReceiver m_Receiver;
  List<string> m_SourceNames = new List<string>();
  int m_Index = 0;

  void Start()
  {
    Check();
  }

  async void Check()
  {
    bool found = false;
    while (true)
    {
      IEnumerable<string> sourceNames = NdiFinder.sourceNames;

      foreach (string name in sourceNames)
      {
        if (!m_SourceNames.Contains(name)) m_SourceNames.Add(name);
      }
      if (m_SourceNames.Count > 0 && !found)
      {
        m_Receiver.ndiName = m_SourceNames[m_Index];
        found = true;
      }

      await UniTask.Delay(1000);
    }
  }

  public void Next()
  {
    if (m_SourceNames.Count <= 0) return;

    m_Index++;
    m_Receiver.ndiName = m_SourceNames[m_Index % m_SourceNames.Count];
  }
}
