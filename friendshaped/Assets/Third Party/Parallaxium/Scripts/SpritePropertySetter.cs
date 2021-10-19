//-----------------------------------------------------------------------------
// SpritePropertySetter.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Material property block setter for sprite renderers
//-----------------------------------------------------------------------------

using UnityEngine;

namespace ParallaxiumBeta
{
    public class SpritePropertySetter : MonoBehaviour
    {
        /// <summary>
        /// Sets renderer property block with given properties
        /// </summary>
        /// <param name="rend">Renderer to be changed</param>
        /// <param name="depth">Depth of layer the renderer is attached to</param>
        /// <param name="toBlur">Boolean whether to blur or not</param>
        /// <param name="blurAmount">Amount to blur object</param>
        /// <param name="toTint">Boolean whether to tint or not</param>
        /// <param name="tintColor">Colour to tint object</param>
        public static void AdjustSprite(Renderer rend, float depth, bool toBlur, float blurAmount, bool toTint, Color tintColor)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();

            rend.GetPropertyBlock(propBlock);

            propBlock.SetFloat("depth", depth);

            if (toBlur)
            {
                propBlock.SetFloat("radius", blurAmount);
            }

            if (toTint)
            {
                propBlock.SetColor("_Color", tintColor);
            }

            rend.SetPropertyBlock(propBlock);
        }
    }
}
