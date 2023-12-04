using UnityEngine;

[ExecuteInEditMode]
public class PostEffect : MonoBehaviour
{
    [SerializeField] private Material filter;
    [SerializeField] private bool active = true;

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if(active) Graphics.Blit(src,dest,filter);
        else Graphics.Blit(src,dest);
    }
}
