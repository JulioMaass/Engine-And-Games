// Preprocessor Directives
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Texture and Sampler Setup
Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
    Filter = Point;
};

// Outside Variables
float2 Direction;
float2 TargetSize;
float Distribution[21];

// Vertex Shader Output Setup
struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Pixel Shader
float4 MainPS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float2 texelSize = float2(1.0f / (float) TargetSize.x, 1.0f / (float) TargetSize.y);
    float4 color = float4(0, 0, 0, 0);
        
    // Sample and accumulate
    [loop]
    for (int i = -10; i <= 10; i++)
    {
        float2 offset = texCoord + (Direction * i * texelSize);
        color += tex2D(SpriteTextureSampler, offset) * Distribution[i + 10];
    }
    return color;
}

// Technique and Pass
technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};