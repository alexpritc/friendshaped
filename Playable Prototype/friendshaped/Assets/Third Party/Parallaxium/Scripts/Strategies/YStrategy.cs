//-----------------------------------------------------------------------------
// YStrategy.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Strategy for Y axis parallaxing
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    public class YStrategy : MonoBehaviour, IParralaxStrategy
    {
        public void MoveLayer(List<GameObject> objects, float xDirection, float yDirection)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].transform.Translate(0, yDirection, 0);
            }
        }
    }
}