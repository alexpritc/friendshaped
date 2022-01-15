//-----------------------------------------------------------------------------
// IParallaxStrategy.cs by Iain Carr
// Copyright (c) 2020 Iain Carr - Parallaxium. All Rights Reserved.
//
// Interface for parallax direction strategies
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace ParallaxiumBeta
{
    public interface IParralaxStrategy
    {
        void MoveLayer(List<GameObject> objects, float xDirection, float yDirection);
    }
}
