using UnityEngine;
using System.Collections;

public class HeightFilter 
{
	INoise3DGenerator _noiseGenerator;

	float _minHeight;
	float _maxHeight;

	public void Init(float minHeight, float maxHeight, INoise3DGenerator noiseGenerator)
	{
		_minHeight = minHeight;
		_maxHeight = maxHeight;
		_noiseGenerator = noiseGenerator;
	}

	public float GetHeight(Vector3 input)
	{
		return _minHeight + (_maxHeight - _minHeight) * _noiseGenerator.Generate(input);
	}
}
