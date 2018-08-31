﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboManager : MonoBehaviour {

    [SerializeField]
    public List<string> ButtonCombo = new List<string>();

    public int _currentComboStage = 0;
    public string _currentComboKey;
    public string _nextComboKey;

	//Start
	void Start () {

        _currentComboKey = ButtonCombo[0];
        _nextComboKey = ButtonCombo[1];
		
	}
	
	//Update
	void Update () {
		
	}

    public void UpdateCombo(string _keyJustHit)
    {

        Debug.Log(_keyJustHit);

        if (_currentComboKey == _keyJustHit && _currentComboStage + 1 < ButtonCombo.Count)
        {
            _currentComboKey = ButtonCombo[_currentComboStage + 1];
            _currentComboStage++;
        }
        else
        {
            _currentComboKey = ButtonCombo[0];
            _currentComboStage = 0;
        }
    }
}