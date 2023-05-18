using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleKey
{
    Collision
}

[System.Serializable]
public struct Particle
{
    public ParticleKey key;
    public GameObject go;
}

public class EffectController : SingletonMonoBehaviour<EffectController>
{

    [SerializeField]
    List<Particle> m_Particles;

    public void Occour(ParticleKey key, Vector3 pos)
    {
        foreach(Particle particle in m_Particles)
        {
            if(particle.key == key)
            {
                GameObject go = Instantiate(particle.go, transform);
                go.transform.position = pos;
            }
        }
    }
}
