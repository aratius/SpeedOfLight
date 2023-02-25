using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Custom;

namespace Unity.Custom
{

  public class ScreenManager : MonoBehaviour
  {

    [SerializeField] GameObject m_ScreenPrefab;
    [SerializeField] TrackerManager m_TrackerManager;

    List<GameObject> m_ScreenList = new List<GameObject>();
    List<GameObject> m_AttachedTrackerList = new List<GameObject>();

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Return))
      {
        if (m_TrackerManager.current)
        {
          bool hasAttached = false;
          foreach (GameObject tracker in m_AttachedTrackerList)
            if (tracker.Equals(m_TrackerManager.current))
              hasAttached = true;
          if (hasAttached)
          {
            // TODO: 確認モーダル（二重だけどいいですか？）OKならAdd
            Debug.Log("### 重複OK?");
            // Add(new Vector3(1, .5f, .01f), m_TrackerManager.current, Vector3.zero);
          }
          else
          {
            m_AttachedTrackerList.Add(m_TrackerManager.current);
            Add(new Vector3(1, .5f, .01f), m_TrackerManager.current, Vector3.zero);
          }
        }
      }
    }

    public void Add(Vector3 size, GameObject tracker, Vector3 offset)
    {
      GameObject screen = Instantiate(m_ScreenPrefab, transform);
      screen.GetComponent<Screen>().Init(size, tracker, offset, m_ScreenList.Count + 1/* NOTE: 2から */);
      m_ScreenList.Add(screen);
    }

    public void Remove(GameObject target)
    {
      foreach (GameObject screen in m_ScreenList)
      {
        if (screen.Equals(target))
        {
          m_ScreenList.Remove(screen);
          Destroy(screen);
        }
      }
    }

  }
}