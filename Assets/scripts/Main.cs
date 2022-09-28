using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

  [SerializeField] GameObject _ball;
  [SerializeField] GameObject _player;
  [SerializeField] GameObject _world;

  // Start is called before the first frame update
  void Start()
  {
    InitWorld();
  }

  // Update is called once per frame
  void Update()
  {

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
    //TODO
    //Instantiate(_player, playerIntialPos, Quaternion.identity);
  }
}
