/*==========================================================================
 * Project: SpriterBetaRuntime
 * File: SpriterCharacterData.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Spriter character data container
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpriterBetaRuntime {
  /// <summary>
  /// Class to hold all processed spriter data
  /// </summary>
  public class SpriterCharacterData {
    [ContentSerializer]
    // character name
    public string name;

    [ContentSerializer]
    // character sprite atlas
    public Texture2D texture = null;

    [ContentSerializer]
    // list of location of individual sprites on the atlas
    public List<Rectangle> imageRectangles = null;

    [ContentSerializer]
    public List<Vector2> imageHotspots = null;

    [ContentSerializer]
    // list of animation data (frame index, duration of each frame)
    public List<SpriterAnimation> animations = new List<SpriterAnimation>();

    [ContentSerializer]
    // list of frame definitions
    public List<SpriterFrame> frames = new List<SpriterFrame>();
  }
}
