using UnityEngine;
using System.Collections;
using MQMTech.Mathematics;

public class HeightFilter 
{
	INoise3DGenerator _sinusNoiseGenerator;
	INoise3DGenerator _perlinNoiseGenerator;

	INoise3DGenerator _noiseGenerator;

	float _minHeight;
	float _maxHeight;

	Quaternion _rotation;

	public void Init(float minHeight, float maxHeight, INoise3DGenerator noiseGenerator)
	{
		_minHeight = minHeight;
		_maxHeight = maxHeight;
		_noiseGenerator = noiseGenerator;

		_sinusNoiseGenerator = new SinusNoiseGenerator();
		_perlinNoiseGenerator = new PerlinNoiseGenerator();

		_rotation = Quaternion.AngleAxis(24.25458f, Vector3.up*0.25f + Vector3.right*0.5f + Vector3.forward* 0.25f);
	}

	public float GetHeight(Vector3 input)
	{
		input = _rotation * input;
		input.Normalize();

		float noise = 0f;
		noise += 0.6f*_sinusNoiseGenerator.Generate(input * 3.15f);
		noise += 0.4f*mMath.smoothstep(0.40f, 1.0f,_noiseGenerator.Generate(input * 2.0125f));
		//noise += 0.25f*_noiseGenerator.Generate(input * 3.5f);

		//noise /= 0.95f;

		return _minHeight + (_maxHeight - _minHeight) * noise;
	}
}
