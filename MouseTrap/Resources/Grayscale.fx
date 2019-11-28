sampler2D implicitInput : register(s0);

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float4 color = tex2D(implicitInput, uv);
	float gray = dot(color.rgb, float3(0.3, 0.59, 0.11));
	return float4(gray, gray, gray, color.a);
}