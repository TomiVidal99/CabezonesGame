using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
  HUDController _hud;
  Main _main;

  bool _isGoalAnimationRunning = false;
  DateTime _goalAnimationTimeDiff;

  const int _ballResetSeconds = 2;

  void Start()
  {
    _main = GameObject.FindGameObjectWithTag("Main").GetComponent<Main>();
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
  }

  void Update()
  {
    if (_isGoalAnimationRunning)
    {
      HandleScoredAnimation();
    }
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
