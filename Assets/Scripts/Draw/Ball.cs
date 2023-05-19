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
    if (collisionInfo.gameObject.tag == "Destroyer")
    {
      m_OnCollideDestroyer.Invoke(gameObject);
    }
    else if (collisionInfo.gameObject.tag == "Ball")
    {
      int size = (int)Mathf.Clamp(transform.localScale.x * 10f, 0f, 10f);
      int vel = (int)Mathf.Clamp(m_Rigid.velocity.magnitude * 3f, 0f, 10f);
      OscSender.Instance.Send("/sound", 0, size, vel);
    }
  }


}