using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComboManager : MonoBehaviour {

    [SerializeField]
    public List<string> ButtonCombo = new List<string>();

    public int _currentComboStage = 0;
    public string _currentComboKey;
    public string _nextComboKey;

    private PlayerController player;


    //Start
    void Start () {

        _currentComboKey = ButtonCombo[0];
        _nextComboKey = ButtonCombo[1];
        _currentComboStage = 0;

        player = FindObjectOfType<PlayerController>();
	}
	
	//Update
	void Update () {
		
	}

    public void UpdateCombo(string _keyJustHit)
    {

        Debug.Log(_keyJustHit);
        Debug.Log(_currentComboKey);

        if (_keyJustHit == _currentComboKey && _currentComboStage + 1 < ButtonCombo.Count)
        {
            Debug.Log(_currentComboStage);

            _currentComboKey = ButtonCombo[_currentComboStage + 1];
            _currentComboStage++;
        }
        else
        {
            _currentComboKey = ButtonCombo[0];
            _currentComboStage = 0;
        }

        if (_currentComboStage == ButtonCombo.Count - 1)
        {
            Destroy(gameObject);
        }
    }
}
