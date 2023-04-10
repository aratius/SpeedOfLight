using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{

  [SerializeField] GameObject m_Prefab;

  void Start()
  {
    Create();
  }

  void Create()
  {
    GameObject go = Instantiate(m_Prefab, transform);
    go.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    Invoke("Create", 1f);
  }
}
