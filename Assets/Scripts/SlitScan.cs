using UnityEngine;

public class SlitScan
{

  const int Resolution = 256;

  Material m_Material;
  Texture2DArray m_Buffer;

  public SlitScan(Material material)
  {
    m_Material = material;
    m_Buffer = new Texture2DArray(512, 512, Resolution, TextureFormat.RGBA32, false);
    m_Buffer.filterMode = FilterMode.Bilinear;
    m_Buffer.wrapMode = TextureWrapMode.Clamp;
  }

  public void Update(Texture texture)
  {
    int frame = Time.frameCount & (Resolution - 1);

    var ac = RenderTexture.active;
    Graphics.ConvertTexture(texture, 0, m_Buffer, frame);
    RenderTexture.active = ac;

    m_Material.SetTexture("_BufferTex", m_Buffer);
    // m_Material.SetFloat("_Axis", _effectType == 0 ? 1 : 0);
    // m_Material.SetFloat("_VFlip", _webcam.videoVerticallyMirrored ? 1 : 0);
    m_Material.SetInt("_Frame", frame);
    m_Material.SetPass(0);
    Graphics.DrawProceduralNow(MeshTopology.Triangles, 6, 1);
  }

}