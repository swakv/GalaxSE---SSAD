/*  SU_StaticStars Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

    DESCRIPTION:
    This script generates a procedural cube mesh which is used instead of the skybox from previous versions.
    The benefit of a custom approach is to save drawcalls because the original skybox component uses 6 draw calls
    but since we use the same texture in all directions this uses 1 draw call (or 2 if static nebula noise is enabled).
    Another benefit is that the color of stars and nebula noise can be tinted with this script.    

    Version History
    1.5     - New feature in version 1.5.
*/

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif


[ExecuteInEditMode]
public class SU_StaticStars : MonoBehaviour
{
    public Texture2D starsTexture;
    public bool hasTintedStars;
    public Color starsColor = Color.white;
    public float starsIntensity = 1.0f;
    public bool hasNoise;
    public Texture2D noiseTexture;
    public Color noiseColor = new Color(0.0f, 0.1f, 0.3f, 1.0f);

    public GameObject customSkybox;

    private MeshFilter _meshFilter;
    private Mesh _mesh;

    private float _side = 1.0f;


    void Awake()
    {
        if (gameObject.GetComponents<SU_StaticStars>().Length > 1)
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Error!", "Only one instance of this component can be used.", "OK");
                DestroyImmediate(this);
#endif
            }
            else {
                Destroy(this);
            }
            return;
        }
        if (gameObject.GetComponent<Camera>() != true)
        {
            if (Application.isEditor)
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Error!", "This component can only be added to a camera.", "OK");
                DestroyImmediate(this);
#endif
            }
            else {
                Destroy(this);
            }
            return;
        }
    }

    void OnEnable()
    {
        // If the custom skybox is null...
        if (customSkybox == null)
            CreateCustomSkybox();

        if (_meshFilter == null || _mesh == null)
            CreateCustomMesh();

        SetShader();

        customSkybox.transform.position = transform.position;
        customSkybox.transform.rotation = Quaternion.identity;
        customSkybox.transform.parent = transform;
        customSkybox.gameObject.layer = gameObject.layer;

        // Hide the transform from the Hierarchy to prevent manual editing and unnecessary confusion =)
        // skyboxTransform.hideFlags = HideFlags.HideInHierarchy;
        customSkybox.hideFlags = 0;

        // Disable skybox on object if present
        if (transform.GetComponent<Skybox>() != null)
            transform.GetComponent<Skybox>().enabled = false;

        // Ensure the skybox is active
        if (customSkybox != null) customSkybox.gameObject.SetActive(true);

    }

    void CreateCustomSkybox()
    {
        // Create a new gameobject and reference the transform
        customSkybox = new GameObject("CustomSkybox");
        // Set the parent to this transform
        customSkybox.transform.parent = transform;
        // Create the custom mesh
        CreateCustomMesh();
    }

    void CreateCustomMesh()
    {
        if (customSkybox.gameObject.GetComponent<MeshFilter>() != null)
            DestroyImmediate(customSkybox.gameObject.GetComponent<MeshFilter>());

        // Add mesh filder component
        _meshFilter = customSkybox.gameObject.AddComponent<MeshFilter>();

        // Procedurally generate a cube mesh
        _mesh = GenCube(_side);
        _meshFilter.mesh = _mesh;

        // Add mesh renderer component
        if (customSkybox.gameObject.GetComponent<MeshRenderer>() != null)
            DestroyImmediate(customSkybox.gameObject.GetComponent<MeshRenderer>());

        MeshRenderer _meshRenderer = customSkybox.gameObject.AddComponent<MeshRenderer>();
        _meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        _meshRenderer.receiveShadows = false;
        _meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        _meshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

    }

    void OnDisable()
    {
        // The component was disabled, disable the custom skybox to hide the stars
       // if (skyboxTransform != null) skyboxTransform.gameObject.SetActive(false);
    }

    void OnPreCull()
    {
        // Rotation of the cube is always reset in Play mode to act as a skybox (to prevent it from rotating with the camera)        
        if (customSkybox != null)
            customSkybox.transform.rotation = Quaternion.identity;
    }

    void LateUpdate()
    {        

#if UNITY_EDITOR
        // Only update the shader in editor mode, in playmode it's taken care of in OnEnable()
        if (EditorApplication.isPlaying)
            return;

        // If there appears to be no skyboxTransform, recreate it and return
        if (customSkybox == null)
        {
            CreateCustomSkybox();
            return;
        }

        if (_meshFilter == null || _mesh == null)
        {
            CreateCustomMesh();
            return;
        }

        if (customSkybox.gameObject.GetComponent<Renderer>().sharedMaterial == null)
        {
            // There seems to be no shaderd material, recreate it within the SetShader Method and return
            SetShader();
            return;
        }

        // Update shader to reflect changes to Static Stars instantly		
        if (customSkybox.GetComponent<Renderer>().sharedMaterial.GetTexture("_MainTex") != starsTexture)
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", starsTexture);

        if (hasNoise != customSkybox.GetComponent<Renderer>().sharedMaterial.HasProperty("_NoiseTex"))
        {
            SetShader();
            return;
        }

        if (hasTintedStars != customSkybox.GetComponent<Renderer>().sharedMaterial.HasProperty("_StarsColor"))
        {
            SetShader();
            return;
        }

        if (hasNoise)
        {
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetColor("_NoiseColor", noiseColor);
            if (customSkybox.GetComponent<Renderer>().sharedMaterial.GetTexture("_NoiseTex") != noiseTexture)
                customSkybox.GetComponent<Renderer>().sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
        }

        if (hasTintedStars)
        {
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetColor("_StarsColor", starsColor);
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetFloat("_StarsIntensity", starsIntensity);
        }
#endif

    }

    void SetShader()
    {
        // Determine the appropriate shader depending on if tinted stars and/or noise is used
        string _shaderName = "SU_StaticStars";
        if (hasTintedStars) _shaderName += "_Tint";
        if (hasNoise) _shaderName += "_Noise";

        // If material already exists, destroy it
        if (customSkybox.GetComponent<Renderer>().sharedMaterial != null) DestroyImmediate(customSkybox.GetComponent<Renderer>().sharedMaterial);

        // Create new material with the appropriate shader
        customSkybox.GetComponent<Renderer>().sharedMaterial = new Material(Shader.Find("SpaceUnity/" + _shaderName));
        // Set the stars texture for the shader which is always required
        customSkybox.GetComponent<Renderer>().sharedMaterial.SetTexture("_MainTex", starsTexture);

        // If tinted stars are used, set the color and intensity values for the shader
        if (hasTintedStars)
        {
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetColor("_StarsColor", starsColor);
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetFloat("_StarsIntensity", starsIntensity);
        }

        // If nebula noise is used, set the texture and color values for the shader
        if (hasNoise)
        {
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
            customSkybox.GetComponent<Renderer>().sharedMaterial.SetColor("_NoiseColor", noiseColor);
        }

    }

    // Procedural cube from http://wiki.unity3d.com/index.php/ProceduralPrimitives
    Mesh GenCube(float _sideLength) {        
		Mesh _mesh = new Mesh();        

        Vector3 _p0 = new Vector3( -_sideLength * .5f,	-_sideLength * .5f, _sideLength * .5f );
		Vector3 _p1 = new Vector3( _sideLength * .5f, 	-_sideLength * .5f, _sideLength * .5f );
		Vector3 _p2 = new Vector3( _sideLength * .5f, 	-_sideLength * .5f, -_sideLength * .5f );
		Vector3 _p3 = new Vector3( -_sideLength * .5f,	-_sideLength * .5f, -_sideLength * .5f );	
		 
		Vector3 _p4 = new Vector3( -_sideLength * .5f,	_sideLength * .5f,  _sideLength * .5f );
		Vector3 _p5 = new Vector3( _sideLength * .5f, 	_sideLength * .5f,  _sideLength * .5f );
		Vector3 _p6 = new Vector3( _sideLength * .5f, 	_sideLength * .5f,  -_sideLength * .5f );
		Vector3 _p7 = new Vector3( -_sideLength * .5f,	_sideLength * .5f,  -_sideLength * .5f );
		 
		Vector3[] _vertices = new Vector3[]
		{
			// Bottom
			_p0, _p1, _p2, _p3,
		 
			// Left
			_p7, _p4, _p0, _p3,
		 
			// Front
			_p4, _p5, _p1, _p0,
		 
			// Back
			_p6, _p7, _p3, _p2,
		 
			// Right
			_p5, _p6, _p2, _p1,
		 
			// Top
			_p7, _p6, _p5, _p4
		};

		
		Vector3[] _normales = new Vector3[]
		{
			// Bottom
			Vector3.up, Vector3.up, Vector3.up, Vector3.up,
		 
			// Left
			Vector3.right, Vector3.right, Vector3.right, Vector3.right,
		 
			// Front			
		 	Vector3.back, Vector3.back, Vector3.back, Vector3.back,
			
			// Back			
			Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward,
		 
			// Right			
			Vector3.left, Vector3.left, Vector3.left, Vector3.left,
		 
			// Top			
			Vector3.down, Vector3.down, Vector3.down, Vector3.down
		};

		Vector2 _00 = new Vector2( 0f, 0f );
		Vector2 _10 = new Vector2( 1f, 0f );
		Vector2 _01 = new Vector2( 0f, 1f );
		Vector2 _11 = new Vector2( 1f, 1f );
		 
		Vector2[] _uvs = new Vector2[]
		{
			// Bottom
			_11, _01, _00, _10,
		 
			// Left
			_11, _01, _00, _10,
		 
			// Front
			_11, _01, _00, _10,
		 
			// Vector3.back
			_11, _01, _00, _10,
		 
			// Right
			_11, _01, _00, _10,
		 
			// Top
			_11, _01, _00, _10,
		};

		int[] _triangles = new int[]
		{
			// Bottom
			0, 1, 3,
			1, 2, 3,			
		 
			// Left
			0 + 4 * 1, 1 + 4 * 1, 3 + 4 * 1,
			1 + 4 * 1, 2 + 4 * 1, 3 + 4 * 1,
		 
			// Front
			0 + 4 * 2, 1 + 4 * 2, 3 + 4 * 2,
			1 + 4 * 2, 2 + 4 * 2, 3 + 4 * 2,
		 
			// Back
			0 + 4 * 3, 1 + 4 * 3, 3 + 4 * 3,
			1 + 4 * 3, 2 + 4 * 3, 3 + 4 * 3,
		 
			// Right
			0 + 4 * 4, 1 + 4 * 4, 3 + 4 * 4,
			1 + 4 * 4, 2 + 4 * 4, 3 + 4 * 4,
		 
			// Top
			0 + 4 * 5, 1 + 4 * 5, 3 + 4 * 5,
			1 + 4 * 5, 2 + 4 * 5, 3 + 4 * 5,
		 
		};
		 
		_mesh.vertices = _vertices;
		_mesh.normals = _normales;
		_mesh.uv = _uvs;
		_mesh.triangles = _triangles;
		 
		_mesh.RecalculateBounds();
		;	
		return _mesh;
	}
}
