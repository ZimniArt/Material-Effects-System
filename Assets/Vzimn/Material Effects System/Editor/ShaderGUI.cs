using UnityEngine;
using UnityEditor;
using System.Web;
using UnityEditor.SearchService;

public class ShaderGUI : UnityEditor.ShaderGUI
{

        bool vis_MainTextureDistortion = false;
        bool vis_UnderlayTexture = false;
        bool vis_Disolve = false;
        bool vis_dissolveTexture = false;
        bool vis_dissolveControls = false;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material material = materialEditor.target as Material;

        #region Functions
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
            rawProperty.vectorValue = new Vector4(v3.x, v3.y, v3.z, v4.w);
        }
        void v4_DropDown(string property, string inspectorName, string[] options)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;

            int selected = 0;

            if (v4.x == 1) selected = 0;
            else if (v4.y == 1) selected = 1;
            else if (v4.z == 1) selected = 2;
            else if (v4.w == 1) selected = 3;

            selected = EditorGUILayout.Popup(inspectorName, selected, options);

            Vector4 new_v4 = Vector4.zero;
            if (selected == 0) new_v4.x = 1;
            else if (selected == 1) new_v4.y = 1;
            else if (selected == 2) new_v4.z = 1;
            else if (selected == 3) new_v4.w = 1;
            rawProperty.vectorValue = new_v4;
        }
        int Get_v4_index(string property)
        {
            var rawProperty = FindProperty(property, properties);
            Vector4 v4 = rawProperty.vectorValue;
            int index = 0;

            if (v4.x == 1) index = 0;
            else if (v4.y == 1) index = 1;
            else if (v4.z == 1) index = 2;
            else if (v4.w == 1) index = 3;
            return index;
        }
        
        void FoldOut(ref bool active, ref bool visible, string label, System.Action drawContent)
        {
            EditorGUILayout.BeginHorizontal();
            active = EditorGUILayout.Toggle(active, GUILayout.Width(20));
            visible = GUILayout.Toggle(visible, label, EditorStyles.foldoutHeader);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!active);
            if (visible)
            {
                drawContent();
            }
            EditorGUI.EndDisabledGroup();
        }
        void Fold_simple(ref bool visible, string label, System.Action drawContent)
        {
            visible = EditorGUILayout.Foldout(visible, label);
            if (visible) drawContent();
        }

        void FoldOut_keyword(string keyword, ref bool visible, string label, System.Action drawContent)
        {
            bool keyword_state = material.IsKeywordEnabled(keyword);

            EditorGUILayout.BeginHorizontal();

            bool new_keyword_state = EditorGUILayout.Toggle(keyword_state, GUILayout.Width(20));

            if (new_keyword_state != keyword_state)
            {
                if(new_keyword_state == true)   material.EnableKeyword(keyword);
                else                            material.DisableKeyword(keyword);
            }
            visible = GUILayout.Toggle(visible, label, EditorStyles.foldoutHeader);
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(!new_keyword_state);
            if (visible)
            {
                drawContent();
            }
            EditorGUI.EndDisabledGroup();
        }

        #endregion


        //Main texture 
        string[] spaceOptions = { "uv", "world", "local", "view" };
        v4_DropDown("_frag_uv_world_local_view", "Space", spaceOptions);
        int space = Get_v4_index("_frag_uv_world_local_view");
        if (space == 1 || space == 2)  DrawProperty_v3("_frag_plane_XYZ", "Plane");
        DrawProperty("_main_tint_color", "Color");
        DrawProperty("_Alpha_cliping_treshold", "Alpha Clipping Threshold");
        DrawProperty("_main_texture", "Texture");


        FoldOut_keyword("_TEXTURE_DISTRORTION", ref vis_MainTextureDistortion, "Texture Distortion", textureDistortion_content);
        void textureDistortion_content()
        {
            DrawProperty_v3("_Fragment_scroll", "scroll speed");
            DrawProperty("_frag_dist_noise_map", "Distortion texture");
            DrawProperty("_frag_dist_noise_amount", "Distortion amount");
            DrawProperty_v3("_frag_dist_noise_scroll", "noise scroll speed");
            DrawProperty("_frag_dist_detail_map", "Detail map");
            DrawProperty("_frag_dist_detail_amount", "detail amount");
        }


        FoldOut_keyword("_DISSOLVE", ref vis_Disolve, "Dissolve effect", dissolve_effect_content);
        void dissolve_effect_content(){
            EditorGUI.indentLevel = 3;
            //Underlay texture
            Fold_simple(ref vis_UnderlayTexture, "Underlay Texture", UnderlayTexture_content);
            void UnderlayTexture_content()
            {
                string[] spaceOptions_2tex = { "uv", "world", "local", "view" };
                v4_DropDown("_2tex_space_uv_world_local_view", "Space", spaceOptions_2tex);
                int space_2tex = Get_v4_index("_2tex_space_uv_world_local_view");
                if (space_2tex == 1 || space_2tex == 2) DrawProperty_v3("_2tex_dis_plane_XYZ", "Plane");
                DrawProperty("_2tex_texture", "texture");
                DrawProperty("_2tex_color_opacity", "Color and Opacity");
                DrawProperty_v3("_underlay_scroll_speed", "Scroll speed");
            }


            // Dissolve
            Fold_simple(ref vis_dissolveTexture, "Disolve texture", Disolve_content);
            void Disolve_content()
            {
                string[] spaceOptions_disolve = { "uv", "world", "local", "view" };
                v4_DropDown("_f_dis_space_UV_World_Local_View", "Space", spaceOptions_disolve);

                DrawProperty("_dissolve_Texture", "texture");
                DrawProperty("_dissolve_scroll_speed", "scroll");
                DrawProperty("_detail_texture", "detail texture");
                DrawProperty("_detail_scroll_speed", "scroll");
                DrawProperty("_detail_influence", "detail amount");
            }
            Fold_simple(ref vis_dissolveControls, "Disolve Controls", Disolve_controls_content);
            void Disolve_controls_content()
            {
                //Dissolve settings
                DrawProperty("_dissolve_master_opacity", "opacity");
                DrawProperty("_dissolve_effect", "Amountt");
                DrawProperty("_disolve_smoothness", "smoothness");
                DrawProperty("_Dissolve_border_size", "border size");
                DrawProperty("_Dissolve_border_Color", "border color");


                DrawProperty("_Directional_Disolve", "directional dissolve");
                float directionalDissolve = FindProperty("_Directional_Disolve",properties).floatValue;
                if (directionalDissolve > 0.5) { 
                    DrawProperty_v3("_Direction", "DIrection");
                    DrawProperty("_Position", "position"); 
                }
            }
            
        }

    }


}


