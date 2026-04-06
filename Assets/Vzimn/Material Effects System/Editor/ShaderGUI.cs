using UnityEngine;
using UnityEditor;
using System.Web;

public class ShaderGUI : UnityEditor.ShaderGUI
{ 
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
       void DrawProperty(string property, string inspectorName)
        {
            materialEditor.ShaderProperty(FindProperty(property, properties), inspectorName);
        }
        void DrawProperty_v3(string property, string inspectorName)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;
            Vector3 v3 = rawProperty.vectorValue;
            v3 = EditorGUILayout.Vector3Field(inspectorName, v3);
            rawProperty.vectorValue = new Vector4(v3.x, v3.y,v3.z, v4.w);
        }

        //Main Texture
        DrawProperty("_frag_uv_world_local_view", "Space");
        DrawProperty_v3("_frag_plane_XYZ", "Plane");
        DrawProperty("_main_texture", "Texture");
        DrawProperty("_main_tint_color", "Color");
        DrawProperty("_Alpha_cliping_treshold", "Threshold");
        
        //Texture Distrorion
    }


}


