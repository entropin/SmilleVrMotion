using UnityEngine;
using System.Collections;

public class velocityGameController : MonoBehaviour {


	public UnityStandardAssets.Characters.ThirdPerson.ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
	private Transform m_Controll;                  // A reference to the main camera in the scenes transform
	private Vector3 m_CamForward;             // The current forward direction of the camera
	public Transform m_Move;
	private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.

	public float m_duckHight =1.5f;
	//Velocity
	private Vector3 oldPoss;
	private Vector3 newPoss;

	private Vector3 media;
	private Vector3 velosity;

	private Vector3 playerMovment;



	// Use this for initialization
	void Start () {
		if (m_Move == null)
			m_Move = transform;

		oldPoss = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		bool crouch;
		bool jump = false;

		//Calculate poss diff from last frame to get Velosity
		newPoss = transform.position;
		media = (newPoss - oldPoss);
		velosity = media / Time.deltaTime;

		//Resetting vars
		oldPoss = newPoss;
		newPoss = transform.position;

		Debug.Log ("Horizontal:" + velosity.x.ToString ());
		Debug.Log ("Vertical:"   + velosity.z.ToString ());
		Debug.Log ("Paralell:"   + velosity.y.ToString ());


		//Controll player crouch
		if(m_Move.localPosition.y <= m_duckHight){
			crouch = true;
		}else{
			crouch = false;
		}

		// we use world-relative directions in the case of no main camera
		playerMovment = velosity.z*Vector3.forward + velosity.x*Vector3.right;
		m_Character.Move(playerMovment, crouch, jump);

	}
}
