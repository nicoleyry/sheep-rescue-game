using UnityEngine;

public class Sheep : MonoBehaviour
{
	public float runSpeed;
	public float gotHayDestroyDelay;
	public bool hitByHay;
	public float dropDestroyDelay;
	private Collider myCollider;
	private Rigidbody myRigidbody;
	private SheepSpawner sheepSpawner;
	public float heartOffset;
	public GameObject hearPrefab;

	private bool hasDropped = false;

	void Start()
    {
		myCollider = GetComponent<Collider>();
		myRigidbody = GetComponent<Rigidbody>();
    }

	void Update()
	{
		transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
	}

	public void HitByHay()
	{
		sheepSpawner.RemoveSheepFromList(gameObject);

		hitByHay = true;
		runSpeed = 0;

		Destroy(gameObject, gotHayDestroyDelay);

		Instantiate(hearPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);

		TweenScale tweenScale = gameObject.AddComponent<TweenScale>();
		tweenScale.targetScale = 0;
		tweenScale.timeToReachTarget = gotHayDestroyDelay;

		SoundManager.Instance.PlaySheepHitClip();
		GameStateManager.Instance.SavedSheep();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Hay") && !hitByHay) {
			Destroy(other.gameObject);
			HitByHay();
		} else if(other.CompareTag("DropSheep")) {
			Drop();
		}
	}

	void Drop()
	{
		if (hasDropped)
		{
			return;
		}
		
		hasDropped = true;
		GameStateManager.Instance.DroppedSheep();
		sheepSpawner.RemoveSheepFromList(gameObject);

		myRigidbody.isKinematic = false;
		myCollider.isTrigger = false;
		Destroy(gameObject, dropDestroyDelay);

		SoundManager.Instance.PlaySheepDroppedClip();
	}
	
	public void SetSpawner(SheepSpawner spawner)
	{
		sheepSpawner = spawner;
	}

}
