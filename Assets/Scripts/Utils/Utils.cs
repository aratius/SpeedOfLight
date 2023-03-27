using UnityEngine.Events;

namespace Unity.Custom
{

  public delegate float Equation(float v);

  public static class Utils
  {

    public static Equation LinearEquationThrough2Points(
      float x1,
      float x2,
      float y1,
      float y2
    )
    {
      return (float x) =>
      {
        // return (y2 - y1) / (x2 - x1) * x + (x2 * y1 - x1 * y2) / (x2 - x1);
        return (y2 - y1) / (x2 - x1) * x + (x2 * y1 - x1 * y2) / (x2 - x1);
      };
    }

  }

}