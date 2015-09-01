using UnityEngine;
using System.Collections;

public class BaseBox : MonoBehaviour 
{
	PlanetGenerator _panet;
	public Vector3 LocalPosition 
	{
		get
		{
			return _panet.ConvertToLocalPosition(transform.position);
		}
	}

	PlanetGenerator.BoxGenerationProperties _generationProperties;
	public PlanetGenerator.BoxGenerationProperties GenerationProperties { get { return _generationProperties; } }

	public void Init(PlanetGenerator planet)
	{
		_panet = planet;
	}

	public void OnTouched()
	{
		gameObject.SetActive(false);
		PlanetGenerator.Instance.OnBoxDestroyed(this);
	}

	public void SetGenerationProperties(PlanetGenerator.BoxGenerationProperties generationProperties)
	{
		_generationProperties = generationProperties;
	}
}
