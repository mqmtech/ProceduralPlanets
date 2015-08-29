using UnityEngine;
using System.Collections;

public class BaseBox : MonoBehaviour 
{
	TerrainGenerator.BoxGenerationProperties _generationProperties;
	public TerrainGenerator.BoxGenerationProperties GenerationProperties { get { return _generationProperties; } }

	public void OnTouched()
	{
		gameObject.SetActive(false);
		TerrainGenerator.Instance.OnBoxDestroyed(this);
	}

	public void SetGenerationProperties(TerrainGenerator.BoxGenerationProperties generationProperties)
	{
		_generationProperties = generationProperties;
	}
}
