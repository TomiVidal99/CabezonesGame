using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  HUDController _hud;

  void Start()
  {
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
  }

  // sets the ball to the start
  void ResetBallPosition()
  {
    Transform ballTransform = gameObject.GetComponent<Transform>();

    Vector2 newPosition = new Vector2(0, 0);
    ballTransform.position = newPosition;

  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.gameObject.tag == "Player")
    {
      Debug.Log("Hit the goal");
    }
    if (collision.gameObject.tag == "Pole Goal Left")
    {
      _hud.UpdateScore(1, 0);
      ResetBallPosition();
    }
    if (collision.gameObject.tag == "Pole Goal Right")
    {
      _hud.UpdateScore(0, 1);
      ResetBallPosition();
    }
  }

}
