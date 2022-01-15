//-----------------------------------------------------------------------------
// Layer.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Layer section manager
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    /// <summary>
    /// Contains Depth information and all Sections to be moved at the set depth
    /// </summary>
    [System.Serializable]
    public class Layer
    {
        public string Name;
        public float Depth;

        public List<Section> sections = new List<Section>();

        /// <summary>
        /// Constructor for Layer class
        /// </summary>
        /// <param name="name">Name for the layer to be created</param>
        /// <param name="depth">Depth of the layer to be created</param>
        public Layer(string name, float depth)
        {
            Name = name;
            Depth = depth;
            Section initialSection = new Section() { Name = "New Section" };
            sections.Add(initialSection);
        }

        /// <summary>
        /// Loops through all sections and initializes them.
        /// </summary>
        public void Init(float viewportWidth, float viewportHeight, Parallaxium main)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                sections[i].Init(viewportWidth, viewportHeight, main);
            }
        }

        /// <summary>
        /// Adds a given Section to the list.
        /// </summary>
        public void AddSection(Section section)
        {
            sections.Add(section);
        }

        /// <summary>
        /// Removes a Section at a given index.
        /// </summary>
        public void RemoveSection(int index)
        {
            if (index >= 0 && index < sections.Count)
            {
                sections.RemoveAt(index);
            }
        }

        /// <summary>
        /// Loops through all sections and calls BlurSprites method
        /// </summary>
        public void BlurOrTintSections(float blurScale, Material blurMaterial, Color backgroundTint, Color foregroundTint, float tintScale)
        {
            float blurAmount = (Depth * 15) * blurScale;

            if (Depth > 0) // if background layer
            {
                for (int i = 0; i < sections.Count; i++)
                {
                    sections[i].BlurOrTintSprites(Depth * tintScale, blurAmount, blurMaterial, backgroundTint); // use background tint
                }
            }
            else if (Depth < 0) // if foreground layer
            {
                for (int i = 0; i < sections.Count; i++)
                {
                    sections[i].BlurOrTintSprites(Mathf.Abs(Depth) * tintScale, blurAmount, blurMaterial, foregroundTint); // use foreground tint
                }
            }
        }

#if UNITY_EDITOR
        public bool isExpanded = true; // custom editor setting not required for builds
#endif
    }
}