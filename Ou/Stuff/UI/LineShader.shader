Shader "Hid/LineShader"
{
	Properties{
		_MyColor("Some Color", Color) = (1,1,1,1)
	}
		SubShader
	{
		Tags{ "Queue" = "Overlay+1" }
		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		fixed4 _MyColor;

	struct v2f
	{
		float4 vertex : POSITION;
	};

	v2f vert(float4 vertex : POSITION)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, vertex);
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		return _MyColor;
	}
		ENDCG
	}
	}

}
