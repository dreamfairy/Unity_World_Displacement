using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalShaderSettings : MonoBehaviour
{
    public Texture2D DissolveTex;
    public Texture2D RampTex;
    public Transform CenterDissolvePos;
    public bool AutoDissolve = false;
    public float DissolveSpeed = 1.0f;
    public float CurrentDissolve = 0.0f;
    [Range(0,150)]
    public float DissolveRadius = 0;

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

        if(CenterDissolvePos)
        {
            Shader.SetGlobalVector("_DissolveCenter", new Vector4(CenterDissolvePos.position.x, CenterDissolvePos.position.y, CenterDissolvePos.position.z, DissolveRadius));
        }
      

        //Debug.Log("_ClipValue:" + ClipValue);
    }

    // Start is called before the first frame update
    void Start()
    {
        Shader.EnableKeyword("REQUIRES_WORLD_SPACE_POS_INTERPOLATOR");
        Shader.SetGlobalTexture("_RampTex", RampTex);
        Shader.SetGlobalTexture("_DissolveTex", DissolveTex);
        Shader.SetGlobalFloat("_Clip", ClipValue);
        Shader.SetGlobalVector("_DissolveCenter", Vector4.zero);

        CurrentDissolve = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(AutoDissolve)
        {
            CurrentDissolve += DissolveSpeed;

            if (CenterDissolvePos)
            {
                Shader.SetGlobalVector("_DissolveCenter", new Vector4(CenterDissolvePos.position.x, CenterDissolvePos.position.y, CenterDissolvePos.position.z, CurrentDissolve));
            }
        }
    }
}
