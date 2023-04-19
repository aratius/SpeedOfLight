using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Ball : MonoBehaviour
{

  UnityEvent<GameObject> m_OnCollideDestroyer = new UnityEvent<GameObject>();
  Rigidbody m_Rigid;

  public UnityEvent<GameObject> OnCollideDestroyer => m_OnCollideDestroyer;

  void Start()
  {
    m_Rigid = GetComponent<Rigidbody>();
    m_Rigid.useGravity = true;
    m_Rigid.mass = transform.localScale.x * 3f;
  }


  void OnCollisionEnter(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Destroyer") m_OnCollideDestroyer.Invoke(gameObject);
  }


}