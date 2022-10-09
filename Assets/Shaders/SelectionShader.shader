// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Unlit/SelectionShader" {
  SubShader {
    Tags { "RenderType"="Transparent" }
    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag
      #include "UnityCG.cginc"
      UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_DEFINE_INSTANCED_PROP(fixed4, _SelectionColor)
      #define _SelectionColor_arr Props
      UNITY_INSTANCING_BUFFER_END(Props)
 
      float4 vert (float4 vertex : POSITION) : SV_POSITION {
        return UnityObjectToClipPos(vertex);
      }
 
      fixed4 frag () : SV_Target {
        //return fixed4(1.0,1.0,0.0,0.0);
        return UNITY_ACCESS_INSTANCED_PROP(_SelectionColor_arr, _SelectionColor);
      }
      ENDCG
    }
  }
}