using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

	public bool isLinks=false;


	void OnTriggerEnter (Collider col)
	{
	
		Debug.Log ("Kollidiere" + isLinks);
		GM.instance.Score(isLinks);
	}

}
