using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class MaterialGenerator 
{
	[System.Serializable]
	public struct MaterialProperty
	{
		public int MinRadius;
		public int MaxRadius;
		public Material Material;
		public float Weight;
	}

	[SerializeField]
	List<MaterialProperty> _materialProperties;

	[SerializeField]
	Material _unlitMaterial;

	List<MaterialProperty> _tempAvalilableMaterials = new List<MaterialProperty>();

	PerlinNoiseGenerator _noiseGenerator = new PerlinNoiseGenerator();

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
		return radius >= property.MinRadius && radius <= property.MaxRadius;
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

		Debug.LogWarning("Found " + _tempAvalilableMaterials.Count + " materials, but the random did not match with any weight :(");
		return _tempAvalilableMaterials[_tempAvalilableMaterials.Count-1].Material;
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
