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
float2 ScreenSize;
int LightCount;
float2 LightPositions[16];
float Radius1[16];
float Radius2[16];
float3 Colors[3];

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
    float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
    
    // Light Calculation
    int lightLevel = 0; // 0 = dark, 1 = dim light, 2 = bright light
    for (int i = 0; i < LightCount; i++)
    {
        float dist = distance(input.TextureCoordinates * ScreenSize, LightPositions[i]);
        if (dist < Radius1[i])
        {
            lightLevel = 2;
            break;
        }
        if (dist < Radius2[i])
            lightLevel = 1;
    }
    float3 lightValue = Colors[lightLevel];
        
    color.rgb *= lightValue;
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