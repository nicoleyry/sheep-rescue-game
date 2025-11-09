using UnityEngine;

public class HayMachine : MonoBehaviour
{
	public float movementSpeed = 100.0f;
	public float horizontalBoundary = 22;
	public GameObject hayBalePrefab;
	public Transform haySpawnpoint;
	public float shootInterval;
	public float shootTimer;
	public Transform modelParent;
	public GameObject blueModelPrefab;
	public GameObject yellowModelPrefab;
	public GameObject redModelPrefab;

	void Start()
	{
		LoadModel();
	}
	
	void LoadModel()
	{
		Destroy(modelParent.GetChild(0).gameObject);

		switch(GameSettings.hayMachineColor)
		{
			case HayMachineColor.Blue:
				Instantiate(blueModelPrefab, modelParent);
				break;

			case HayMachineColor.Yellow:
				Instantiate(yellowModelPrefab, modelParent);
				break;

			case HayMachineColor.Red:
				Instantiate(redModelPrefab, modelParent);
				break;
		}
	}

	void Update()
	{
		UpdateMovement();
		UpdateShooting();
	}

	void UpdateMovement()
	{
		float horizontalInput = Input.GetAxis("Horizontal");

		if (horizontalInput < 0 && transform.position.x > -horizontalBoundary)
		{
			transform.Translate(transform.right * -movementSpeed * Time.deltaTime);
		}
		else if (horizontalInput > 0 && transform.position.x < horizontalBoundary)
		{
			transform.Translate(transform.right * movementSpeed * Time.deltaTime);
		}
	}
	
	void UpdateShooting()
	{
		shootTimer -= Time.deltaTime;

		if(shootTimer <= 0 && Input.GetKey(KeyCode.Space))
		{
			shootTimer = shootInterval;
			ShootHay();
		}
	}

	void ShootHay()
	{
		Instantiate(hayBalePrefab, haySpawnpoint.position, Quaternion.identity);

		SoundManager.Instance.PlayShootClip();
	}
}
