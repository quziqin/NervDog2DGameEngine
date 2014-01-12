float Alpha;
float4x4 World;
float4x4 View;
float4x4 Projection;
texture Texture;
float3 Color;

sampler TextureSampler : register(s0) = sampler_state
{
   Texture = (Texture);
   MinFilter = Linear;
   MagFilter = Linear;
   MipFilter = Linear;   
   AddressU  = Clamp;
   AddressV  = Clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float3 View     : TEXCOORD1;
    float2 TexCoord : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    VertexShaderOutput output;
    output.Position = mul(viewPosition, Projection);
    output.View = worldPosition;
    output.TexCoord = input.TexCoord;
    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    float4 texColor = tex2D(TextureSampler, input.TexCoord);
    float3 finalColor = texColor + Color;
    return float4(finalColor, texColor.a*Alpha);
}

technique Technique0
{
    pass Pass0
    {
        AlphaBlendEnable = true;
        SrcBlend         = SRCALPHA;
        DestBlend        = INVSRCALPHA;
        ZEnable          = true;
        ZWriteEnable     = false;
        CullMode         = none;

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
