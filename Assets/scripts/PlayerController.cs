using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  HUDController _hud;
  Rigidbody2D _playerRB;
  Ball _ball;
  Debugger _debugger;

  float _kickingForce = 10f;
  float _playerSpeed = 7f;
  float _jumpSpeed = 800f;

  bool facingRight = true;

  float _inputHorizontal;
  float _inputVertical;
  bool _jumpKey, _leftKey, _rightKey, _hitKey;
  Vector3 _mousePosition;

  bool _isGrounded = false;

  bool _canKick = true;
  DateTime _kickedTime;
  float _kickCoolDownSeconds = 2f;

  void Start()
  {
    _debugger = GameObject.FindGameObjectWithTag("Main").GetComponent<Debugger>();
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
    _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    _playerRB = gameObject.GetComponent<Rigidbody2D>();
  }

  // runs every frame
  void Update()
  {
    UpdateMousePosition(); // get the current position of the mouse
    HandleMovementInput(); // gets the keys currently press
    HandleMovement(); // updates the player movement based on the key pressed

    HandleKicking(); // check if the player can kick and if so applies the force, else just wait the kick cooldown

    UpdateFacingSide();

    UpdateKickCooldownHUD();
  }

  void UpdateKickCooldownHUD()
  {
    double cooldown = (_kickCoolDownSeconds - (DateTime.Now - _kickedTime).Seconds);
    // only update if the time it's less than the _kickCoolDownSeconds
    if (cooldown >= 0)
      _hud.UpdateKickCooldown(cooldown.ToString("0") + "s", "");
  }

  // updates the position of the mouse based on unity meassuring system;
  void UpdateMousePosition()
  {
    _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
  }

  void HandleMovementInput()
  {
    // left, right, up, down movement 
    _inputHorizontal = Input.GetAxisRaw("Horizontal");
    _inputVertical = Input.GetAxisRaw("Vertical");

    // special keys
    _jumpKey = Input.GetKeyDown(KeyCode.W);
    _leftKey = Input.GetKeyDown(KeyCode.A);
    _rightKey = Input.GetKeyDown(KeyCode.D);
    _hitKey = Input.GetKeyDown(KeyCode.Space);
  }

  // Controls the movement of the character
  void HandleMovement()
  {
    if (_inputHorizontal != 0)
    {
      _playerRB.AddForce(new Vector2(_inputHorizontal * _playerSpeed, 0));
    }

    if (_inputVertical > 0 && _isGrounded)
    {
      _playerRB.AddForce(new Vector2(0, _inputVertical * _jumpSpeed));
      _isGrounded = false;
    }
  }

  // sets whether should be facing left or right based on the mouse position
  void UpdateFacingSide()
  {
    Vector3 facingPos = GetFacingDirection();
    // Debug.Log($"({facingPos.x}, {facingPos.y})");
    if (facingPos.x < 0 && facingRight || facingPos.x > 0 && !facingRight)
    {
      facingRight = !facingRight;
      gameObject.transform.RotateAround(transform.position, transform.up, 180f);
    }

    // updates the eyes positions
    float eyeAngle = Mathf.Atan(facingPos.normalized.y / facingPos.normalized.x);
    GameObject.FindGameObjectWithTag("Eye").transform.rotation = Quaternion.Euler(0f, 0f, facingRight ? eyeAngle * Mathf.Rad2Deg : eyeAngle * Mathf.Rad2Deg - 180f);
  }

  // returns where the player it's facing based on the mouse input
  public Vector3 GetFacingDirection()
  {
    Vector3 facingPos = _mousePosition - gameObject.transform.position;
    _debugger.DisplayVector(Vector3.zero, facingPos.normalized, Color.red);
    return facingPos;
  }

  // resets the position
  public void ResetPosition()
  {
    // TODO check this when making the MP
    Vector2 player1InitPos = new Vector2(-5, 0);
    gameObject.transform.position = player1InitPos;
  }

  // returns the position of the "feet"
  Vector3 GetFeetPosition()
  {
    GameObject feet = GameObject.Find("KickOrigin");
    Transform feetTransform =  feet.transform;
    return feetTransform.position;
  }

  // checks if the player it's pressing the kicking key and applies the force to the ball
  // TODO: optimize this calculating the magnitude before and only run it if the distance it's at a certain minimun
  void HandleKicking()
  {
    if (_hitKey)
    {
      Vector3 ballPosition = _ball.GetCurrentPosition();
      Vector3 feetPosition = GetFeetPosition();
      Vector3 facingDirection = GetFacingDirection();
      float ballRadius = _ball.GetRadius();

      // TODO: make this value more realistic
      float sweetSpotRadius = .5f;
      float sweetSpotTolarence = 0.6f; 

      Vector3 feetBallVector = ballPosition - feetPosition;
      float playerBallMagDiff = feetBallVector.magnitude - ballRadius;

      // apply a force proportional to the distance of the player
      // TODO: eventually make a sweet spot where the force it's maximum at a certain
      // distance of the player
      if (playerBallMagDiff >= (sweetSpotRadius-sweetSpotTolarence) &&
          playerBallMagDiff <= (sweetSpotRadius+sweetSpotTolarence))
      {
        Vector3 kickingForce = facingDirection.normalized * playerBallMagDiff * _kickingForce * ( (float) Math.Cos(facingDirection.normalized.x));
        KickBall(kickingForce);
      }
    }
  }

  void KickBall(Vector3 kickingForce = new Vector3())
  {
    TimeSpan cooldown = DateTime.Now - _kickedTime;
    // check if the player it's kicking the ball
    if (_canKick && cooldown.Seconds > _kickCoolDownSeconds)
    {
      _ball.ApplyHit(kickingForce);
      _canKick = false;
      _kickedTime = DateTime.Now;
    } else
    {
      _canKick = true;
    }
  }

  // detect if the player it's touching the ground
  void OnCollisionEnter2D(Collision2D collision)
  {
    //Debug.Log("collitions " + collision.gameObject.tag);
    if (collision.gameObject.tag == "Ground")
    {
      _isGrounded = true;
    }
  }

}
