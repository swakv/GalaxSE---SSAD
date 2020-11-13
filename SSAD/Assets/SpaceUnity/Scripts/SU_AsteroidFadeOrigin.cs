/*  SU_AsteroidFadeOrigin Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB
    
    DESCRIPTION:
    This component tells the asteroid shader(s) of the visibility origin so the asteroid vertex shader can fade/scale
    asteroids at a given distance. Normally this component is added to the main camera. It can be done manually or
    alterlatively the SU_Asteroid.cs component adds it manually to the main camera during at runtime.

    Version History
    1.5     - New script for version 1.5.
*/
using UnityEngine;

public class SU_AsteroidFadeOrigin : MonoBehaviour
{
    // Used for shader int ID instead of string for perfomance since it's updated every frame
    private int _shaderIDOrigin;

    void Start()
    {
        // Grab the integer of the shader property for better performance since we update every frame
        _shaderIDOrigin = Shader.PropertyToID("_AsteroidOrigin");
    }

    void Update()
    {
        // Keep updating the shader(s) with the position of the main camera so it knows where to fade asteroids
        Shader.SetGlobalVector(_shaderIDOrigin, transform.position);
    }
}