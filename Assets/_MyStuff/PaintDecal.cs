using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintDecal : MonoBehaviour
{
    public int resolution = 512;
    public string textureToPaintTo = "_MainTex";  
    public float decalSize = 0.2f;
    public bool overrideTexture = true;
    public Texture2D decalTexture;
    public Color decalColor = Color.white;
    Texture2D clearMap;
    RenderTexture rTexture;
    RenderTexture resultTex;
    Projector proj;
    Camera cam;

    public static Dictionary<Collider, RenderTexture> paintTextures = new Dictionary<Collider, RenderTexture>();
    void Start()
    {
        proj = GetComponent<Projector>();
        cam = GetComponentInChildren<Camera>();
        cam.orthographicSize = 1000;
        SetProjector();       
        CreateClearTexture();// clear white texture to draw on     
        rTexture = GetClearRT(rTexture);
        resultTex = GetClearRT(resultTex);
        cam.targetTexture = rTexture;
    }

    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 20f, Color.magenta);
        RaycastHit hit; 
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) // delete previous and uncomment for mouse painting
        {          
           transform.LookAt(hit.point);
           Collider coll = hit.collider;
           if (Input.GetMouseButtonDown(0))
           {
                if (coll != null && coll.gameObject.layer == 30)
                {
                    Renderer rend = hit.transform.root.GetComponentInChildren<Renderer>();                    
                    if (!paintTextures.ContainsKey(coll)) // if there is already paint on the material, add to that, otherwise add new one
                    {
                        paintTextures.Add(coll, GetClearRT(new RenderTexture(resolution, resolution, 32)));
                    }                 
                 ProjectDecal(decalTexture, decalSize, decalColor, paintTextures[coll], rend, textureToPaintTo);          
                }
           }
        }
    }

    void ProjectDecal(Texture decalTex, float decalSize, Color decalColor, RenderTexture rt, Renderer rend, string textureString)
    {
        SetProjector();
        if (overrideTexture)
        {
            DrawTexture(rTexture, rt, rend.material.GetTexture(textureString), true);
        }
        else
        {
            DrawTexture(rTexture, rt, null, false);
        }      
       rend.material.SetTexture(textureString, rt);       
    }

  
    void DrawTexture(RenderTexture rt, RenderTexture result, Texture baseMaterial, bool drawOver)
    {         
        RenderTexture.active = result; // activate rendertexture for drawtexture;
        GL.PushMatrix();                       // save matrixes
        GL.LoadPixelMatrix(0, rt.width, rt.height, 0);      // setup matrix for correct size
        if (drawOver)
        {
            Graphics.Blit(baseMaterial, result);
        }      
        Graphics.DrawTexture(new Rect(0, 0, rt.width, rt.height), rt);
        GL.PopMatrix();
        RenderTexture.active = null;// turn off rendertexture               
    }

    RenderTexture GetClearRT(RenderTexture rt)
    {
        rt = new RenderTexture(resolution, resolution, 32);
        Graphics.Blit(clearMap, rt);
        return rt;
    }

    void CreateClearTexture()
    {
        clearMap = new Texture2D(1, 1);
        clearMap.SetPixel(0, 0, Color.clear);
        clearMap.Apply();
    }

    void SetProjector()
    {
        proj.orthographicSize = decalSize;
        proj.material.SetTexture("_Decal", decalTexture);
        proj.material.color = decalColor;
    }
}