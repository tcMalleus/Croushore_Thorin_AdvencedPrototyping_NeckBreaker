using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour {

    public List<string> ButtonCombo = new List<string>();

    public int _currentComboStage = 0;
    public string _currentComboKey;
    public string _nextComboKey;

	// Use this for initialization
	void Start () {

        _currentComboKey = ButtonCombo[0];
        _nextComboKey = ButtonCombo[1];
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateCombo(string _keyJustHit)
    {
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
