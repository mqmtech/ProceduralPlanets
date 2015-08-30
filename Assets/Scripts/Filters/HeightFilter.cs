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

		_rotation = Quaternion.AngleAxis(18.0f, Vector3.right*0.5f + Vector3.forward* 0.5f);
	}

	public float GetHeight(Vector3 input)
	{
		input = _rotation * input;
		input.Normalize();

		float noise = 0f;

		noise += 0.500f*mMath.smoothstep(0.15f, 1.0f, _sinusNoiseGenerator.Generate(input * 3.15f));
		noise += 0.250f*mMath.smoothstep(0.0f, 1.0f, _sinusNoiseGenerator.Generate(input * 6.15f));
		noise += 0.250f*mMath.smoothstep(0.0f, 1.0f,_noiseGenerator.Generate(input * 2.01125f));

		noise /= 1.000f;

		noise = Mathf.Clamp01(noise - 0.25f)/0.75f;

		return _minHeight + (_maxHeight - _minHeight) * noise;
	}
}
