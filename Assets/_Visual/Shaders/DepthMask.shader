Shader "Custom/DepthMask"
{
    Properties{}

	SubShader{

		Tags {
			"RenderType" = "Opaque"
		}

		Pass {
			ZWrite Off
		}
	}
}
