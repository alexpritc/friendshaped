//-----------------------------------------------------------------------------
// ParallaxiumEditor.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Custom editor for Parallaxium script
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace ParallaxiumBeta
{

    [CustomEditor(typeof(Parallaxium))]
    public class ParallaxiumEditor : Editor
    {
        private List<string> names;

        private string newLayerName = "New Layer";

        private Parallaxium m_Target;

        // Lists to show layers and sections correctly
        private List<bool> showLayer;
        private List<List<bool>> showSection;
        private List<List<int>> sectionLayerIndexes;
        private Dictionary<string, int> layerIndices;

        // Custom styles
        GUIStyle gsLayerFirst;
        GUIStyle gsLayerSecond;
        GUIStyle layerSectionStyle;
        GUIStyle sectionFoldoutStyle;
        GUIStyle layerHeaderSkin;

        /// <summary>
        /// Initializes variables
        /// </summary>
        private void OnEnable()
        {
            m_Target = (Parallaxium)target; // get target script

            if (m_Target.layers.Count == 0) // if no layers
            {
                m_Target.layers.Add(new Layer("Default", 0.0f)); // add default layer
            }

            layerIndices = new Dictionary<string, int>();
            names = new List<string>();
            SetNames();

            showLayer = new List<bool>();
            showSection = new List<List<bool>>();
            sectionLayerIndexes = new List<List<int>>();
            OrganizeFoldouts();

            gsLayerFirst = new GUIStyle();
            gsLayerFirst.normal.background = MakeTex(600, 1, new Color(1.0f, 1.0f, 1.0f, 0.1f));

            gsLayerSecond = new GUIStyle();
            gsLayerSecond.normal.background = MakeTex(600, 1, new Color(1.0f, 1.0f, 1.0f, 0.05f));
        }

        /// <summary>
        /// Called everytime the user interacts with the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            SetNames();
            OrganizeFoldouts();
            InitStyles();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("cam"), new GUIContent("Camera", "The active camera in the scene."));

            DrawDirectionOptions();

            DrawBlurOptions();

            DrawTintOptions();

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            ListLayers();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Initializes the styles to be used when drawing the editor
        /// </summary>
        private void InitStyles()
        {
            layerSectionStyle = new GUIStyle(EditorStyles.foldout);
            Color myStyleColor = Color.black;
            layerSectionStyle.fontStyle = FontStyle.Bold;
            layerSectionStyle.fixedWidth = 100f;

            layerHeaderSkin = new GUIStyle(EditorStyles.boldLabel);
            layerHeaderSkin.alignment = TextAnchor.MiddleRight;
            layerHeaderSkin.padding.right = 35;

            sectionFoldoutStyle = new GUIStyle(EditorStyles.foldout);
            myStyleColor = Color.gray;
            sectionFoldoutStyle.fontStyle = FontStyle.Bold;
            sectionFoldoutStyle.normal.textColor = myStyleColor;
            sectionFoldoutStyle.onNormal.textColor = myStyleColor;
            sectionFoldoutStyle.hover.textColor = myStyleColor;
            sectionFoldoutStyle.onHover.textColor = myStyleColor;
            sectionFoldoutStyle.focused.textColor = myStyleColor;
            sectionFoldoutStyle.onFocused.textColor = myStyleColor;
            sectionFoldoutStyle.active.textColor = myStyleColor;
            sectionFoldoutStyle.onActive.textColor = myStyleColor;
        }

        /// <summary>
        /// Draws parallax direction options
        /// </summary>
        private void DrawDirectionOptions()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Parallax Direction:", GUILayout.MaxWidth(150f));

                EditorGUI.BeginChangeCheck();

                bool newXDirection = GUILayout.Toggle(m_Target.xParallaxDirection, "X", GUILayout.MaxWidth(60f));
                bool newYDirection = GUILayout.Toggle(m_Target.yParallaxDirection, "Y", GUILayout.MaxWidth(60f));

                if (!newXDirection && !newYDirection)
                {
                    newXDirection = m_Target.xParallaxDirection;
                    newYDirection = m_Target.yParallaxDirection;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Target, "Modify Parallax Direction");

                    m_Target.xParallaxDirection = newXDirection;
                    m_Target.yParallaxDirection = newYDirection;

                    EditorUtility.SetDirty(m_Target);
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draw depth of field options
        /// </summary>
        private void DrawBlurOptions()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DepthOfField"));

            EditorGUI.BeginDisabledGroup(!m_Target.DepthOfField);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Blur Strength");

            EditorGUI.BeginChangeCheck();

            float newBlurScale = EditorGUILayout.Slider(m_Target.BlurScale, 0, 2);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Target, "Modify Blur Strength");

                m_Target.BlurScale = newBlurScale;

                EditorUtility.SetDirty(m_Target);
            }

            GUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Draw distance fog options
        /// </summary>
        private void DrawTintOptions()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("DistanceFog"));

            EditorGUI.BeginDisabledGroup(!m_Target.DistanceFog);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("BackgroundTint"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ForegroundTint"));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Fog Strength");

            EditorGUI.BeginChangeCheck();

            float newTintScale = EditorGUILayout.Slider(m_Target.TintScale, 0, 1);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(m_Target, "Modify Fog Strength");

                m_Target.TintScale = newTintScale;

                EditorUtility.SetDirty(m_Target);
            }

            GUILayout.EndHorizontal();

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// Loops through layers and draws them including sections if foldout is expanded
        /// </summary>
        private void ListLayers()
        {
            DrawToolbar();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Layers", EditorStyles.boldLabel);
            GUILayout.Label("Depth", layerHeaderSkin, GUILayout.Width(100));
            GUILayout.EndHorizontal();

            for (int i = 0; i < m_Target.layers.Count; i++)
            {
                if (i % 2 == 0)
                {
                    GUILayout.BeginVertical(gsLayerFirst);
                }
                else
                {
                    GUILayout.BeginVertical(gsLayerSecond);
                }

                GUILayout.BeginHorizontal();
                {
                    showLayer[i] = EditorGUILayout.BeginFoldoutHeaderGroup(showLayer[i], m_Target.layers[i].Name, layerSectionStyle);

                    EditorGUI.BeginChangeCheck();

                    float newScrollSpeed = EditorGUILayout.Slider(m_Target.layers[i].Depth, -1f, 1f, GUILayout.MinWidth(200f));

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Modify Layer Depth");

                        m_Target.layers[i].Depth = newScrollSpeed;

                        EditorUtility.SetDirty(m_Target);
                    }

                    EditorGUI.BeginDisabledGroup(m_Target.layers.Count == 1); // Disable button if there is only 1 layer

                    if (GUILayout.Button(" x ", EditorStyles.miniButton, GUILayout.Width(17)))
                    {
                        Undo.RecordObject(m_Target, "Delete Layer");
                        m_Target.layers.RemoveAt(i);
                        SetNames();
                        OrganizeFoldouts();
                        EditorUtility.SetDirty(m_Target);
                        GUIUtility.ExitGUI();
                    }

                    EditorGUI.EndDisabledGroup();
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(2);

                m_Target.layers[i].isExpanded = showLayer[i];

                if (showLayer[i])
                {
                    EditorGUI.indentLevel++;

                    for (int j = 0; j < m_Target.layers[i].sections.Count; j++)
                    {
                        if (j % 2 != 0)
                        {
                            GUILayout.BeginVertical(gsLayerFirst);
                        }
                        else
                        {
                            GUILayout.BeginVertical(gsLayerSecond);
                        }

                        GUILayout.BeginHorizontal();

                        showSection[i][j] = EditorGUILayout.Foldout(showSection[i][j], m_Target.layers[i].sections[j].Name, true, sectionFoldoutStyle);

                        m_Target.layers[i].sections[j].isExpanded = showSection[i][j];

                        if (GUILayout.Button("Remove", EditorStyles.miniButton, GUILayout.Width(50)))
                        {
                            Undo.RecordObject(m_Target, "Delete Section");
                            m_Target.layers[i].RemoveSection(j);
                            OrganizeFoldouts();
                            EditorUtility.SetDirty(m_Target);
                            GUIUtility.ExitGUI();
                        }
                        GUILayout.EndHorizontal();

                        if (showSection[i][j])
                        {
                            DrawSection(i, j);
                        }

                        GUILayout.EndVertical();

                    }
                    EditorGUI.indentLevel--;

                    GUILayout.Space(5);

                    DrawAddSection(i);
                }

                GUILayout.EndVertical();

                GUILayout.Space(5);

                EditorGUILayout.EndFoldoutHeaderGroup();

            }
        }

        /// <summary>
        /// Adds layer with given name
        /// </summary>
        /// <param name="layerName">Name of new layer</param>
        private void SubmitNewLayer(string layerName)
        {
            if (!ValidLayerName(layerName)) // if not valid name
            {
                int i = 1; // used to count amount of duplicates

                while (!ValidLayerName(layerName)) // while still not valid name
                {
                    if (i != 1) // if not first time
                    {
                        if (i != 2)
                        {
                            i++;
                        }
                        layerName = layerName.Remove(layerName.Length - 3, 3); // remove ending
                        layerName += "(" + i + ")"; // replace with new value
                        if (i == 2)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        layerName += " (" + i + ")"; // add ending
                        i++;
                    }
                }
            }

            Undo.RecordObject(m_Target, "Add New Layer");

            m_Target.layers.Add(new Layer(layerName, 0.0f));
            OrganizeFoldouts();
            SetNames();
            EditorUtility.SetDirty(m_Target);
            GUIUtility.ExitGUI();
        }

        /// <summary>
        /// Draws toolbar containing new layer name input, add layer button, autofill button and order by depth button
        /// </summary>
        private void DrawToolbar()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            newLayerName = EditorGUILayout.TextField(newLayerName, EditorStyles.toolbarTextField);

            if (GUILayout.Button("Add Layer", EditorStyles.toolbarButton))
            {
                SubmitNewLayer(newLayerName);
            }

            if (GUILayout.Button("Autofill", EditorStyles.toolbarButton))
            {
                Undo.RecordObject(m_Target, "Autofill Parallaxium");
                AutoFill();
                EditorUtility.SetDirty(m_Target);
            }

            if (GUILayout.Button("Order by Depth", EditorStyles.toolbarButton))
            {
                Undo.RecordObject(m_Target, "Rearrange Layers");
                RearrangeLayers();
                EditorUtility.SetDirty(m_Target);
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Draws a section with all information
        /// </summary>
        /// <param name="i">Index of layer containing section</param>
        /// <param name="j">Index of section to be drawn</param>
        private void DrawSection(int i, int j)
        {
            float width = EditorGUIUtility.currentViewWidth;

            if (j < 0 || j >= m_Target.layers[i].sections.Count)
            {
                return;
            }

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Name", EditorStyles.label, GUILayout.Width(50));

                EditorGUI.BeginChangeCheck();

                string newName = GUILayout.TextField(m_Target.layers[i].sections[j].Name, GUILayout.Width(width - 75));

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Target, "Modify Section Name");

                    m_Target.layers[i].sections[j].Name = newName;

                    EditorUtility.SetDirty(m_Target);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Layer", EditorStyles.label, GUILayout.Width(50));

                EditorGUI.BeginChangeCheck();
                sectionLayerIndexes[i][j] = EditorGUILayout.Popup(sectionLayerIndexes[i][j], names.ToArray());

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Target, "Modify Layer Name");

                    m_Target.layers[sectionLayerIndexes[i][j]].AddSection(m_Target.layers[i].sections[j]);
                    m_Target.layers[i].RemoveSection(j);
                    OrganizeFoldouts();
                    EditorUtility.SetDirty(m_Target);
                    GUIUtility.ExitGUI();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Seamless Sprites:");

                EditorGUI.BeginChangeCheck();
                bool newHorizontal = GUILayout.Toggle(m_Target.layers[i].sections[j].horizontalSeamless, "Horizontal");
                bool newVertical = GUILayout.Toggle(m_Target.layers[i].sections[j].verticalSeamless, "Vertical");

                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(m_Target, "Modify Seamless Sprites");

                    m_Target.layers[i].sections[j].horizontalSeamless = newHorizontal;
                    m_Target.layers[i].sections[j].verticalSeamless = newVertical;

                    EditorUtility.SetDirty(m_Target);
                }
            }
            GUILayout.EndHorizontal();

            if (m_Target.DepthOfField)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Disable Blur:", GUILayout.Width(135));

                    EditorGUI.BeginChangeCheck();

                    bool newBlur = GUILayout.Toggle(m_Target.layers[i].sections[j].DisableBlur, "");

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Modify Section Disable Blur");

                        m_Target.layers[i].sections[j].DisableBlur = newBlur;

                        EditorUtility.SetDirty(m_Target);
                    }
                }
                GUILayout.EndHorizontal();
            }

            if (m_Target.DistanceFog)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Disable Tint:", GUILayout.Width(135));

                    EditorGUI.BeginChangeCheck();

                    bool newTint = GUILayout.Toggle(m_Target.layers[i].sections[j].DisableTint, "");

                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(m_Target, "Modify Section Disable Tint");

                        m_Target.layers[i].sections[j].DisableTint = newTint;

                        EditorUtility.SetDirty(m_Target);
                    }
                }
                GUILayout.EndHorizontal();
            }

            SerializedProperty listIterator = serializedObject.FindProperty("layers");
            SerializedProperty sections = listIterator.GetArrayElementAtIndex(i).FindPropertyRelative("sections");

            GUILayout.Space(5);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(sections.GetArrayElementAtIndex(j).FindPropertyRelative("objects"), true);
            EditorGUI.indentLevel--;
        }

        /// <summary>
        /// Draws add new section button
        /// </summary>
        /// <param name="i"></param>
        private void DrawAddSection(int i)
        {
            if (GUILayout.Button("Add Section", GUILayout.Height(20)))
            {
                Undo.RecordObject(m_Target, "Add New Section");

                Section newSection = new Section { Name = "New Section" };

                m_Target.layers[i].AddSection(newSection);

                OrganizeFoldouts();

                EditorUtility.SetDirty(m_Target);

                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// Clears and reorganizes lists
        /// </summary>
        private void OrganizeFoldouts()
        {
            showSection.Clear();
            showLayer.Clear();
            sectionLayerIndexes.Clear();

            for (int i = 0; i < m_Target.layers.Count; i++)
            {
                showLayer.Add(m_Target.layers[i].isExpanded);
                showSection.Add(new List<bool>());
                sectionLayerIndexes.Add(new List<int>());

                for (int j = 0; j < m_Target.layers[i].sections.Count; j++)
                {
                    showSection[i].Add(m_Target.layers[i].sections[j].isExpanded);
                    sectionLayerIndexes[i].Add(i);
                }
            }
        }

        /// <summary>
        /// Clears and organizes name and index lists
        /// </summary>
        private void SetNames()
        {
            names.Clear();
            layerIndices.Clear();

            for (int i = 0; i < m_Target.layers.Count; i++)
            {
                names.Add(m_Target.layers[i].Name);
                layerIndices.Add(m_Target.layers[i].Name, i);
            }
        }

        /// <summary>
        /// Check if name is already used for a layer and returns the result
        /// </summary>
        /// <param name="name">The layer name to be checked</param>
        /// <returns>True if name is not used, False if name is used and therefor invalid</returns>
        private bool ValidLayerName(string name)
        {
            for (int i = 0; i < m_Target.layers.Count; i++)
            {
                if (m_Target.layers[i].Name == name)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Rearranges the order of layers based on depth using bubble sort algorithm
        /// </summary>
        private void RearrangeLayers()
        {
            bool swapped;
            for (int i = 0; i < m_Target.layers.Count - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < m_Target.layers.Count - i - 1; j++)
                {
                    if (m_Target.layers[j].Depth < m_Target.layers[j + 1].Depth)
                    {
                        Layer temp = m_Target.layers[j];
                        m_Target.layers[j] = m_Target.layers[j + 1];
                        m_Target.layers[j + 1] = temp;
                        swapped = true;
                    }
                }

                if (swapped == false)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Creates a texture to be used in the editor
        /// </summary>
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        /// <summary>
        /// Loops through all gameobjects in scene and organizes layers based on sprite sorting layer
        /// </summary>
        private void AutoFill()
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            Dictionary<string, List<GameObject>> sorts = new Dictionary<string, List<GameObject>>();
            SpriteRenderer[] tempRenderer;

            // iterate through all objects in scene
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                if (rootObjects[i].GetComponentsInChildren<SpriteRenderer>() != null) // if gameobject has sprite renderer component
                {
                    tempRenderer = rootObjects[i].GetComponentsInChildren<SpriteRenderer>(); // store component

                    for (int j = 0; j < tempRenderer.Length; j++)
                    {
                        string layerName = tempRenderer[j].sortingLayerName + " " + tempRenderer[j].sortingOrder;
                        if (sorts.ContainsKey(layerName)) // if sorting layer and order combination exists
                        {
                            sorts[layerName].Add(tempRenderer[j].gameObject); // add gameobject
                        }
                        else
                        {
                            sorts.Add(layerName, new List<GameObject>()); // create sorting layer and order combination in dictionary
                            sorts[layerName].Add(tempRenderer[j].gameObject); // add gameobject
                        }
                    }
                }
            }

            m_Target.layers.Clear();

            int num = 0;
            foreach (KeyValuePair<string, List<GameObject>> layer in sorts)
            {
                m_Target.layers.Add(new Layer(layer.Key, 0.0f));
                m_Target.layers[num].isExpanded = false;
                m_Target.layers[num].sections[0].objects = layer.Value;
                num++;
            }

            SetNames();
            OrganizeFoldouts();
            GUIUtility.ExitGUI();
        }
    }
}