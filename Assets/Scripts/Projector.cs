using UnityEngine;

namespace Unity.Custom
{
  [ExecuteAlways]
  [DefaultExecutionOrder(0)] // ExecutionOrderは適宜設定
  public class Projector : MonoBehaviour
  {
    [SerializeField, Range(0.0001f, 179)]
    private float _fieldOfView = 60;
    [SerializeField, Range(0.2f, 5.0f)]
    private float _aspect = 1.0f;
    [SerializeField, Range(0.0001f, 1000.0f)]
    private float _nearClipPlane = 0.01f;
    [SerializeField, Range(0.0001f, 1000.0f)]
    private float _farClipPlane = 100.0f;
    [SerializeField]
    private bool _orthographic = false;
    [SerializeField]
    private float _orthographicSize = 1.0f;
    [SerializeField, Range(0.0001f, 10f)]
    private float _distMin = 2f;
    [SerializeField, Range(0.0001f, 10f)]
    private float _distMax = 4f;
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Texture _videoTexture;
    private SlitScan _slitScan;

    void Start()
    {
      Shader.SetGlobalFloat("_DMin", _distMin);
      Shader.SetGlobalFloat("_DMax", _distMax);
      _slitScan = new SlitScan(_material);
    }

    // とりあえず今回はLateUpdateで更新
    private void LateUpdate()
    {
      if (_slitScan == null) return;

      var viewMatrix = Matrix4x4.Scale(new Vector3(1, 1, -1)) * transform.worldToLocalMatrix;
      Matrix4x4 projectionMatrix;
      if (_orthographic)
      {
        var orthographicWidth = _orthographicSize * _aspect;
        projectionMatrix = Matrix4x4.Ortho(-orthographicWidth, orthographicWidth, -_orthographicSize, _orthographicSize, _nearClipPlane, _farClipPlane);
      }
      else
      {
        var camera = GetComponent<Camera>();
        projectionMatrix = Matrix4x4.Perspective(_fieldOfView, _aspect, _nearClipPlane, _farClipPlane);
      }
      projectionMatrix = GL.GetGPUProjectionMatrix(projectionMatrix, true);
      Shader.SetGlobalMatrix("_ProjectorMatrixVP", projectionMatrix * viewMatrix);
      // プロジェクターの位置を渡す
      // _ObjectSpaceLightPosのような感じでwに0が入っていたらOrthographicの前方方向とみなす
      var projectorPos = Vector4.zero;
      projectorPos = _orthographic ? transform.forward : transform.position;
      projectorPos.w = _orthographic ? 0 : 1;
      Shader.SetGlobalVector("_ProjectorPos", projectorPos);

      _slitScan.Update(_videoTexture);
    }

    private void OnDrawGizmos()
    {
      var gizmosMatrix = Gizmos.matrix;
      Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

      if (_orthographic)
      {
        var orthographicWidth = _orthographicSize * _aspect;
        var length = _farClipPlane - _nearClipPlane;
        var start = _nearClipPlane + length / 2;
        Gizmos.DrawWireCube(Vector3.forward * start, new Vector3(orthographicWidth * 2, _orthographicSize * 2, length));
      }
      else
      {
        Gizmos.DrawFrustum(Vector3.zero, _fieldOfView, _farClipPlane, _nearClipPlane, _aspect);
      }

      Gizmos.matrix = gizmosMatrix;
    }
  }
}