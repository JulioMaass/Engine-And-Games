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
// Palette
bool ApplyPalette;
float PaletteIndex;
// Color
bool ApplyWhite;
bool ApplyGrayscale;


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

    // Apply Palette
    float2 paletteColorPosition = float2(color.r, PaletteIndex);
    float4 paletteColor = tex2D(PaletteTextureSampler, paletteColorPosition);
    color.rgb = lerp(color.rgb, paletteColor.rgb, ApplyPalette);
    
    // Apply White
    color.rgb = lerp(color.rgb, float3(1, 1, 1), ApplyWhite);
    
    // Apply Black and White
    color.rgb = lerp(color.rgb, float(color.r * 0.299 + color.g * 0.587 + color.b * 0.114), ApplyGrayscale);

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