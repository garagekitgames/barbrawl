using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSetter : MonoBehaviour
{
    public Color[] wallColors;
    public Color[] groundColors;
    public Material wallMaterial;
    public Material floorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        floorMaterial = this.GetComponent<MeshRenderer>().materials[0];
        wallMaterial = this.GetComponent<MeshRenderer>().materials[1];

        int selectedColor =  Random.Range(0, wallColors.Length);
        wallMaterial.color = wallColors[selectedColor];

       // selectedColor = Random.Range(0, groundColors.Length);
        floorMaterial.color = groundColors[selectedColor];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
