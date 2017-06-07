using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shoot : MonoBehaviour {
	public GameObject ball; //reference to the ball prefab, set in editor
	private Vector3 perfectThrow = new Vector3(0, 12, 7); //This value is a sure basket, we'll modify this using the forcemeter
	private Vector3 throwSpeed;
	public Vector3 ballPos; //starting ball position
	private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
	private GameObject ballClone; //we don't use the original prefab

	public Text availableShotsGO; //ScoreText game object reference
	private int availableShots = 10;

	public GameObject meter; //references to the force meter
	public GameObject arrow;
	private float arrowSpeed = 0.15f; //Difficulty, higher value = faster arrow movement
	private bool right = true; //used to revers arrow movement

	public Text gameOver; //game over text

	private Vector3 initGravity = new Vector3(0, -10, 3.66f);
	private Vector3 thrownGravity = new Vector3(0, -10, 1.5f);

	// Use this for initialization
	void Start () {
		/* Increase Gravity */
		Physics.gravity = initGravity;
		gameOver.gameObject.SetActive (false);

		throwSpeed = perfectThrow;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void DeleteBall() {
		/* Remove Ball when it hits the floor */

		if (ballClone != null && thrown) {
			Physics.gravity = initGravity;

			Rigidbody rb = ballClone.GetComponent<Rigidbody> ();
			if (ballClone.transform.position.y < -3 || rb.velocity.z < 0) {
				Destroy (ballClone);
				ball.SetActive (true);
				thrown = false;
				throwSpeed = perfectThrow;

				/* Check if out of shots */
				if (availableShots == 0) {
					arrow.SetActive (false);
					gameOver.gameObject.SetActive (true); 
					Invoke ("restart", 2);
				}
			}
		}
	}

	void TakeShot() {
		/* Shoot ball on Tap */

		if (Input.GetButton("Fire1") && !thrown && availableShots > 0)
		{
			Physics.gravity = thrownGravity;

			thrown = true;
			availableShots--;
			availableShotsGO.text = availableShots.ToString();

			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
			throwSpeed.y = throwSpeed.y + arrow.transform.position.x * Mathf.Sqrt(2);
			throwSpeed.z = throwSpeed.z + arrow.transform.position.x * Mathf.Sqrt(2);
			ball.SetActive (false);

			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
		}
	}

	void MovePowerMeter() {
		/* Move Meter Arrow */

		if (arrow.transform.position.x < 2.5f && right)
		{
			arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
		}
		if (arrow.transform.position.x >= 2.5f)
		{
			right = false;
		}
		if (right == false)
		{
			arrow.transform.position -= new Vector3(arrowSpeed, 0, 0);
		}
		if (arrow.transform.position.x <= -2.5f)
		{
			right = true;
		}
	}

	void FixedUpdate() {

		MovePowerMeter ();

		TakeShot ();

		DeleteBall ();
	}

	void restart()
	{
//		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		SceneManager.LoadScene("home");
	}

}
