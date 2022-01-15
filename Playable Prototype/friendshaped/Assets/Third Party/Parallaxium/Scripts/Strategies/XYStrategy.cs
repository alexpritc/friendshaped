//-----------------------------------------------------------------------------
// XYStrategy.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Strategy for XY axis parallaxing
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    public class XYStrategy : MonoBehaviour, IParralaxStrategy
    {
        public void MoveLayer(List<GameObject> objects, float xDirection, float yDirection)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].transform.Translate(xDirection, yDirection, 0);
            }
        }
    }
}
