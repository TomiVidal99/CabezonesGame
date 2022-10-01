using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

  HUDController _hud;
  Rigidbody2D _playerRB;
  float _playerSpeed = 15f;
  float _jumpSpeed = 1000f;

  float _inputHorizontal;
  float _inputVertical;

  bool _isGrounded = false;

  void Start()
  {
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
    _playerRB = gameObject.GetComponent<Rigidbody2D>();
  }

  void Update()
  {
    HandleMovement();
    HandleAbilities();
  }

  // Controls the movement of the character
  void HandleMovement()
  {
    // handle move left and right
    bool  jump = Input.GetKeyDown(KeyCode.W),
          left = Input.GetKeyDown(KeyCode.A),
          right = Input.GetKeyDown(KeyCode.D),
          hit = Input.GetKeyDown(KeyCode.Space);

    _inputHorizontal = Input.GetAxisRaw("Horizontal");
    _inputVertical = Input.GetAxisRaw("Vertical");

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

  // resets the position
  public void ResetPosition()
  {
    // TODO check this when making the MP
    gameObject.transform.position = new Vector2(0, 0);
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
