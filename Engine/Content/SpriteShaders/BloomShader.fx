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
int TargetWidth;
int TargetHeight;

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
    float2 texelSize = float2(1.0f / (float) TargetWidth, 1.0f / (float)TargetHeight);
    float4 color = float4(0, 0, 0, 0);
    
    // Blur weights
    const float weights[21] =
    {
        0.025,
        0.05,
        0.075,
        0.10,
        0.125,
        0.15,
        0.16,
        0.17,
        0.18,
        0.19,
        0.20,
        0.19,
        0.18,
        0.17,
        0.16,
        0.15,
        0.125,
        0.10,
        0.075,
        0.05,
        0.025
        
        //// Sin based - Divide the result by 5 to keep same intensity as the above
        //0.0500,
        //0.1564,
        //0.3090,
        //0.4540,
        //0.5878,
        //0.7071,
        //0.8090,
        //0.8910,
        //0.9511,
        //0.9877,
        //1.0000,
        //0.9877,
        //0.9511,
        //0.8910,
        //0.8090,
        //0.7071,
        //0.5878,
        //0.4540,
        //0.3090,
        //0.1564,
        //0.0500
    };
    
    // Sample and accumulate
    for (int i = -10; i <= 10; i++)
    {
        float2 offset = texCoord + (Direction * i * texelSize);
        color += tex2D(SpriteTextureSampler, offset) * weights[i + 10];
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