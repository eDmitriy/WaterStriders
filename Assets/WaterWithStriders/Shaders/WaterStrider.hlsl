// https://docs.unity3d.com/Packages/com.unity.shadergraph@17.0/manual/Custom-Function-Node.html
//
//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float4 _RipplePoints[200];
float _RippleMaxRadius = 1;
int _RipplePointsNum = 200;

float _WavesWeight = 0.03;
float _WavesXSize = 2;
float _WavesZSize = 3;


void MyFunction_float(float3 PixelWPos, float3 SurfNormal, float Time, out float3 Out)
{
	float mask = 0;
	float3 SurfNormalRipple = SurfNormal;

	[loop]
    for (int i = 0; i < _RipplePointsNum; ++i)
    {
	    const float rippleFrequency = 4;

	    float rippleTime = pow( _RipplePoints[i].w, 2);

		[branch]
		if (rippleTime < 0.01)
			continue;

	    float radius = (1.0 -  rippleTime ) * _RippleMaxRadius;
	    float clampedDist = min( distance(_RipplePoints[i].xyz, PixelWPos), radius);
	    float normalizedDist = clampedDist / radius;

		float normalizedDistInverse =  1.0 - normalizedDist ;


	    float rippleWeight = normalizedDistInverse * rippleTime;
	    float rippleWeightFrac = 1- frac(normalizedDistInverse * rippleTime * rippleFrequency);


		mask += rippleWeight;

		float3 dirFromCenterToWPos = normalize(PixelWPos - _RipplePoints[i].xyz);

		float3 lerp1 = lerp(-dirFromCenterToWPos, dirFromCenterToWPos, rippleWeightFrac);
		SurfNormalRipple = lerp(SurfNormalRipple, lerp1, rippleWeight * sin( rippleWeightFrac) * rippleTime);
		
    }

	//apply waves
	float wave = sin(PixelWPos.x* _WavesXSize + Time) * cos(PixelWPos.z* _WavesZSize + Time); //todo: gerstner waves can be used for betters waves
	float3 waveNormal = lerp(SurfNormal, float3(1, 0, 0), wave); //todo: need better normal

	Out = lerp(SurfNormalRipple, waveNormal, _WavesWeight);

}
#endif //MYHLSLINCLUDE_INCLUDED