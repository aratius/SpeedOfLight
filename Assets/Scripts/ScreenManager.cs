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
    [SerializeField] ScreenInfoUI m_ScreenInfo;

    List<GameObject> m_ScreenList = new List<GameObject>();
    List<GameObject> m_AttachedTrackerList = new List<GameObject>();

    void Start()
    {
      m_TrackerManager.onChangeSelectedTracker.AddListener(OnChangeSelectedTracker);
      m_ScreenInfo.onSubmit.AddListener(AddScreen);
      m_ScreenInfo.onChange.AddListener(OnChangeInfo);
    }

    public void Add(string key, Vector3 size, GameObject tracker, Vector3 offset)
    {
      GameObject screen = Instantiate(m_ScreenPrefab, transform);
      screen.GetComponent<Screen>().Init(key, size, tracker, offset, m_ScreenList.Count + 1/* NOTE: 2から */);
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

    void AddScreen()
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
          string k = m_ScreenInfo.name;
          float w = m_ScreenInfo.width;
          float h = m_ScreenInfo.height;
          float d = m_ScreenInfo.depth;
          float x = m_ScreenInfo.offsetX;
          float y = m_ScreenInfo.offsetY;
          float z = m_ScreenInfo.offsetZ;
          Add(k, new Vector3(w, h, d), m_TrackerManager.current, new Vector3(x, y, z));
          m_AttachedTrackerList.Add(m_TrackerManager.current);
          m_ScreenInfo.hasAttached = true;
          ScreenInfoUI.caches.Add(k, new ScreenInfo(k, w, h, d, x, y, z));
        }
      }
    }

    void OnChangeSelectedTracker(string? name)
    {
      if (name == null)
      {
        m_ScreenInfo.Empty();
        m_ScreenInfo.name = "No Select";
        return;
      }

      if (ScreenInfoUI.caches.ContainsKey(name))
      {
        // TODO: ScreenInfoの値を復元
        Debug.Log("### Restore");
        m_ScreenInfo.Copy(ScreenInfoUI.caches[name]);
      }
      else
      {
        m_ScreenInfo.name = name;
      }
    }

    void OnChangeInfo()
    {
      if (m_ScreenInfo.hasAttached)
      {
        // TODO: Screenの位置を更新
        foreach (GameObject screen in m_ScreenList)
        {
          Screen screenScript = screen.GetComponent<Screen>();
          if (screenScript.key == m_ScreenInfo.name)
          {
            string k = m_ScreenInfo.name;
            float w = m_ScreenInfo.width;
            float h = m_ScreenInfo.height;
            float d = m_ScreenInfo.depth;
            float x = m_ScreenInfo.offsetX;
            float y = m_ScreenInfo.offsetY;
            float z = m_ScreenInfo.offsetZ;
            screenScript.UpdateInfo(new Vector3(w, h, d), new Vector3(x, y, z));
            ScreenInfoUI.caches[k] = new ScreenInfo(k, w, h, d, x, y, z);
          }
        }
      }
    }

  }
}