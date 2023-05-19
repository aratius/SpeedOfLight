using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BallSatellite : MonoBehaviour
{

  Rigidbody m_Rigid;
  List<BallPlanet> m_Planets;

  void Start()
  {
    float scale = Random.Range(.05f, .1f);
    transform.DOScale(Vector3.one * scale, 1f).SetEase(Ease.OutElastic);
    m_Rigid = GetComponent<Rigidbody>();
    m_Rigid.useGravity = false;
    m_Rigid.mass = scale;
  }

  void FixedUpdate()
  {
    if (m_Planets == null) return;
    foreach (BallPlanet planet in m_Planets)
    {
      // if(planet == null)
      Vector3 direction = Vector3.Normalize(planet.transform.position - transform.position);
      float distance = Vector3.Distance(transform.position, planet.transform.position);
      m_Rigid.AddForce(direction * distance * .0001f, ForceMode.Impulse);
      m_Rigid.velocity = m_Rigid.velocity * .999f;
    }

    float minSpeed = 1f;
    float maxSpeed = 3f;
    if (m_Rigid.velocity.magnitude < minSpeed)
    {
      m_Rigid.AddForce(Vector3.Normalize(m_Rigid.velocity) * .01f, ForceMode.Impulse);
    }
    else if (m_Rigid.velocity.magnitude > maxSpeed)
    {
      m_Rigid.AddForce(-Vector3.Normalize(m_Rigid.velocity) * .01f, ForceMode.Impulse);
    }

  }

  void OnCollisionEnter(Collision collisionInfo)
  {
    GameObject other = collisionInfo.gameObject;
    if (other.tag == "Ball")
    {
      int kind = other.GetComponent<BallSatellite>() == null ? 0 : 1;  // 惑星との衝突は0（低い音）, 衛星同士は1（高い音）
      int size = (int)Mathf.Clamp(transform.localScale.x * 10f, 0f, 10f);
      int vel = (int)Mathf.Clamp(m_Rigid.velocity.magnitude * 3f, 0f, 10f);
      OscSender.Instance.Send("/sound", kind, size, vel);
    }
  }

  public void SetPlanets(List<BallPlanet> planets)
  {
    m_Planets = planets;
  }

}