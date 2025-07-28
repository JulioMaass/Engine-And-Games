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

Texture2D PaletteTexture;
sampler2D PaletteTextureSampler = sampler_state
{
    Texture = <PaletteTexture>;
    Filter = Point;
};

// Outside Variables
bool IsOn;
float PaletteIndex;

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

    // Bypass
    if (!IsOn)
    {
        return color;
    }
    
    // Get palette color
    float originalAlpha = color.a;
    float2 paletteColorPosition = float2(color.r, PaletteIndex);
    color = tex2D(PaletteTextureSampler, paletteColorPosition);
    color.a = originalAlpha;
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