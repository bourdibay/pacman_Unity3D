using UnityEngine;
using System.Collections;

public class callGameController : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GameController t = GameController.Instance;
		// enleve le warning
		t.enabled = true;
	}
	
}
