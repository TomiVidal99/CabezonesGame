using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

  [SerializeField] GameObject _ball;
  [SerializeField] GameObject _player;
  [SerializeField] GameObject _world;

  PlayerController _player1;

  // Start is called before the first frame update
  void Start()
  {
    InitWorld();
  }

  // Update is called once per frame
  void Update()
  {

  }

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
    //Instantiate(_world, Vector2.zero, Quaternion.identity);

    // create ball
    //Instantiate(_ball, ballInitialPos, Quaternion.identity);

    // create player
    Instantiate(_player, playerIntialPos, Quaternion.identity);
    
    // sets the variables of the players
    GetPlayers();
  }

  // reset the position of the player when theres a score
  public void ResetPlayersPositions()
  {
    _player1.ResetPosition();
  }

}
