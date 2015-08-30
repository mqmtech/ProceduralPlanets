using UnityEngine;
using System.Collections;

public class BaseBox : MonoBehaviour 
{
	PlanetGenerator.BoxGenerationProperties _generationProperties;
	public PlanetGenerator.BoxGenerationProperties GenerationProperties { get { return _generationProperties; } }

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
