using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

  TMP_Text _globalInformation;
  TMP_Text _scoreDisplay;
  TMP_Text _player1Cooldown;
  string _player1CurrentCooldown = "0";
  int _scoreLeft = 0;
  int _scoreRight = 0;
  DateTime _informationTimeSpan;
  bool _updatedGlobalInformationRecently = false;

  const int _globalInformationUpdateSeconds = 1;

  void Start()
  {
    // get the components
    _globalInformation = GameObject.FindGameObjectWithTag("GlobalInformation").GetComponent<TMP_Text>();
    _player1Cooldown = GameObject.FindGameObjectWithTag("AbilitiesDisplay").GetComponent<TMP_Text>();
    _scoreDisplay = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();

    // initial values
    SetUpScore();
    _globalInformation.text = "";
  }

  void Update()
  {
    if (_updatedGlobalInformationRecently)
    {
      UpdateGlobalInformation();
    }
  }

  public void ResetScore()
  {
    _scoreLeft = 0;
    _scoreRight = 0;
    SetUpScore();
    SetGlobalInformation("Game Restarted!");
  }

  void SetKickCooldown()
  {
    _player1Cooldown.text = _player1CurrentCooldown;
  }

  // updates the current used ability
  // TODO: should select the corret HUD for the current player
  public void UpdateKickCooldown(string cooldown, string player)
  {
    _player1CurrentCooldown = cooldown;
    SetKickCooldown();
  }

  // recieves the new score
  public void UpdateScore(int left, int right)
  {
    if (left > 0)
    {
      _scoreLeft += left;
    }
    if (right > 0)
    {
      _scoreRight += right;
    }
    SetUpScore();
  }

  // buils the score string and updates the text based on the scores
  void SetUpScore()
  {
    StringBuilder scoreString = new StringBuilder();

    scoreString.Append(_scoreLeft.ToString());
    scoreString.Append(" - ");
    scoreString.Append(_scoreRight.ToString());

    _scoreDisplay.text = scoreString.ToString();
  }

  // displays a text for a short period of time
  public void SetGlobalInformation(string info)
  {
    _informationTimeSpan = DateTime.Now;
    _globalInformation.text = info;
    _updatedGlobalInformationRecently = true;
  }

  // cleans the text of the global information after a while
  void UpdateGlobalInformation()
  {
    TimeSpan timeDiff = DateTime.Now -_informationTimeSpan;
    if (timeDiff.Seconds > _globalInformationUpdateSeconds)
    {
      _updatedGlobalInformationRecently = true;
      _globalInformation.text = "";
    }
  }

}
