using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{

    Rigidbody m_Rigid;

    void Start()
    {
        m_Rigid = GetComponent<Rigidbody>();
        m_Rigid.useGravity = true;
        m_Rigid.mass = transform.localScale.x * 3f;
    }
    
}