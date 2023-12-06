using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Unity.Custom;

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

    // MeshRenderer r = GetComponent<MeshRenderer>();
    // r.material = new Material(r.material.shader);
    // Color[] colors = {
    //   new Color(249f / 255f, 61f / 255f, 64f / 255f, 1),
    //   new Color(9f / 255f, 166f / 255f, 208f / 255f, 1),
    //   new Color(2f / 255f, 31f / 255f, 161f / 255f, 1),
    //   new Color(250f / 255f, 186f / 255f, 75f / 255f, 1)
    // };
    // Color[] colors = {
    //   new Color(100f / 255f, 100f / 255f, 100f / 255f, 1),
    //   new Color(75f / 255f, 75f / 255f, 75f / 255f, 1),
    //   new Color(50f / 255f, 50f / 255f, 50f / 255f, 1),
    // };
    // Color[] colors = {
    //   new Color(0f / 255f, 0f / 255f, 100f / 255f, 1),
    //   new Color(0f / 255f, 0f / 255f, 75f / 255f, 1),
    //   new Color(0f / 255f, 0f / 255f, 50f / 255f, 1),
    // };
    // Color c = colors[(int)Mathf.Floor(Random.Range(0, colors.Length))];
    // r.material.SetColor("_MainColor", c);
  }


  void OnCollisionEnter(Collision collisionInfo)
  {
    if (collisionInfo.gameObject.tag == "Destroyer")
    {
      m_OnCollideDestroyer.Invoke(gameObject);
    }
    else if ( m_Rigid && m_Rigid.velocity.magnitude > 1f)
    {
      // OSCトリガー
      int size = (int)Mathf.Clamp(transform.localScale.x * 10f, 0f, 10f);
      int vel = (int)Mathf.Clamp(m_Rigid.velocity.magnitude * 3f, 0f, 10f);
      OscSender.Instance.Send("/sound", 0, size, vel);

      // エフェクト
      Vector3 collisionDirection = Vector3.Normalize(m_Rigid.velocity);
      Vector3 collistionPoint = transform.position + Vector3.Scale(collisionDirection, transform.localScale) * .5f;
      EffectController.Instance.Occour(ParticleKey.Collision, collistionPoint);
    }
  }


}