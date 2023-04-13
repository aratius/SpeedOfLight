using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

  [SerializeField] GameObject m_Prefab;
  [SerializeField] float m_Interval = 1f;

  void Start()
  {
    Create();
  }

  void Create()
  {
    GameObject go = Instantiate(m_Prefab, transform);
    go.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    float scale = Random.Range(.1f, .5f);
    go.transform.localScale = Vector3.one * scale;
    Invoke("Create", m_Interval);
  }
}
