using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Debugger : MonoBehaviour
{
  // utilty script to debug stuff

  // shows a vector in the 2D scene
  public void DisplayVector(Vector3 origin, Vector3 vector)
  {
    Debug.Log($"Display vector 3d: ({vector.x}, {vector.y}, {vector.z})");
    //DisplayVector(new Vector2(origin.x, origin.y), new Vector2(vector.x, vector.y));
  }
  public void DisplayVector(Vector2 origin, Vector2 vector)
  {
    Debug.Log($"Display vector 2d: ({vector.x}, {vector.y})");
    Debug.DrawLine(origin, vector);
  }

}
