// Preprocessor Directives
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Properties
float AccumulationStrength;
float RenderCapValue;

// Texture and Sampler Setup
Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
    Filter = Point;
};
Texture2D AccumulatorTexture;
sampler2D AccumulatorTextureSampler = sampler_state
{
    Texture = <AccumulatorTexture>;
    Filter = Point;
};

// Vertex Shader Output Setup
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float4 renderColor = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    float4 accumulatedColor = tex2D(AccumulatorTextureSampler, input.TextureCoordinates) * input.Color * AccumulationStrength;
    return max(renderColor * RenderCapValue, accumulatedColor);
}

// Technique and Pass
technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};