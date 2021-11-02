//-----------------------------------------------------------------------------
// Parallaxium.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Parallaxium main controller
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    [System.Serializable]
    public class Parallaxium : MonoBehaviour
    {
        public Camera cam;

        public bool active = true;

        // Parallax settings
        public bool xParallaxDirection = true;
        public bool yParallaxDirection = true;

        // Depth of field settings
        public bool DepthOfField = false;
        public float BlurScale = 1f;
        // Distance fog settings
        public bool DistanceFog = false;
        public Color BackgroundTint = Color.gray;
        public Color ForegroundTint = Color.black;
        public float TintScale = 0.5f;

        public List<Layer> layers = new List<Layer>();

        private Vector2 distanceMoved;
        private Vector3 lastPosition;

        // strategies
        private IParralaxStrategy[] strategies;
        private IParralaxStrategy parallaxStrategy;

        private void Awake()
        {
            distanceMoved = new Vector2();

            // Create strategies in array
            strategies = new IParralaxStrategy[] { gameObject.AddComponent<XStrategy>(), gameObject.AddComponent<YStrategy>(), gameObject.AddComponent<XYStrategy>() };

            // Set initial parallax direction
            ToggleDirection(xParallaxDirection, yParallaxDirection);
        }

        private void Start()
        {
            // Get extremities of viewport and calculate dimensions
            Vector3 bottomLeft = cam.ViewportToWorldPoint(Vector3.zero);
            Vector3 topRight = cam.ViewportToWorldPoint(Vector3.one);

            float viewportWidth = topRight.x - bottomLeft.x;
            float viewportHeight = topRight.y - bottomLeft.y;

            // Initialize layers
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].Init(viewportWidth, viewportHeight, this);
            }

            Material material = Resources.Load<Material>("Materials/Blur");

            if (DepthOfField)
            {
                if (DistanceFog)
                {
                    AdjustSprites(material, BlurScale, TintScale, BackgroundTint, ForegroundTint); // enable distance fog and depth of field
                }
                else
                {
                    AdjustSprites(material, BlurScale, 0, BackgroundTint, ForegroundTint); // enable depth of field
                }
            }
            else if (DistanceFog)
            {
                AdjustSprites(material, 0, TintScale, BackgroundTint, ForegroundTint); // enable distance fog
            }

            // Set first position
            lastPosition = cam.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (active)
            {
                if (!lastPosition.Equals(cam.transform.position)) // if the camera has moved since last frame
                {
                    MoveLayers(cam.transform.position);
                }
            }
        }

        /// <summary>
        /// Loops through layers and calls the BlurOrTintSections method while passing references to it
        /// </summary>
        /// <param name="material">The shared material loaded from the Resources directory</param>
        private void AdjustSprites(Material material, float blurAmount, float tintAmount, Color backgroundTint, Color foregroundTint)
        {
            if (material != null)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].BlurOrTintSections(blurAmount, material, backgroundTint, foregroundTint, tintAmount);
                }
            }
            else
            {
                Debug.LogError("Blur material could not be found at Resources/Materials/Blur");
            }
        }

        /// <summary>
        /// Moves all the layers according to the distance moved by the camera and layer depth.
        /// </summary>
        /// <param name="camPosition">Cached position of camera.</param>
        private void MoveLayers(Vector3 camPosition)
        {
            // calculate distance moved on each axis
            distanceMoved.x = camPosition.x - lastPosition.x;
            distanceMoved.y = camPosition.y - lastPosition.y;

            for (int i = 0; i < layers.Count; i++)
            {
                // calculate amount of parallax to add
                float xParallax = camPosition.x * (1 - layers[i].Depth);
                float yParallax = camPosition.y * (1 - layers[i].Depth);

                // loop through all sections of this layer and move the sprites
                for (int j = 0; j < layers[i].sections.Count; j++)
                {
                    parallaxStrategy.MoveLayer(layers[i].sections[j].objects, distanceMoved.x * layers[i].Depth, distanceMoved.y * layers[i].Depth);

                    if (layers[i].sections[j].horizontalSeamless || layers[i].sections[j].verticalSeamless)
                    {
                        layers[i].sections[j].Reorder(xParallax, yParallax);
                    }
                }
            }

            lastPosition = camPosition;
        }

        /// <summary>
        /// Sets which axis parallax should affect.
        /// </summary>
        /// <param name="xAxis"></param>
        /// <param name="yDirection"></param>
        public void ToggleDirection(bool xAxis, bool yDirection)
        {
            if (xAxis)
            {
                if (yDirection)
                {
                    parallaxStrategy = strategies[2]; // XYStrategy
                }
                else
                {
                    parallaxStrategy = strategies[0]; // XStrategy
                }
            }
            else if (yDirection)
            {
                parallaxStrategy = strategies[1]; // YStrategy
            }
            else
            {
                Debug.LogError("Parallax must have set direction - setting to X Direction default. Use SetActive(false) to disable parallax.");
                ToggleDirection(true, false);
            }
        }

        /// <summary>
        /// Finds and returns a layer from a given name
        /// </summary>
        /// <param name="name">The name of the layer to be found</param>
        /// <returns>Found Layer or null if no Layer found</returns>
        public Layer GetLayer(string name)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                if (name == layers[i].Name)
                {
                    return layers[i];
                }
            }

            Debug.LogError("Parallaxium: Could not link section to layer. " + name + " - not recognized layer");

            return null;
        }

        /// <summary>
        /// Sets active setting to given bool value 
        /// </summary>
        /// <param name="setting"></param>
        public void SetActive(bool setting)
        {
            active = setting;
        }
    }
}

