using UnityEngine;

public class Box : MonoBehaviour
{

  [SerializeField] GameObject m_Floor;
  Camera m_CaptureCamera;
  Camera m_DrawCamera;
  GameObject m_Target;
  Vector3 m_LowestPoint;
  Vector3 m_SidePoint;
  Vector3 m_CenterPoint;

  public void Init(GameObject target, Camera captureCamera, Camera drawCamera)
  {
    m_Target = target;
    m_CaptureCamera = captureCamera;
    m_DrawCamera = drawCamera;
    Calculate();
  }

  void Calculate()
  {
    // スクリーン座標に変換
    Vector3 targetScale = m_Target.transform.localScale;
    Vector3[] targetVertices = GetBoxVertices(1f, 1f, 1f);
    Vector2[] targetVerticeScreenCoordinates = new Vector2[targetVertices.Length];
    for (int i = 0; i < targetVertices.Length; i++)
    {
      // NOTE: TransformPoint関数自体がScaleの影響受けるからGetBoxVerticesの引数は1で良い！
      Vector3 worldPosition = m_Target.transform.TransformPoint(targetVertices[i]);
      Vector3 viewportPosition = m_CaptureCamera.WorldToViewportPoint(worldPosition);
      targetVerticeScreenCoordinates[i] = new Vector2(
        viewportPosition.x * m_DrawCamera.pixelWidth,
        viewportPosition.y * m_DrawCamera.pixelHeight
      );
    }

    // 一番下左右のpointを抽出
    // 中心点を算出
    int lowestIndex = 0;
    Vector2 lowestVerticeScreenPoint = targetVerticeScreenCoordinates[lowestIndex];
    Vector2 rightVerticeScreenPoint = targetVerticeScreenCoordinates[lowestIndex];
    Vector2 leftVerticeScreenPoint = targetVerticeScreenCoordinates[lowestIndex];
    Vector2 sum = Vector2.zero;
    // 下だけ先に
    for (int i = 0; i < targetVerticeScreenCoordinates.Length; i++)
    {
      Vector2 coord = targetVerticeScreenCoordinates[i];

      if (coord.y < lowestVerticeScreenPoint.y)
      {
        lowestVerticeScreenPoint = coord;
        lowestIndex = i;
      }
      sum += coord;
    }
    // 中心点を保存しておく
    m_CenterPoint = sum / targetVerticeScreenCoordinates.Length;

    // 次に左右の端
    for (int i = 0; i < targetVerticeScreenCoordinates.Length; i++)
    {
      if (i == lowestIndex) continue;
      Vector3 lowestVertices = targetVertices[lowestIndex];
      Vector3 vertices = targetVertices[i];
      // 対角を除外
      if (
        Mathf.Sign(lowestVertices.x) != Mathf.Sign(vertices.x) &&
        Mathf.Sign(lowestVertices.y) != Mathf.Sign(vertices.y)
      ) continue;

      Vector2 coord = targetVerticeScreenCoordinates[i];
      if (coord.x > rightVerticeScreenPoint.x) rightVerticeScreenPoint = coord;
      if (coord.x < leftVerticeScreenPoint.x) leftVerticeScreenPoint = coord;
      sum += coord;
    }

    // Floorに座標をapply
    float distXToRightSide = Mathf.Abs(rightVerticeScreenPoint.x - lowestVerticeScreenPoint.x);
    float distXToLeftSide = Mathf.Abs(leftVerticeScreenPoint.x - lowestVerticeScreenPoint.x);
    Vector2 floorAntherPointScreenPoint = distXToRightSide > distXToLeftSide ? rightVerticeScreenPoint : leftVerticeScreenPoint;
    Vector3 floorAntherPoint = m_DrawCamera.ScreenToWorldPoint(new Vector3(floorAntherPointScreenPoint.x, floorAntherPointScreenPoint.y, 10f));
    Vector3 floorLowestPoint = m_DrawCamera.ScreenToWorldPoint(new Vector3(lowestVerticeScreenPoint.x, lowestVerticeScreenPoint.y, 10f));
    Vector3 floorPoint = (floorAntherPoint + floorLowestPoint) / 2f;

    m_Floor.transform.position = floorPoint;
    m_Floor.transform.localScale = new Vector3((floorLowestPoint - floorAntherPoint).magnitude, .1f, 1f);
    float rotZ = Mathf.Atan2(lowestVerticeScreenPoint.y - floorAntherPointScreenPoint.y, lowestVerticeScreenPoint.x - floorAntherPointScreenPoint.x);
    Vector3 floorEulerAngles = m_Floor.transform.eulerAngles;
    m_Floor.transform.eulerAngles = new Vector3(floorEulerAngles.x, floorEulerAngles.y, rotZ * 180f / Mathf.PI);

    Invoke("Calculate", 1f);
  }

  Vector3[] GetBoxVertices(float sizeX, float sizeY, float sizeZ)
  {
    Vector3[] vertices = new Vector3[8];

    float halfSizeX = sizeX * 0.5f;
    float halfSizeY = sizeY * 0.5f;
    float halfSizeZ = sizeZ * 0.5f;

    vertices[0] = new Vector3(-halfSizeX, -halfSizeY, -halfSizeZ);
    vertices[1] = new Vector3(halfSizeX, -halfSizeY, -halfSizeZ);
    vertices[2] = new Vector3(halfSizeX, halfSizeY, -halfSizeZ);
    vertices[3] = new Vector3(-halfSizeX, halfSizeY, -halfSizeZ);
    vertices[4] = new Vector3(-halfSizeX, -halfSizeY, halfSizeZ);
    vertices[5] = new Vector3(halfSizeX, -halfSizeY, halfSizeZ);
    vertices[6] = new Vector3(halfSizeX, halfSizeY, halfSizeZ);
    vertices[7] = new Vector3(-halfSizeX, halfSizeY, halfSizeZ);

    return vertices;
  }

  Vector2[] debugScreenPositions = new Vector2[0];
  void OnDrawGizmos()
  {
    foreach (Vector3 p in debugScreenPositions)
    {
      Gizmos.DrawSphere(m_DrawCamera.ScreenToWorldPoint(new Vector3(p.x, p.y, 10f)), .05f);
    }

  }

}