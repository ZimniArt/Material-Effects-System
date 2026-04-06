using UnityEngine;
using UnityEditor;
using System.Web;

public class ShaderGUI : UnityEditor.ShaderGUI
{ 
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
       void DrawProperty(string property, string inspectorName)
        {
            materialEditor.ShaderProperty(FindProperty(property, properties), "Space");
        }
        //Main Texture
        DrawProperty("_frag_uv_world_local_view", "Space");
        DrawProperty("_frag_plane_XYZ", "Plane");
        DrawProperty("_main_texture", "Texture");
        DrawProperty("_main_tint_color", "Color");
        DrawProperty("_Alpha_cliping_treshold", "Threshold");
        
        //Texture Distrorion
    }


}


