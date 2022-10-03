using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

  [SerializeField] GameObject _ballPrefab;
  [SerializeField] GameObject _playerPrefab;
  //[SerializeField] GameObject _worldPrefab;

  HUDController _hud;
  Ball _ball;

  PlayerController _player1;

  const bool _testing = true;

  // Start is called before the first frame update
  void Start()
  {
    InitWorld();
  }

  // Update is called once per frame
  void Update()
  {
    HandleMenu();
  }

  // handles the menu interaction
  void HandleMenu()
  {
    bool resetKeyPressed = Input.GetButtonDown("Debug Reset");
    if (resetKeyPressed && _testing)
    {
      // reset the game
      Debug.Log("Resetting game");
      _hud.ResetScore();
      _ball.ResetBallPosition();
    }
  }

  // function that get executed after the world objects have been created, this way we get the references
  void GetIntialValues()
  {
    _hud = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDController>();
    _ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<Ball>();
    GetPlayers();
  }

  // sets the players variables
  void GetPlayers()
  {
    _player1 = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
  }

  // initiazes the world, with the scenerio, the player and the ball
  void InitWorld()
  {
    Vector2 ballInitialPos = new Vector2(0, 0);
    Vector2 playerIntialPos = new Vector2(0, -5f);

    // init map
    //Instantiate(_worldPrefab, Vector2.zero, Quaternion.identity);

    // create ball
    Instantiate(_ballPrefab, ballInitialPos, Quaternion.identity);

    // create player
    Instantiate(_playerPrefab, playerIntialPos, Quaternion.identity);

    // sets the variables
    GetIntialValues();
  }

  // reset the position of the player when theres a score
  public void ResetPlayersPositions()
  {
    _player1.ResetPosition();
  }

}
