using UnityEngine;
using System.Collections;

public class TouchTrigger : MonoBehaviour 
{
	void Update()
	{
		if(Input.GetMouseButtonUp(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			RaycastHit hitInfo;
			if(Physics.Raycast(ray, out hitInfo))
			{
				BaseBox box = hitInfo.collider.GetComponentInParent<BaseBox>();
				if(box != null)
				{
					box.OnTouched();
				}
			}
		}
	}
}
