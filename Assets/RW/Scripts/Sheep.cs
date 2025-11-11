using System.Collections;
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

	[Header("Boundaries")]
    public float leftBoundary = -15f;
    public float rightBoundary = 15f;

	void Start()
    {
		myCollider = GetComponent<Collider>();
		myRigidbody = GetComponent<Rigidbody>();

		StartCoroutine(ChangeDirectionRoutine());
    }

	void Update()
	{
		boundaryCheck();
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

	private IEnumerator ChangeDirectionRoutine()
	{
		while (true)
		{
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            if (hitByHay || hasDropped)
            {
                yield break;
            }
            
            // Just apply a random wiggle.
            float turnAngle = Random.Range(-45f, 45f);
            transform.Rotate(0, turnAngle, 0);
		}
	}
	
	void boundaryCheck()
	{
        if (hitByHay || hasDropped)
        {
            return;
        }

        Vector3 currentPos = transform.position;

		// Outside the left boundary.
		if (currentPos.x < leftBoundary)
		{
			// Nudge the sheep back to the boundary line
			currentPos.x = leftBoundary;

			// Force the sheep to turn right (toward the center)
			transform.rotation = Quaternion.Euler(0, Random.Range(45f, 135f), 0);
		}
		// Outside the right boundary.
		else if (currentPos.x > rightBoundary)
		{
			// Nudge the sheep back to the boundary line
			currentPos.x = rightBoundary;

			// Force the sheep to turn left (toward the center)
			transform.rotation = Quaternion.Euler(0, Random.Range(-135f, -45f), 0);
		}

        // Apply the (potentially corrected) position
        transform.position = currentPos;
        
        // Move the sheep forward in its current (or newly forced) direction
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
	}

}
