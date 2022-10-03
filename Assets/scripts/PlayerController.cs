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

  [SerializeField] float _kickingForce = 70f;
  float _playerSpeed = 15f;
  float _jumpSpeed = 1000f;

  bool facingRight = true;

  float _inputHorizontal;
  float _inputVertical;
  bool _jumpKey, _leftKey, _rightKey, _hitKey;
  Vector3 _mousePosition;

  bool _isGrounded = false;

  bool _canKick = false;
  DateTime _kickedTime;
  float _kickCoolDownSeconds = 0.1f;

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
    HandleAbilities(); // updates the currently selected ability based on the key pressed

    HandleKicking(); // check if the player can kick and if so applies the force, else just wait the kick cooldown

    UpdateFacingSide();
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

  // controls the current selected ability of the player
  void HandleAbilities()
  {
    bool ability1 = Input.GetKeyDown(KeyCode.Alpha1),
         ability2 = Input.GetKeyDown(KeyCode.Alpha2),
         ability3 = Input.GetKeyDown(KeyCode.Alpha3);
    if (ability1)
    {
      _jumpSpeed = 1000f;
      _hud.UpdateAbility("1");
    }
    if (ability2)
    {
      _hud.UpdateAbility("2");
      _jumpSpeed = 1400f;
    }
    if (ability3)
    {
      _hud.UpdateAbility("3");
      _jumpSpeed = 1800f;
    }
  }

  // sets whether should be facing left or right based on the mouse position
  void UpdateFacingSide()
  {
    Vector3 facingPos = GetFacingDirection();
    // Debug.Log($"({facingPos.x}, {facingPos.y})");
    if (facingPos.x > 0 && facingRight || facingPos.x < 0 && !facingRight)
    {
      facingRight = !facingRight;
      gameObject.transform.RotateAround(transform.position, transform.up, 180f);
    }

    // updates the eyes positions
    float eyeAngle = Mathf.Atan(facingPos.normalized.y / facingPos.normalized.x);
    GameObject.FindGameObjectWithTag("Eye").transform.Rotate(new Vector3(0, 0, eyeAngle), Space.Self);

  }

  // returns where the player it's facing based on the mouse input
  public Vector3 GetFacingDirection()
  {
    _debugger.DisplayVector(new Vector3(0,0,0), new Vector3(5, 5, 0));
    return gameObject.transform.position - _mousePosition;
  }

  // resets the position
  public void ResetPosition()
  {
    // TODO check this when making the MP
    Vector2 player1InitPos = new Vector2(-5, 0);
    gameObject.transform.position = player1InitPos;
  }

  // checks if the player it's pressing the kicking key and applies the force to the ball
  // TODO: optimize this calculating the magnitude before and only run it if the distance it's at a certain minimun
  void HandleKicking()
  {
    if (_hitKey)
    {
      Vector3 ballPosition = _ball.GetCurrentPosition();
      Vector3 feetPosition = _ball.GetFeetPosition();
      float ballRadius = _ball.GetRadius();

      Vector3 feetBallDirection = feetPosition - ballPosition;

      float sweetSpotRadius = .5f;
      float sweetSpotTolarence = 0.25f; // TODO: make this value more realistic
      float playerBallMagDiff = feetBallDirection.magnitude + ballRadius + transform.lossyScale.x/2;

      Debug.Log($"playerBallMagDiff = {playerBallMagDiff}");

      // apply a force proportional to the distance of the player
      // TODO: eventually make a sweet spot where the force it's maximum at a certain
      // distance of the player
      if (playerBallMagDiff >= (sweetSpotRadius-sweetSpotTolarence) &&
          playerBallMagDiff <= (sweetSpotRadius+sweetSpotTolarence))
      {
        Vector3 kickingForce = _mousePosition.normalized * playerBallMagDiff * _kickingForce;
        KickBall(kickingForce);
        Debug.Log("Should kick");
      }
    }
  }

  void KickBall(Vector3 kickingForce = new Vector3())
  {
    TimeSpan cooldown = DateTime.Now - _kickedTime;
    // check if the player it's kicking the ball
    if (_canKick && cooldown.Seconds > _kickCoolDownSeconds)
    {
      // TODO: apply force to the ball and update _isKicking
      Debug.Log("HIT THE BALL");
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
