//-----------------------------------------------------------------------------
// Section.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Section sprite manager
//-----------------------------------------------------------------------------


using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    /// <summary>
    /// Contains all gameobjects and seamless sprites functionality.
    /// </summary>
    [System.Serializable]
    public class Section
    {
        public string Name;

        public bool horizontalSeamless;
        public bool verticalSeamless;

        public bool DisableBlur = false;
        public bool DisableTint = false;

        public List<GameObject> objects = new List<GameObject>(new GameObject[1]);

        public Vector2 StartPos { get; set; }

        /// <summary>
        /// Length of the section. (right of right-most sprite - left of left-most sprite)
        /// </summary>
        public float Length { get; private set; }

        /// <summary>
        /// Height of the section. (top of top-most sprite - bottom of bottom-most sprite)
        /// </summary>
        public float Height { get; private set; }

        private Parallaxium parallaxium; // reference to parent script set in initialization method

        /// <summary>
        /// Initialization method, calls relevant methods
        /// </summary>
        /// <param name="viewportWidth">The width of the screen</param>
        /// <param name="viewportHeight">The height of the screen</param>
        /// <param name="main">Reference to parent script</param>
        public void Init(float viewportWidth, float viewportHeight, Parallaxium main)
        {
            parallaxium = main;

            if (CheckObjects())
            {
                CalculateDimensions();

                if (horizontalSeamless || verticalSeamless)
                {
                    SeamlessInitalization(viewportWidth, viewportHeight);
                }
            }
        }

        /// <summary>
        /// Loops through gameobjects and searches for null references. Disables parallaxium if null reference is found.
        /// </summary>
        /// <returns>Boolean representing whether there are no null references</returns>
        private bool CheckObjects()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == null)
                {
                    Debug.LogError("Section '" + Name + "' contains a null GameObject. Parallaxium has been disabled.");
                    parallaxium.SetActive(false);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Loops through gameobjects and calculates Width and Height of this Section.
        /// </summary>
        private void CalculateDimensions()
        {
            float left = 0;
            float right = 0;
            float down = 0;
            float up = 0;

            Vector3 min = Vector3.zero;
            Vector3 max = Vector3.zero;


            for (int i = 0; i < objects.Count; i++)
            {
                SpriteRenderer[] sprites = objects[i].GetComponentsInChildren<SpriteRenderer>(true);

                for (int j = 0; j < sprites.Length; j++)
                {
                    min = sprites[j].bounds.min;
                    max = sprites[j].bounds.max;

                    // Set to default
                    if (i == 0 && j == 0)
                    {
                        left = min.x;
                        down = min.y;
                        right = max.x;
                        up = max.y;
                    }
                    else
                    {
                        if (min.x < left)
                        {
                            left = min.x;
                        }
                        if (min.y < down)
                        {
                            down = min.y;
                        }
                        if (max.x > right)
                        {
                            right = max.x;
                        }
                        if (max.y > up)
                        {
                            up = max.y;
                        }
                    }
                }
            }

            Length = right - left;
            Height = up - down;

            StartPos = new Vector2(left + (Length / 2), down + (Height / 2));
        }

        /// <summary>
        /// Creates parents and clone template and passes them to relevant initialization methods.
        /// </summary>
        /// <param name="viewportWidth"></param>
        /// <param name="viewportHeight"></param>
        private void SeamlessInitalization(float viewportWidth, float viewportHeight)
        {
            // Create gameobject to store all clones of this section
            GameObject duplicate = new GameObject(Name + " Duplicate");

            // Create gameobject to store copy of all objects in this section
            // Clone is then used as a template to instantiate new clones
            GameObject clone = new GameObject("");
            Replicate(clone.transform);

            if (horizontalSeamless)
            {
                HorizontalSeamlessInitialization(viewportWidth, duplicate, clone);
            }

            if (verticalSeamless)
            {
                VerticalSeamlessInitialization(viewportHeight, duplicate, clone);
            }

            Object.Destroy(clone);

            objects.Add(duplicate);
            duplicate.transform.SetParent(parallaxium.gameObject.transform);
        }

        /// <summary>
        /// Duplicates gameobjects along X axis.
        /// </summary>
        private void HorizontalSeamlessInitialization(float viewportWidth, GameObject duplicate, GameObject clone)
        {
            if (viewportWidth > Length) // if more than one replication is required in both directions
            {
                float xReplications = Mathf.CeilToInt((((viewportWidth - Length) / Length) / 2));

                for (int i = 1; i < xReplications + 2; i++)
                {
                    float xOffset = Length * i;

                    for (int j = 1; j >= -1; j -= 2)
                    {
                        CreateClone(clone, duplicate.transform, -(xOffset * j), 0);
                    }
                }
            }
            else
            {
                CreateClone(clone, duplicate.transform, Length, 0);
                CreateClone(clone, duplicate.transform, -Length, 0);
            }
        }

        /// <summary>
        /// Duplicates gameobjects along Y axis.
        /// </summary>
        private void VerticalSeamlessInitialization(float viewportHeight, GameObject duplicate, GameObject clone)
        {
            float yReplications = Mathf.CeilToInt((((viewportHeight - Height) / Height) / 2));

            if (yReplications == 0) // If replications not required ensure at least one replication
            {
                yReplications++;
            }

            int numChildren = duplicate.transform.childCount;

            // Loop through horizontal clones and create vertical clones at each position
            for (int i = 0; i < numChildren; i++)
            {
                Transform child = duplicate.transform.GetChild(i);

                for (int j = 1; j < yReplications + 1; j++)
                {
                    float yOffset = Height * j;

                    for (int k = 1; k >= -1; k -= 2)
                    {
                        CreateClone(clone, duplicate.transform, child.position.x, -(yOffset * k));
                    }
                }
            }

            // Create vertical replications for original section
            for (int j = 1; j < yReplications + 1; j++)
            {
                float yOffset = Height * j;

                for (int k = 1; k >= -1; k -= 2)
                {
                    CreateClone(clone, duplicate.transform, 0, -(yOffset * k));
                }
            }
        }

        /// <summary>
        /// Duplicates gameobject, moves it by given offsets and sets parent to given Transform
        /// </summary>
        /// <param name="clone">Object to be cloned</param>
        /// <param name="parent">Transform of parent GameObject</param>
        private void CreateClone(GameObject clone, Transform parent, float xOffset, float yOffset)
        {
            GameObject temp = Object.Instantiate(clone);
            temp.transform.Translate(temp.transform.position.x + xOffset, temp.transform.position.y + yOffset, temp.transform.position.z);
            temp.transform.SetParent(parent);
        }

        /// <summary>
        /// Duplicates all gameobjects and sets the parent to a given Transform
        /// </summary>
        /// <param name="parent">Transform to act as parent for all new GameObjects</param>
        private void Replicate(Transform parent)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                GameObject temp = Object.Instantiate(objects[i], new Vector3(objects[i].transform.position.x,
                    objects[i].transform.position.y),
                    objects[i].transform.rotation);

                temp.transform.SetParent(parent.transform, true);
            }
        }

        /// <summary>
        /// Checks distance moved and shifts objects if threshold is passed.
        /// </summary>
        /// <param name="xParallax"></param>
        /// <param name="yParallax"></param>
        public void Reorder(float xParallax, float yParallax)
        {
            if (horizontalSeamless)
            {
                if (xParallax > (StartPos.x + Length / 2.0f))
                {
                    ShiftObjects(true, 1);
                    StartPos = new Vector2(StartPos.x + Length, StartPos.y);
                }
                else if (xParallax < StartPos.x - (Length / 2.0f))
                {
                    ShiftObjects(true, -1);
                    StartPos = new Vector2(StartPos.x - Length, StartPos.y);
                }
            }

            if (verticalSeamless)
            {
                if (yParallax > (StartPos.y + Height / 2.0f))
                {
                    ShiftObjects(false, 1);
                    StartPos = new Vector2(StartPos.x, StartPos.y + Height);
                }
                else if (yParallax < StartPos.y - Height / 2.0f)
                {
                    ShiftObjects(false, -1);
                    StartPos = new Vector2(StartPos.x, StartPos.y - Height);
                }
            }
        }

        /// <summary>
        /// Moves objects by Length of Height of section.
        /// </summary>
        /// <param name="xAxis">True if objects should be shifted along X axis, False if Y axis</param>
        /// <param name="direction">1 if to be moved in positive direction or -1 for negative direction</param>
        private void ShiftObjects(bool xAxis, int direction)
        {
            if (xAxis)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.Translate(Length * direction, 0, 0);
                }
            }
            else
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    objects[i].transform.Translate(0, Height * direction, 0);
                }
            }
        }

        /// <summary>
        /// Loops through gameobjects and sets material properties based on settings.
        /// </summary>
        /// <param name="depth">Depth of Layer this section is assigned to</param>
        /// <param name="blur">Amount to blur gameobject</param>
        /// <param name="blurMaterial">Material to be assigned to sprite</param>
        /// <param name="tint">Colour to tint sprite</param>
        public void BlurOrTintSprites(float depth, float blur, Material blurMaterial, Color tint)
        {
            if (!DisableBlur)
            {
                if (!DisableTint)
                {
                    for (int i = 0; i < objects.Count; i++) // loop through all gameobjects
                    {
                        Renderer[] renderers = objects[i].GetComponentsInChildren<Renderer>(true); // get all renderers including those of disabled gameobjects

                        for (int j = 0; j < renderers.Length; j++)
                        {
                            renderers[j].sharedMaterial = blurMaterial; // assign material to gameobject
                            SpritePropertySetter.AdjustSprite(renderers[j], depth, true, blur, true, tint); // set blur and tint
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        Renderer[] renderers = objects[i].GetComponentsInChildren<Renderer>(true);

                        for (int j = 0; j < renderers.Length; j++)
                        {
                            renderers[j].sharedMaterial = blurMaterial;
                            SpritePropertySetter.AdjustSprite(renderers[j], 0, true, blur, true, tint); // set blur but not tint
                        }
                    }
                }
            }
            else
            {
                if (!DisableTint)
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        Renderer[] renderers = objects[i].GetComponentsInChildren<Renderer>(true);

                        for (int j = 0; j < renderers.Length; j++)
                        {
                            renderers[j].sharedMaterial = blurMaterial;
                            SpritePropertySetter.AdjustSprite(renderers[j], depth, true, 0, true, tint); // set tint but not blur
                        }
                    }
                }
            }
        }

#if UNITY_EDITOR
        public bool isExpanded = true; // custom editor setting not required for builds
#endif
    }
}
