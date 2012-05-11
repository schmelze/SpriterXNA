/*==========================================================================
 * Project: SpriterBetaRuntime
 * File: SpriterFrame.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Spriter frame and subframe container
 *
 * TODO: Combine xFlip, yFlip to a basic effect during processing and use
 *       directly instead of using flags
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SpriterBetaRuntime {
  /// <summary>
  /// Information for each sprite that make up a frame
  /// </summary>
  public class SpriterSubFrame {
    [ContentSerializer]
    // index into texture atlas
    int imageIndex=0;

    [ContentSerializer]
    // color and opacity of sprite
    Color tint = Color.White;

    [ContentSerializer]
    // rotation of sprite around top left corner
    float angle = 0f;

    [ContentSerializer]
    // flag indicating horizontal flip
    bool xflip = false;

    [ContentSerializer]
    // flag indicating vertical flip
    bool yflip = false;

    [ContentSerializer]
    // sprite scaling from top left corner
    Vector2 size = Vector2.Zero;

    [ContentSerializer]
    // sprite offset from base position
    Vector2 position = Vector2.Zero;

    /// <summary>
    /// image index into texture atlas
    /// </summary>
    public int ImageIndex { get { return imageIndex; } }

    /// <summary>
    /// sprite color and opacity
    /// </summary>
    public Color Tint { get { return tint; } }

    /// <summary>
    /// sprite rotation around top left corner
    /// </summary>
    public float Rotation { get { return angle; } }

    /// <summary>
    /// flag indicating if sprite should be drawn with horizontal flip
    /// </summary>
    public bool XFlip { get { return xflip; } }

    /// <summary>
    /// flag indicating if sprite should be drawn with vertical flip
    /// </summary>
    public bool YFlip { get { return yflip; } }

    /// <summary>
    /// sprite scaling factor
    /// </summary>
    public Vector2 Size { get { return size; } }

    /// <summary>
    /// sprite offset from base position
    /// </summary>
    public Vector2 Position { get { return position; } }
  }

  /// <summary>
  /// Class containing the list of sprites composing a single frame
  ///
  /// A frame definition is a single image composed of potentially many
  /// sprites, each sprite individually rotated, scaled, moved, etc.
  /// </summary>
  public class SpriterFrame {
    [ContentSerializer]
    // list of sprite data
    public List<SpriterSubFrame> sprites;
  }
}
