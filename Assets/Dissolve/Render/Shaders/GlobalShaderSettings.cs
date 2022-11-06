using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderSettings : MonoBehaviour
{
    public Texture2D DissolveTex;
    public Texture2D RampTex;

    [Range(-1,1)]
    public float ClipValue = -1;

    void Awake()
    {
        
    }

    private void OnValidate()
    {
        Shader.SetGlobalTexture("_RampTex", RampTex);
        Shader.SetGlobalTexture("_DissolveTex", DissolveTex);
        Shader.SetGlobalFloat("_Clip", ClipValue);

        Debug.Log("_ClipValue:" + ClipValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        Shader.SetGlobalTexture("_RampTex", RampTex);
        Shader.SetGlobalTexture("_DissolveTex", DissolveTex);
        Shader.SetGlobalFloat("_Clip", ClipValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
