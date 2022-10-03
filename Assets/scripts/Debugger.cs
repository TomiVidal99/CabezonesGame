using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
  // utilty script to debug stuff

  // shows a vector in the 2D scene
  public void DisplayVector(Vector3 origin, Vector3 vector, Color color)
  {
    // Debug.Log($"Display vector 3d: ({vector.x}, {vector.y}, {vector.z})");
    DisplayVector(new Vector2(origin.x, origin.y), new Vector2(vector.x, vector.y), color);
  }
  public void DisplayVector(Vector2 origin, Vector2 vector, Color color)
  {
    // Debug.Log($"Display vector 2d: ({vector.x}, {vector.y})");
    Debug.DrawLine(origin, vector, color);
  }

  public void ShowVector(Vector3 vector, string name = "vector")
  {
    Debug.Log($"{name} = ({vector.x}, {vector.y}, {vector.z})");
  }
  public void ShowVector(Vector2 vector, string name = "vector")
  {
    Debug.Log($"{name} = ({vector.x}, {vector.y})");
  }

}
