using UnityEngine;
using DG.Tweening;

public class BallPlanet : MonoBehaviour
{

    Rigidbody m_Rigid;
    Box m_Target;

    void Start()
    {
        float scale = Random.Range(.4f, .6f);
        transform.DOScale(Vector3.one * scale, 1f).SetEase(Ease.OutElastic);
        m_Rigid = GetComponent<Rigidbody>();
        m_Rigid.useGravity = false;
        m_Rigid.mass = scale * 100f;
        // GetComponent<MeshRenderer>().material.color = Color.red;
    }

    void FixedUpdate()
    {
        if(m_Target == null) return;
        Vector3 direction = Vector3.Normalize(m_Target.center - transform.position);
        float distance = Vector3.Distance(transform.position, m_Target.center);
        m_Rigid.AddForce(direction * distance * .5f, ForceMode.Impulse);
        m_Rigid.velocity = m_Rigid.velocity * .99f;
    }

    public void SetTarget(Box box)
    {
        m_Target = box;
    }
    
}