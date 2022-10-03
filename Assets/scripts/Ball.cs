using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  HUDController _hud;
  Main _main;
  Rigidbody2D _ballRB;

  bool _isGoalAnimationRunning = false;
  DateTime _goalAnimationTimeDiff;

  const int _ballResetSeconds = 1;

  void Start()
  {
    _main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
    _ballRB = gameObject.GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    if (_isGoalAnimationRunning)
    {
      HandleScoredAnimation();
    }
  }

  // returns the current position of the ball
  public Vector3 GetCurrentPosition()
  {
    Vector3 currentPos = gameObject.transform.position;
    // Debug.Log($"({currentPos.x}, {currentPos.y})");
    return currentPos;
  }

  // returns where it's going/facing
  // TODO: currently not used, maybe i will later use it
  public Vector3 GetFacingDirection()
  {
    Vector3 currentVelocity = gameObject.GetComponent<Rigidbody2D>().velocity;
    return currentVelocity.normalized;
  }

  // sets the ball to the start
  // TODO: reset rotation as well
  public void ResetBallPosition()
  {
    Transform ballTransform = gameObject.GetComponent<Transform>();
    Vector2 newPosition = new Vector2(0, 0);

    ballTransform.position = newPosition; // resets the position
    _ballRB.velocity = new Vector3(0, 0, 0); // resets the speed
  }

  // returns the radius of the ball
  public float GetRadius()
  {
    return gameObject.transform.lossyScale.x/2;
  }

  // returns the position of the "feet"
  public Vector3 GetFeetPosition()
  {
    Vector3 currentFeetPosition = transform.position - gameObject.GetComponentsInChildren<Transform>()[4].position;
    Debug.Log($"FEET: ({currentFeetPosition.x}, {currentFeetPosition.y})");
    return currentFeetPosition;
  }

  // applies a force to the ball
  public void ApplyHit(Vector3 force)
  {
    _ballRB.AddForce(force, ForceMode2D.Impulse);
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    //if (collision.gameObject.tag == "Player")
    //{
    //  Debug.Log("Hit the ball");
    //}
    if (collision.gameObject.tag == "Pole Goal Left" && !_isGoalAnimationRunning)
    {
      StartScoredAnimation(true);
    }
    if (collision.gameObject.tag == "Pole Goal Right" && !_isGoalAnimationRunning)
    {
      StartScoredAnimation(false);
    }
  }

  // starts the scoring animation: text and after a cool down resets the ball
  // Should reset _goalAnimationTimeDiff and _isGoalAnimationRunning and set the new scoreboard
  void StartScoredAnimation(bool leftPlayerHasScored)
  {
    _goalAnimationTimeDiff = DateTime.Now;
    _isGoalAnimationRunning = true;

    _hud.SetGlobalInformation("GOAL!");

    // sets the new scoreboard
    if (leftPlayerHasScored)
      _hud.UpdateScore(1, 0);
    else
      _hud.UpdateScore(0, 1);
  }

  // animation run when the player scores a goal
  void HandleScoredAnimation()
  {
    TimeSpan animationTimeDiff = DateTime.Now - _goalAnimationTimeDiff;
    if (animationTimeDiff.Seconds > _ballResetSeconds)
    {
      ResetBallPosition();
      _main.ResetPlayersPositions();
      _isGoalAnimationRunning = false;
    }
  }


}
