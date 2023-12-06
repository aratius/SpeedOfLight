using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Unity.Custom;
using OscJack;

public enum AnimationMode
{
  Earth,
  Universe
}

[System.Serializable]
public struct MaterialSet {
  public Material sphere;
  public Material box;
}

// TODO: 万有引力の実装
// ランダムに惑星を初期化
// TODO: 万有引力と重力ときりかえ
public class BallManager : MonoBehaviour
{

  [SerializeField] GameObject m_BallSpherePrefab;
  [SerializeField] GameObject m_BallBoxPrefab;
  [SerializeField] MaterialSet[] m_MaterialSets;
  [SerializeField] BoxManager m_Boxes;
  [SerializeField] GameObject m_Destroyer;
  [SerializeField] float verticalMin = -1f;
  [SerializeField] float verticalMax = 2f;

  List<GameObject> m_Balls = new List<GameObject>();
  List<BallPlanet> m_Planets = new List<BallPlanet>();
  List<BallSatellite> m_Satellites = new List<BallSatellite>();
  AnimationMode m_Mode = AnimationMode.Earth;

  void Start()
  {
    CreateLoop();

    OscReceiver.Instance.AddCallback("/trigger", async (string address, OscDataHandle data) =>
    {
      await UniTask.WaitForFixedUpdate();
      if (data.GetElementAsInt(0) == 0) Earth();
      else Universe();
    });
  }

  void CreateLoop()
  {
    if (m_Mode == AnimationMode.Earth) Create();
    Invoke("CreateLoop", .2f);
  }

  void Create()
  {
    bool isBox = Random.Range(0, 1f) < 0.3f;
    GameObject prefabPickedUp = isBox ? m_BallBoxPrefab : m_BallSpherePrefab;
    GameObject go = Instantiate(prefabPickedUp, transform);
    go.transform.localPosition = new Vector3(Random.Range(-3f, 3f), Random.Range(verticalMin, verticalMax), 0);
    go.transform.localScale = Vector3.one * Random.Range(.3f, .8f);
    int index = (int)(Time.time / 30f) % m_MaterialSets.Length;
    Debug.Log(index);
    go.GetComponent<MeshRenderer>().material = isBox ? m_MaterialSets[index].box : m_MaterialSets[index].sphere;
    // go.transform.localScale = Vector3.one * Random.Range(.2f, .4f);
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
