Shader "T70/Self-Illumin/Diffuse_Instanced"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Illum ("Illumin (A)", 2D) = "white" {}
        _Emission ("Emission (Lightmapper)", Float) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert
        #pragma target 3.0
        #pragma multi_compile_instancing

        sampler2D _MainTex;
        sampler2D _Illum;
        fixed4 _Color;
        fixed _Emission;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_Illum;

            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        // nếu cần per-instance biến, khai báo ở đây
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            UNITY_SETUP_INSTANCE_ID(IN); // setup instance id cho GPU Instancing

            fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 c = tex * _Color;
            o.Albedo = c.rgb;
            o.Emission = c.rgb * tex2D(_Illum, IN.uv_Illum).a;

            #if defined (UNITY_PASS_META)
            o.Emission *= _Emission.rrr;
            #endif

            o.Alpha = c.a;
        }
        ENDCG
    }

    FallBack "T70/Self-Illumin/VertexLit"
    CustomEditor "LegacyIlluminShaderGUI"
}