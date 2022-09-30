using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

  TMP_Text _scoreDisplay;
  TMP_Text _abilityComponent;
  string _currentAbility = "1";
  int _scoreLeft = 0;
  int _scoreRight = 0;

  void Start()
  {
    _abilityComponent = GameObject.FindGameObjectWithTag("AbilitiesDisplay").GetComponent<TMP_Text>();
    _scoreDisplay = GameObject.FindGameObjectWithTag("Score").GetComponent<TMP_Text>();
    SetUpScore();
  }

  void UpdateHUDTextAbility()
  {
    _abilityComponent.text = _currentAbility;
  }

  // updates the current used ability
  public void UpdateAbility(string ability)
  {
    //Debug.Log($"Updating ability: {ability}");
    _currentAbility = ability;
    UpdateHUDTextAbility();
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

}
