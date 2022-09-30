using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDController : MonoBehaviour
{

  TMP_Text _abilityComponent;
  string _currentAbility = "1";
  int _scoreLeft = 0;
  int _scoreRight = 0;

  void Start()
  {
    _abilityComponent = GameObject.FindGameObjectWithTag("AbilitiesDisplay").GetComponent<TMP_Text>();
  }

  void UpdateHUDTextAbility()
  {
    _abilityComponent.text = _currentAbility;
  }

  public void UpdateAbility(string ability)
  {
    //Debug.Log($"Updating ability: {ability}");
    _currentAbility = ability;
    UpdateHUDTextAbility();
  }

  public void UpdateScore(int left, int right)
  {
    if (left > 0)
    {
      _scoreLeft = left;
    }
    if (right > 0)
    {
      _scoreRight = right;
    }
  }
}
