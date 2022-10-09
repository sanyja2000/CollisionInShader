using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class clickhandler : MonoBehaviour
{
    public RenderTexture selectionTexture;
    public Shader clickShader;
    public Camera clickCamera;
    GameObject[] renderables;
    public Texture2D texture;
    Dictionary<uint, GameObject> renderedGameObjects;
    Color32 nothingFound;
    public Color32 UIntToColor(uint number)
    {
        var intBytes = System.BitConverter.GetBytes(number);

        return new Color32(intBytes[0], intBytes[1], intBytes[2], intBytes[3]);
    }
    public byte[] ColorToByteArray(Color32 color)
    {
        return new[] { color.r, color.g, color.b, color.a };
    }
    public uint ColorToUInt(Color32 color)
    {
        return System.BitConverter.ToUInt32(ColorToByteArray(color), 0);
    }
    // Start is called before the first frame update
    void Start()
    {
        renderables = GameObject.FindGameObjectsWithTag("clickable");
        selectionTexture = new RenderTexture(Screen.width, Screen.height, 0)
        {
            antiAliasing = 1,
            filterMode = FilterMode.Point,
            autoGenerateMips = false,
            depth = 24
        };
        texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        clickCamera.targetTexture = selectionTexture;
        renderedGameObjects = new Dictionary<uint, GameObject>();
        foreach (var go in renderables)
        {
            renderedGameObjects.Add((uint)go.GetInstanceID(), go);
            MaterialPropertyBlock props = new MaterialPropertyBlock();
            props.SetColor("_SelectionColor", UIntToColor((uint)go.GetInstanceID()));
            go.GetComponent<Renderer>().SetPropertyBlock(props);
            
        }
        nothingFound = new Color(0,0,0,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            clickCamera.RenderWithShader(clickShader, "");

            RenderTexture.active = selectionTexture;

            // Read pixels
            texture.ReadPixels(new Rect(0,0,Screen.width,Screen.height), 0, 0);
            texture.Apply();
            Color readColor = texture.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);

            RenderTexture.active = null; // added to avoid errors 
            if (readColor != nothingFound) {
                
                uint instanceID = ColorToUInt(readColor);
                Debug.Log(renderedGameObjects[instanceID].name);

            }

        }
    }
}
