using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCharacterScript : MonoBehaviour
{

	public Transform Player;

	private Vector3 distance;
	
	// Use this for initialization
	void Start ()
	{
		distance = Player.position - transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Player.position - distance;
	}
}
