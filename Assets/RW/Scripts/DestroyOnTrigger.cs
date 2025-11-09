using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
	public string tagFilter;

	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag(tagFilter))
		{
			Destroy(gameObject);
		}
	}
}
