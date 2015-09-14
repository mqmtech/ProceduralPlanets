using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MaterialGenerator 
{
	[System.Serializable]
	public struct MaterialProperty
	{
		public float MinRadius;
		public float MaxRadius;
		public Material Material;
		public float Weight;
	}

	[SerializeField]
	List<MaterialProperty> _materialProperties;

	List<MaterialProperty> _tempAvalilableMaterials = new List<MaterialProperty>();

	PerlinNoiseGenerator _noiseGenerator = new PerlinNoiseGenerator();

	float _maxRadius;

	public void Init(float maxRadius)
	{
		_maxRadius = maxRadius;
	}

	public Material Get(float radius, Vector3 point)
	{
		CalculateAvailableMaterials(radius);
		return DoGetMaterial(point);
	}

	void CalculateAvailableMaterials(float radius)
	{
		_tempAvalilableMaterials.Clear();

		for (int i = 0; i < _materialProperties.Count; ++i) 
		{
			if(IsMaterialValid(_materialProperties[i], radius))
			{
				_tempAvalilableMaterials.Add(_materialProperties[i]);
			}
		}
	}

	bool IsMaterialValid(MaterialProperty property, float radius)
	{
		float minRadius =  Mathf.Round(property.MinRadius * _maxRadius);
		float maxRadius =  Mathf.Round(property.MaxRadius * _maxRadius);
		radius = Mathf.Round(radius);
		return radius >= minRadius && radius <= maxRadius;
	}

	Material DoGetMaterial(Vector3 point)
	{
		float randomWeight = _noiseGenerator.Generate(point*0.20f);

		float totalWeight = CalculateTotalWeight();
		float currentWeight = 0f;
		for (int i = 0; i < _tempAvalilableMaterials.Count; ++i) 
		{
			currentWeight += _tempAvalilableMaterials[i].Weight / totalWeight;
			if(randomWeight <= currentWeight)
			{
				return _tempAvalilableMaterials[i].Material;
			}
		}

		//Debug.LogWarning("Found " + _tempAvalilableMaterials.Count + " materials, but the random did not match with any weight :(");
		return _materialProperties[_materialProperties.Count-1].Material;
	}

	float CalculateTotalWeight()
	{
		float weight = 0f;

		_tempAvalilableMaterials.ForEach((MaterialProperty property)=>
     	{
			weight += property.Weight;
		});

		return  weight;
	}
}
