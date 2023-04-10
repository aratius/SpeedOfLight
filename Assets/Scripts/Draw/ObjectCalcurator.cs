using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Custom;

public class ObjectCalcurator : MonoBehaviour
{
  [SerializeField] ScreenManager m_Screen;
  [SerializeField] TrackerManager m_Tracker;
  [SerializeField] GameObject m_FloorPlacerPrefab;
  private List<GameObject> m_FloorPlacer = new List<GameObject>();

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
