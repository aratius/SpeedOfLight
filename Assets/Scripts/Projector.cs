using UnityEngine;
using OscJack;
using Cysharp.Threading.Tasks;

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
    private float _slitScanEnable = 1;

    void Start()
    {
      Shader.SetGlobalFloat("_DMin", _distMin);
      Shader.SetGlobalFloat("_DMax", _distMax);
      Shader.SetGlobalFloat("_Enable", _slitScanEnable);
      _slitScan = new SlitScan(_material);

      OscReceiver.Instance.AddCallback("/projector:tx", (string address, OscDataHandle data) => ApplyInfo("tx", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:ty", (string address, OscDataHandle data) => ApplyInfo("ty", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:tz", (string address, OscDataHandle data) => ApplyInfo("tz", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:rx", (string address, OscDataHandle data) => ApplyInfo("rx", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:ry", (string address, OscDataHandle data) => ApplyInfo("ry", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:rz", (string address, OscDataHandle data) => ApplyInfo("rz", data.GetElementAsFloat(0)));
      OscReceiver.Instance.AddCallback("/projector:fov", (string address, OscDataHandle data) => ApplyInfo("fov", data.GetElementAsFloat(0)));
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

    public void ToggleSlitScan()
    {
      _slitScanEnable++;
      Shader.SetGlobalFloat("_Enable", _slitScanEnable % 2);
    }

    async void ApplyInfo(string type, float value)
    {
      await UniTask.WaitForFixedUpdate();
      Vector3 p = transform.localPosition;
      Vector3 r = transform.localEulerAngles;
      if (type == "tx") p.x = value;
      if (type == "ty") p.y = value;
      if (type == "tz") p.z = value;
      if (type == "rx") r.x = value;
      if (type == "ry") r.y = value;
      if (type == "rz") r.z = value;
      if (type == "fov") _fieldOfView = value;

      transform.localPosition = p;
      transform.localEulerAngles = r;
    }

  }
}