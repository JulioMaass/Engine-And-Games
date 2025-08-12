// Preprocessor Directives
#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Shader configuration
float2 OutputSize;
float FrameCount;
float WiggleToggle;
float Curvature;
float Ghosting;
float Vignette;
float ScanRoll;

// Texture and Sampler Setup
Texture2D SpriteTexture;
sampler2D SpriteTextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
    Filter = Point;
};

// Vertex Shader Output Setup
struct VertexShaderOutput
{
    float4 Position : SV_POSITION0;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

float3 tsample(float2 tc, float offs, float2 resolution)
{
    // Adjust texture coordinates
    tc = tc * float2(1.025, 0.92) + float2(-0.0125, 0.04);
    float3 s = tex2D(SpriteTextureSampler, tc).rgb;
    
    // Gamma correction and brightness boost
    s = pow(abs(s), 2.2);
    return s * 1.25;
}

float3 filmic(float3 LinearColor)
{
    float3 x = max(0.0, LinearColor - 0.004);
    return (x * (6.2 * x + 0.5)) / (x * (6.2 * x + 1.7) + 0.06);
}

float2 curve(float2 uv)
{
    uv = (uv - 0.5); // * 2.0;
    //uv.x *= 0.75;
    uv *= float2(0.925, 1.095);
    uv *= Curvature;
    uv.x *= 1.0 + pow((abs(uv.y) / 4.0), 2.0);
    uv.y *= 1.0 + pow((abs(uv.x) / 3.0), 2.0);
    uv /= Curvature;
    uv += 0.5;
    uv = uv * 0.92 + 0.04;
    return uv;
}

float rand(float2 co)
{
    return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
}

float mod(float x, float y) // Implementation of GLSL mod for HLSL
{
    return x - y * floor(x / y);
}

// Pixel Shader
float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 resolution = OutputSize;
    float time = mod(FrameCount, 849.0) * 36.0;
    float2 uv = input.TextureCoordinates;
    
    // Curve
    float2 curved_uv = lerp(curve(uv), uv, 0.4);
    float scale = -0.101;
    float2 scuv = curved_uv * (1.0 - scale) + scale / 2.0 + float2(0.003, -0.001);
    uv = scuv;

    // Main Color, Bleed
    float x = WiggleToggle * sin(0.1 * time + curved_uv.y * 13.0) * sin(0.23 * time + curved_uv.y * 19.0) * sin(0.3 + 0.11 * time + curved_uv.y * 23.0) * 0.0012;
    // Add scanline oscillation effect
    float o = sin(input.Position.y * 2.0) / resolution.x; // 0.15
    x += o * 0.125; // 0.25;
    // color sampling with slight offsets for RGB channels to create color bleed effect
    float3 col;
    col.r = tsample(float2(x + scuv.x + 0.0009, scuv.y + 0.0009), resolution.y / 800.0, resolution).r + 0.02;
    col.g = tsample(float2(x + scuv.x + 0.0000, scuv.y - 0.0011), resolution.y / 800.0, resolution).g + 0.02;
    col.b = tsample(float2(x + scuv.x - 0.0015, scuv.y + 0.0000), resolution.y / 800.0, resolution).b + 0.02;
    
    // Ghosting
    float i = clamp(col.r * 0.299 + col.g * 0.587 + col.b * 0.114, 0.0, 1.0);
    i = pow(1.0 - pow(i, 2.0), 1.0);
    i = (1.0 - i) * 0.85 + 0.15;
    float ghs = 0.15 * Ghosting;
    float3 r = tsample(float2(x - 0.014 * 1.0, -0.027) * 0.85 + 0.007 * float2(0.35 * sin(1.0 / 7.0 + 15.0 * curved_uv.y + 0.9 * time),
        0.35 * sin(2.0 / 7.0 + 10.0 * curved_uv.y + 1.37 * time)) + float2(scuv.x + 0.001, scuv.y + 0.001),
        5.5 + 1.3 * sin(3.0 / 9.0 + 31.0 * curved_uv.x + 1.70 * time), resolution).xyz * float3(0.5, 0.25, 0.25);
    float3 g = tsample(float2(x - 0.019 * 1.0, -0.020) * 0.85 + 0.007 * float2(0.35 * cos(1.0 / 9.0 + 15.0 * curved_uv.y + 0.5 * time),
        0.35 * sin(2.0 / 9.0 + 10.0 * curved_uv.y + 1.50 * time)) + float2(scuv.x + 0.000, scuv.y - 0.002),
        5.4 + 1.3 * sin(3.0 / 3.0 + 71.0 * curved_uv.x + 1.90 * time), resolution).xyz * float3(0.25, 0.5, 0.25);
    float3 b = tsample(float2(x - 0.017 * 1.0, -0.003) * 0.85 + 0.007 * float2(0.35 * sin(2.0 / 3.0 + 15.0 * curved_uv.y + 0.7 * time),
        0.35 * cos(2.0 / 3.0 + 10.0 * curved_uv.y + 1.63 * time)) + float2(scuv.x - 0.002, scuv.y + 0.000),
        5.3 + 1.3 * sin(3.0 / 7.0 + 91.0 * curved_uv.x + 1.65 * time), resolution).xyz * float3(0.25, 0.25, 0.5);
    col += (ghs * (1.0 - 0.299)) * pow(clamp(3.0 * r, 0.0, 1.0), 2.0) * i;
    col += (ghs * (1.0 - 0.587)) * pow(clamp(3.0 * g, 0.0, 1.0), 2.0) * i;
    col += (ghs * (1.0 - 0.114)) * pow(clamp(3.0 * b, 0.0, 1.0), 2.0) * i;
    
    // Level adjustment (curves)
    col *= float3(0.95, 1.05, 0.95);
    col = clamp(col * 1.3 + 0.75 * col * col + 1.25 * col * col * col * col * col, 0.0, 10.0);
    
    // Vignette
    float vig = ((1.0 - 0.99 * Vignette) + 1.0 * 16.0 * curved_uv.x * curved_uv.y * (1.0 - curved_uv.x) * (1.0 - curved_uv.y));
    vig = 1.3 * pow(vig, 0.5);
    col *= vig;
    
    // Scanlines
    float scanRoll = time * ScanRoll;
    //float scans = clamp(0.35 + 0.18 * sin(6.0 * scanRoll - curved_uv.y * resolution.y * 2.0), 0.0, 1.0);
    float scans = clamp(0.35 + 0.1 * sin(6.0 * scanRoll - curved_uv.y * resolution.y * 2.20), 0.0, 1.0);
    float s = pow(scans, 0.9);
    col = col * s;
    
    // Vertical lines (shadow mask)
    col *= 1.0 - 0.23 * (clamp((mod(input.Position.xy.x, 3.0)) / 2.0, 0.0, 1.0));
    
    // Tone map
    col = filmic(col);
    
    // Noise
    float2 seed = curved_uv * resolution.xy;;
    col -= 0.015 * pow(float3(rand(seed + time), rand(seed + time * 2.0), rand(seed + time * 3.0)), 1.5);

    // Flicker
    col *= (1.0 - 0.004 * (sin(50.0 * time + curved_uv.y * 2.0) * 0.5 + 0.5));

    uv = curved_uv;
    //// Frame
    //float2 fscale = float2(0.026, -0.018);
    //uv = float2(uv.x, 1. - uv.y);
    //float4 f = FrameTexture.Sample(FrameTexture, input.TextureCoordinates);
    //f.xyz = lerp(f.xyz, float3(0.5, 0.5, 0.5), 0.5);
    //float fvig = clamp(-0.00 + 512.0 * uv.x * uv.y * (1.0 - uv.x) * (1.0 - uv.y), 0.2, 0.8);
    //col = lerp(col, lerp(max(col, 0.0), pow(abs(f.xyz), 1.4) * fvig, f.w * f.w), 1.0); // last float is "use frame"
    
    // Output final color
    return float4(col, 1.0);
}

// Technique and Pass
technique SpriteDrawing
{
    pass P0
    {
        PixelShader = compile PS_SHADERMODEL MainPS();
    }
};