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
        transform.DOScale(Vector3.one * scale, .8f).SetEase(Ease.OutElastic);
        m_Rigid = GetComponent<Rigidbody>();
        m_Rigid.useGravity = false;
        m_Rigid.mass = scale;
    }

    void FixedUpdate()
    {
        if(m_Planets == null) return;
        foreach(BallPlanet planet in m_Planets)
        {
            // if(planet == null)
            Vector3 direction = Vector3.Normalize(planet.transform.position - transform.position);
            float distance = Vector3.Distance(transform.position, planet.transform.position);
            m_Rigid.AddForce(direction * distance * .0001f, ForceMode.Impulse);
            m_Rigid.velocity = m_Rigid.velocity * .999f;
        }
        
        float minSpeed = 1f;
        if(m_Rigid.velocity.magnitude < minSpeed) 
        {
            m_Rigid.AddForce(Vector3.Normalize(m_Rigid.velocity) * .01f, ForceMode.Impulse);
        }
    }

    public void SetPlanets(List<BallPlanet> planets)
    {
        m_Planets = planets;
    }
    
}