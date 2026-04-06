using UnityEngine;
using UnityEditor;

public class ShaderGUI : UnityEditor.ShaderGUI
{ 
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        //Main Texture

        MaterialProperty MainSpace = FindProperty("_frag_uv_world_local_view", properties);
        materialEditor.ShaderProperty(MainSpace, "Space");

        MaterialProperty MainPlane = FindProperty("_frag_plane_XYZ", properties);
        materialEditor.ShaderProperty(MainPlane, "Plane");

        MaterialProperty MainTexture = FindProperty("_main_texture", properties);
        materialEditor.ShaderProperty(MainTexture, "Defuse");

        MaterialProperty color = FindProperty("_main_tint_color", properties);
        materialEditor.ShaderProperty(color, "Color");

        MaterialProperty AlphaClippingThreshold = FindProperty("_Alpha_cliping_treshold", properties);
        materialEditor.ShaderProperty(AlphaClippingThreshold, "Alpha Clipping Threshold");



    }
}


