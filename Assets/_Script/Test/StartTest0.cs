using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTest0 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameManager.Instance.battleManager.StartBattle(null, new BattleContext());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
