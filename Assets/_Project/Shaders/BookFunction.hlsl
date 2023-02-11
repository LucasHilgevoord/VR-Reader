//UNITY_SHADER_NO_UPGRADE
#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

// This is a HLSL include file that can be included in other HLSL files.
// It is not a shader itself.

void BookFunction_float(float x, float i, float q, float w, float t, float a, float d, out float Out) {
	float b = max(-a * pow(abs(x * i) - 1 + t, 2) + pow(a, 2), 0);
	//Kleine optimalisatie voor l, gezien hij blijkbaar h=0 neemt: l(g,0,k)=g(1-k)

	float OneMinusK = b * (w - q * abs(x * i));
	float l = b * OneMinusK;
	float f = d * l;
	Out = f;
}

#endif // MYHLSLINCLUDE_INCLUDED