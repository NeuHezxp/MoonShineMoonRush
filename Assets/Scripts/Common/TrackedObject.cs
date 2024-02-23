using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrackedObject : MonoBehaviour
{
	public UnityAction<GameObject> OnDestroyed;//used in spawner for how many should spawn

	private void OnDestroy()
	{
		OnDestroyed?.Invoke(gameObject);
	}
}
