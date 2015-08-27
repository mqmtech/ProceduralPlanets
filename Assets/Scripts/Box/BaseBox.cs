using UnityEngine;
using System.Collections;

public class BaseBox : MonoBehaviour 
{
	public void OnTouched()
	{
		gameObject.SetActive(false);
	}
}
