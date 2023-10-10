using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationMode
{
  Earth,
  Universe
}

// TODO: 万有引力の実装
// ランダムに惑星を初期化
// TODO: 万有引力と重力ときりかえ
public class BallManager : MonoBehaviour
{

  [SerializeField] GameObject m_Prefab;
  [SerializeField] BoxManager m_Boxes;
  [SerializeField] GameObject m_Destroyer;

  List<GameObject> m_Balls = new List<GameObject>();
  List<BallPlanet> m_Planets = new List<BallPlanet>();
  List<BallSatellite> m_Satellites = new List<BallSatellite>();
  AnimationMode m_Mode = AnimationMode.Earth;

  void Start()
  {
    CreateLoop();
  }

  void CreateLoop()
  {
    if (m_Mode == AnimationMode.Earth) Create();
    Invoke("CreateLoop", .2f);
  }

  void Create()
  {
    GameObject go = Instantiate(m_Prefab, transform);
    go.transform.localPosition = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
    go.transform.localScale = Vector3.one * Random.Range(.1f, .4f);
    m_Balls.Add(go);
    Ball ballScript = go.AddComponent<Ball>();
    ballScript.OnCollideDestroyer.AddListener(OnCollideDestroyer);
  }

  void FixedUpdate()
  {
    if (Input.GetKeyDown(KeyCode.U)) Universe();
    else if (Input.GetKeyDown(KeyCode.E)) Earth();
  }

  void Earth()
  {
    if (m_Mode == AnimationMode.Earth) return;
    m_Mode = AnimationMode.Earth;
    foreach (Box box in m_Boxes.boxes)
    {
      box.EnableFloor();
    }
    foreach (BallPlanet ball in m_Planets)
    {
      Destroy(ball.GetComponent<BallPlanet>());
    }
    m_Planets.Clear();
    foreach (BallSatellite ball in m_Satellites)
    {
      Destroy(ball.GetComponent<BallSatellite>());
    }
    m_Satellites.Clear();
    foreach (GameObject ball in m_Balls)
    {
      Ball ballScript = ball.AddComponent<Ball>();
      ballScript.OnCollideDestroyer.AddListener(OnCollideDestroyer);
    }
    m_Destroyer.SetActive(true);
  }

  void Universe()
  {
    if (m_Mode == AnimationMode.Universe) return;
    m_Mode = AnimationMode.Universe;
    foreach (GameObject ball in m_Balls)
    {
      Ball ballScript = ball.GetComponent<Ball>();
      ballScript.OnCollideDestroyer.RemoveListener(OnCollideDestroyer);
      Destroy(ballScript);
    }
    foreach (Box box in m_Boxes.boxes)
    {
      box.DisableFloor();
      Vector3 boxCenter = box.center;
      while (m_Balls.Count < 5) Create();
      GameObject? nearestBall = null;
      float nearestDist = 9999f;
      foreach (GameObject ball in m_Balls)
      {
        if (ball.GetComponent<BallPlanet>() != null) continue;
        float dist = Vector3.Distance(ball.transform.position, boxCenter);
        if (dist < nearestDist)
        {
          nearestBall = ball;
          nearestDist = dist;
        }
      }
      if (nearestBall != null)
      {
        BallPlanet planet = nearestBall.AddComponent<BallPlanet>();
        planet.SetTarget(box);
        m_Planets.Add(planet);
      }
      m_Destroyer.SetActive(false);
    }

    foreach (GameObject ball in m_Balls)
    {
      if (ball.GetComponent<BallPlanet>() == null)
      {
        BallSatellite satellite = ball.AddComponent<BallSatellite>();
        satellite.SetPlanets(m_Planets);
      }
    }

  }

  void OnCollideDestroyer(GameObject ball)
  {
    m_Balls.Remove(ball);
    Destroy(ball);
  }

}