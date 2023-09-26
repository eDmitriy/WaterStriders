// https://docs.unity3d.com/Packages/com.unity.shadergraph@17.0/manual/Custom-Function-Node.html
//
//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

float4 _RipplePoints[2]; 
int _RipplePointsNum = 0;

void MyFunction_float(float3 PixelWPos, float Radius, out float3 Out)
{

    for (int i = 0; i < _RipplePointsNum; ++i)
    {
	    if(distance(_RipplePoints[i].xyz, PixelWPos) < Radius)
	    {
			Out = 1;
			break;
	    }
    }
}
#endif //MYHLSLINCLUDE_INCLUDED