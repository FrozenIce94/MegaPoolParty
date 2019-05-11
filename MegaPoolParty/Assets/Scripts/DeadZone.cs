using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour {

	public bool isLinks=false;


	void OnTriggerEnter (Collider col)
	{
	
		Debug.Log ("Kollidiere "+this.gameObject.name+":" +col.gameObject.name +" "+ isLinks);
       // Debug.Break();
		GM.instance.Score(isLinks);
	}

}
