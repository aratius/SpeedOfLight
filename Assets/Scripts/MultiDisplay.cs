using UnityEngine;

public class MultiDisplay : MonoBehaviour
{
  [SerializeField, Range(1, 8)]
  private int m_useDisplayCount = 4;

  void Awake()
  {
    int count = Mathf.Min(Display.displays.Length, m_useDisplayCount);

    for (int i = 0; i < count; i++)
    {
      Display.displays[i].Activate();
    }
  }
}