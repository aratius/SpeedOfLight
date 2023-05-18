using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeVertexScreenCoordinates : MonoBehaviour
{
  public Camera mainCamera;
  public Camera drawCamera;
  public GameObject parent;
  public GameObject prefab;
  private Vector3[] localVertices;
  private Vector2[] screenCoordinates;

  void Start()
  {
    localVertices = GetCubeVertices();
    screenCoordinates = new Vector2[localVertices.Length];
    CalculateScreenCoordinates();
    PrintScreenCoordinates();
  }

  void Update()
  {
  }

  Vector3[] GetCubeVertices()
  {
    Vector3[] vertices = new Vector3[8];

    float halfSize = 0.5f;
    vertices[0] = new Vector3(-halfSize, -halfSize, -halfSize);
    vertices[1] = new Vector3(halfSize, -halfSize, -halfSize);
    vertices[2] = new Vector3(halfSize, halfSize, -halfSize);
    vertices[3] = new Vector3(-halfSize, halfSize, -halfSize);
    vertices[4] = new Vector3(-halfSize, -halfSize, halfSize);
    vertices[5] = new Vector3(halfSize, -halfSize, halfSize);
    vertices[6] = new Vector3(halfSize, halfSize, halfSize);
    vertices[7] = new Vector3(-halfSize, halfSize, halfSize);

    return vertices;
  }

  void CalculateScreenCoordinates()
  {
    for (int i = 0; i < localVertices.Length; i++)
    {
      Vector3 worldPosition = transform.TransformPoint(localVertices[i]);
      Vector3 viewportPosition = mainCamera.WorldToViewportPoint(worldPosition);
      screenCoordinates[i] = new Vector2(
          viewportPosition.x * mainCamera.pixelWidth,
          viewportPosition.y * mainCamera.pixelHeight);
    }
  }

  void PrintScreenCoordinates()
  {
    Vector2 lowest = screenCoordinates[0];
    Vector2 right = screenCoordinates[0];
    Vector2 left = screenCoordinates[0];
    for (int i = 0; i < screenCoordinates.Length; i++)
    {
      Vector2 coord = screenCoordinates[i];
      Debug.Log($"Vertex {i + 1}: {screenCoordinates[i]}");

      if (coord.y < lowest.y) lowest = coord;
      if (coord.x > right.x) right = coord;
      if (coord.x < left.x) left = coord;
    }
    Debug.Log(lowest);
    Debug.Log(right);
    Debug.Log(left);
    Vector2 other = Mathf.Abs(right.x - lowest.x) > Mathf.Abs(left.x - lowest.x) ? right : left;
    Debug.Log(other);
    GameObject go = Instantiate(prefab, parent.transform);
    Vector3 half = (lowest + other) / 2f;
    go.transform.position = drawCamera.ScreenToWorldPoint(new Vector3(half.x, half.y, 10f));
    go.transform.localScale = new Vector3((drawCamera.ScreenToWorldPoint(lowest) - drawCamera.ScreenToWorldPoint(other)).magnitude, .2f, .2f);
    float rotY = Mathf.Atan2(lowest.y - other.y, lowest.x - other.x);
    go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, go.transform.localEulerAngles.y, rotY * 180f / Mathf.PI);
  }
}