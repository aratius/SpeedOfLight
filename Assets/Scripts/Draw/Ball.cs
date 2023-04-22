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
    else if(collisionInfo.gameObject.tag == "FrameFloor") 
    {
      Vector3 collisionDirection = Vector3.Normalize(collisionInfo.gameObject.transform.position - transform.position);
      Vector3 collistionPoint = transform.position + Vector3.Scale(collisionDirection, transform.localScale) * .5f;
      EffectController.Instance.Occour(ParticleKey.Collision, collistionPoint);
    }
  }


}