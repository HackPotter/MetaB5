
#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4  || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#define UNITY_4
#endif

using System;
using System.Timers;
using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Color3 Grading")]
sealed public class Color3Grading : MonoBehaviour
{
	private ColorSpace colorSpace = ColorSpace.Uninitialized;

	private Shader shaderGrading;
	private Shader shaderBlend;
	private Shader shaderMask;
	private Shader shaderBlendMask;

	public float BlendAmount = 0f;
	public Texture2D LutTexture;
	public Texture2D LutBlendTexture;
	public Texture2D MaskTexture;

	private bool use3d = false;
	private Texture lutTexture3d = null;
	private Texture lutBlendTexture3d = null;
#if UNITY_4	
	private bool managedLutTexture3d = false;
	private bool managedLutBlendTexture3d = false;
	public Texture3D LutTexture3d { get { return lutTexture3d as Texture3D; } set { lutTexture3d = value; } }
	public Texture3D LutBlendTexture3d { get { return lutBlendTexture3d as Texture3D; } set { lutBlendTexture3d = value; } }
#endif

	private Material materialGrading;
	private Material materialBlend;
	private Material materialMask;
	private Material materialBlendMask;

	private bool blending;
	private float blendingTime;
	private float blendingTimeCountdown;

	public delegate void OnFinishBlend();
	private OnFinishBlend onFinishBlend;

	public bool IsBlending { get { return blending; } }
	
	internal bool JustCopy = false;

#if TRIAL
	private Texture2D m_watermark = null;
#endif

	public bool WillItBlend { get { return LutTexture != null && LutBlendTexture != null && !blending; } }

	void ReportMissingShaders()
	{
		Debug.LogError( "[Color3] Error initializing shaders. Please reinstall Color3." );
		enabled = false;
	}

	void ReportNotSupported()
	{
		Debug.LogError( "[Color3] This image effect is not supported on this platform. Please make sure your Unity license supports Full-Screen Post-Processing Effects which is usually reserved forn Pro licenses." );
		enabled = false;
	}

	bool CheckShader( Shader s )
	{
		if ( s == null )
		{
			ReportMissingShaders();
			return false;
		}
		if ( !s.isSupported )
		{
			ReportNotSupported();
			return false;
		}
		return true;
	}

	bool CheckShaders()
	{
		return CheckShader( shaderGrading ) && CheckShader( shaderBlend ) && CheckShader( shaderMask ) && CheckShader( shaderBlendMask );
	}

	bool CheckSupport()
	{
		// Disable if we don't support image effect or render textures
		if ( !SystemInfo.supportsImageEffects )
		{
			ReportNotSupported();
			return false;
		}
		return true;
	}

	void OnEnable()
	{
		if ( !CheckSupport() )
			return;
		
		CreateMaterials( false );

		if ( ( LutTexture != null && LutTexture.mipmapCount > 1 ) || ( LutBlendTexture != null && LutBlendTexture.mipmapCount > 1 ) )
			Debug.LogError( "[Color3] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. " +
				"Change Texture Type to \"Advanced\" to access Mip settings." );

	#if TRIAL
		m_watermark = new Texture2D( 4, 4 );
		m_watermark.LoadImage( Watermark.ImageData );
	#endif
	}

	void OnDisable()
	{
		ReleaseShaders();
		ReleaseTextures();

	#if TRIAL
		DestroyImmediate( m_watermark );
	#endif
	}

	public void BlendTo(Texture2D blendTargetLUT, float blendTimeInSec, OnFinishBlend onFinishBlend)
	{
		LutBlendTexture = blendTargetLUT;
		BlendAmount = 0.0f;
		this.onFinishBlend = onFinishBlend;
		blendingTime = blendTimeInSec;
		blendingTimeCountdown = blendTimeInSec;
		blending = true;
	}

	private void Update()
	{
		if ( blending )
		{
			BlendAmount = ( blendingTime - blendingTimeCountdown ) / blendingTime;
			blendingTimeCountdown -= Time.smoothDeltaTime;

			if ( BlendAmount >= 1.0f )
			{
				LutTexture = LutBlendTexture;
				BlendAmount = 0.0f;
				blending = false;
				LutBlendTexture = null;

				if ( onFinishBlend != null )
					onFinishBlend();
			}
		}
		else
		{
			BlendAmount = Mathf.Clamp01( BlendAmount );
		}
	}

	private void SetupShader( bool fallback )
	{
		colorSpace = QualitySettings.activeColorSpace;
		string linear = ( colorSpace == ColorSpace.Linear ) ? "Linear" : "";
		string ext3d = "";
	#if UNITY_4
		if ( SystemInfo.supports3DTextures && !fallback )
		{
			ext3d = "3d";
			use3d = true;
		}
	#endif

		shaderGrading = Shader.Find( "Hidden/Color3" + linear + ext3d );
		shaderBlend = Shader.Find( "Hidden/Color3Blend" + linear + ext3d );
		shaderMask = Shader.Find( "Hidden/Color3Mask" + linear + ext3d );
		shaderBlendMask = Shader.Find( "Hidden/Color3BlendMask" + linear + ext3d );
	}

	private void ReleaseShaders()
	{
		if ( materialGrading != null )
		{
			DestroyImmediate( materialGrading );
			materialGrading = null;
		}
		if ( materialBlend != null )
		{
			DestroyImmediate( materialBlend );
			materialBlend = null;
		}
		if ( materialMask != null )
		{
			DestroyImmediate( materialMask );
			materialMask = null;
		}
		if ( materialBlendMask != null )
		{
			DestroyImmediate( materialBlendMask );
			materialBlendMask = null;
		}
	}	

	private void CreateMaterials( bool fallback )
	{
		SetupShader( fallback );

		if ( !CheckShaders() )
			return;

		ReleaseShaders();

		materialGrading = new Material( shaderGrading ) { hideFlags = HideFlags.HideAndDontSave };
		materialBlend = new Material( shaderBlend ) { hideFlags = HideFlags.HideAndDontSave };
		materialMask = new Material( shaderMask ) { hideFlags = HideFlags.HideAndDontSave };
		materialBlendMask = new Material( shaderBlendMask ) { hideFlags = HideFlags.HideAndDontSave };
	}

#if UNITY_4
	private void ReleaseLutTexture3d()
	{
		if ( lutTexture3d != null )
		{
			DestroyImmediate( lutTexture3d );
			lutTexture3d = null;
			managedLutTexture3d = false;
		}
	}

	private void ReleaseLutBlendTexture3d()
	{
		if ( lutBlendTexture3d != null )
		{
			DestroyImmediate( lutBlendTexture3d );
			lutBlendTexture3d = null;
			managedLutBlendTexture3d = false;
		}
	}
#endif

	private void ReleaseTextures()
	{
	#if UNITY_4
		if ( use3d )
		{
			if ( managedLutTexture3d )
				ReleaseLutTexture3d();

			if ( managedLutBlendTexture3d )
				ReleaseLutBlendTexture3d();
		}
	#endif
	}
	
	public static bool ValidateLutDimensions( Texture2D lut )
	{
		bool valid = true;
		if ( lut != null )
		{
			if ( ( lut.width / lut.height) != lut.height )
			{
				Debug.LogWarning( "[Color3] Lut " + lut.name + " has invalid dimensions." );
				valid = false;
			}
			else
			{
				if ( lut.anisoLevel != 0 )
					lut.anisoLevel = 0;	
			}
		}
		return valid;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		BlendAmount = Mathf.Clamp01( BlendAmount );

		if ( colorSpace != QualitySettings.activeColorSpace )
			CreateMaterials( false );

		bool validLut = ValidateLutDimensions( LutTexture );
		bool validLutBlend = ValidateLutDimensions( LutBlendTexture );

		if ( JustCopy || !validLut || !validLutBlend )
		{
			Graphics.Blit( source, destination );
			return;
		}

		if ( LutTexture == null && lutTexture3d == null )
		    Graphics.Blit( source, destination );
		else
		{
		    Material material;
			if ( LutBlendTexture != null || lutBlendTexture3d != null )
			{
				if ( MaskTexture != null )
					material = materialBlendMask;
				else
					material = materialBlend;
			}
			else
			{
				if ( MaskTexture != null )
					material = materialMask;
				else
					material = materialGrading;
			}

			int pass = ( !GetComponent<Camera>().hdr && BlendAmount == 0.0f ) ? 0 : 1;

			material.SetFloat( "_lerpAmount", BlendAmount );			
			if ( MaskTexture != null )
				material.SetTexture( "_MaskTex", MaskTexture );

		#if UNITY_4
			if ( use3d )
			{
				CacheConvertLutsTo3D();

				bool failedLut = ( LutTexture != null && LutTexture3d == null );
				bool failedLutBlend = ( LutBlendTexture != null && LutBlendTexture3d == null );
				if ( failedLut || failedLutBlend )
				{
					CreateMaterials( true );
					use3d = false;
					Debug.LogWarning( "[Color3] For optimal performance, please ensure LUTs are 1024x32 and have \"Read/Write Enabled\", for optimal performance. Falling back to standard method." );
					return;
				}
			}

			if ( use3d )
			{
				if ( LutTexture3d != null )
					material.SetTexture( "_RgbTex3d", LutTexture3d );
				if ( LutBlendTexture3d != null )
					material.SetTexture( "_LerpRgbTex3d", LutBlendTexture3d );
			}
#endif

			if ( !use3d )
			{
				if ( LutTexture != null )
					material.SetTexture( "_RgbTex", LutTexture );
				if ( LutBlendTexture != null )
					material.SetTexture( "_LerpRgbTex", LutBlendTexture );
			}

			Graphics.Blit( source, destination, material, pass );
		}
	}

#if UNITY_4
	public void CacheConvertLutsTo3D()
	{
		if ( LutTexture != null )
		{
			if ( lutTexture3d == null || lutTexture3d.name != LutTexture.GetHashCode().ToString() )
			{
				lutTexture3d = ConvertLutTo3D( LutTexture );
				managedLutTexture3d = true;
			}
		}
		else if ( managedLutTexture3d )
			ReleaseLutTexture3d();		

		if ( LutBlendTexture != null )
		{
			if ( lutBlendTexture3d == null || lutBlendTexture3d.name != LutBlendTexture.GetHashCode().ToString() )
			{
				lutBlendTexture3d = ConvertLutTo3D( LutBlendTexture );
				managedLutBlendTexture3d = true;
			}
		}
		else if ( managedLutBlendTexture3d )
			ReleaseLutBlendTexture3d();
	}

	public static Texture3D ConvertLutTo3D( Texture2D lut )
	{
		Texture3D lut3d = null;

		if ( lut != null )
		{
			if ( ValidateLutDimensions( lut ) )
			{
				try
				{
					Color[] src = lut.GetPixels();
					Color[] dst = new Color[ src.Length ];

					for ( int x = 0; x < 32; x++ )
					{
						for ( int y = 0; y < 32; y++ )
						{
							int src_off = x + ( y << 10 );
							int dst_off = x + ( y << 5 );

							for ( int z = 0; z < 32; z++ )
								dst[ dst_off + ( z << 10 ) ] = src[ src_off + ( z << 5 ) ];
						}
					}

					lut3d = new Texture3D( 32, 32, 32, TextureFormat.ARGB32, false );
					lut3d.SetPixels( dst );
					lut3d.Apply();
				}
				catch ( System.Exception )
				{
				}
			}

			if ( lut3d == null )
				Debug.LogError( "[Color3] ConvertLutTo3D failed on \"" + lut.name + "\"." );
			else
				lut3d.name = lut.GetHashCode().ToString();
		}	
		else
			Debug.LogError( "[Color3] An invalid/null Lut texture was passed into ConvertLutTo3D" );

		return lut3d;
	}
#endif

#if TRIAL
	void OnGUI()
	{
		if ( m_watermark != null )
			GUI.DrawTexture( new Rect( 10, Screen.height - m_watermark.height, m_watermark.width, m_watermark.height ), m_watermark );
	}
#endif
}