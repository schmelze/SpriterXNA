/*==========================================================================
 * Project: SpriterBetaPipelineExtension
 * File: SpriterBetaContent.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * The shadow-classes for SpriterBetaRuntime, must be kept in sync with
 * the runtime library, or bad things will happen
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;

namespace SpriterBetaPipelineExtension {
  /// <summary>
  /// Build-time type used to hold the output data from the SpriteSheetProcessor.
  /// This is serialized into XNB format, then at runtime, the ContentManager
  /// loads the data into a SpriteSheet object.
  /// </summary>
  [ContentSerializerRuntimeType("SpriterBetaRuntime.SpriterSubFrame, SpriterBetaRuntime")]
  public class ShadowSubFrame {
    // index into image rectangle list for sprite
    public int ImageIdx;
    // color and opacity of sprite
    public Color Tint;
    // angle in radians, clipped to +pi -pi
    public float Angle;
    // bool determining if sprite should be flipped horizontally
    public bool Xflip;
    // bool determining if sprite should be flipped vertically
    public bool Yflip;
    // scale from native rectangle size
    public Vector2 Size;
    // sprite offest from main position
    public Vector2 Position;
  }

  [ContentSerializerRuntimeType("SpriterBetaRuntime.SpriterFrame, SpriterBetaRuntime")]
  public class ShadowFrame {
    // list of sprites that compose a single frame
    public List<ShadowSubFrame> Sprites = new List<ShadowSubFrame>();
  }

  /// <summary>
  /// Build-time type used to hold the output data from the SpriteSheetProcessor.
  /// This is serialized into XNB format, then at runtime, the ContentManager
  /// loads the data into a SpriteSheet object.
  /// </summary>
  [ContentSerializerRuntimeType("SpriterBetaRuntime.SpriterAnimation, SpriterBetaRuntime")]
  public class ShadowAnimation {
    // animation name
    public string Name;

    // parallel lists FrameIdx, FrameDuration are paired

    // list of index into frame definition
    public List<int> FrameIdx = new List<int>();
    // list of duration in msecs for each frame
    public List<float> FrameDuration = new List<float>();
  }

  /// <summary>
  /// Build-time type used to hold the output data from the SpriteSheetProcessor.
  /// This is serialized into XNB format, then at runtime, the ContentManager
  /// loads the data into a SpriteSheet object.
  /// </summary>
  [ContentSerializerRuntimeType("SpriterBetaRuntime.SpriterCharacterData, SpriterBetaRuntime")]
  public class ShadowCharacterData {
    // Character name
    public string Name;

    // Single texture atlas combining all separate sprite images
    public Texture2DContent Texture = new Texture2DContent();
    // List containing location of each sprint on single texture
    public List<Rectangle> ImageRectangles = new List<Rectangle>();

    // List of animation data
    public List<ShadowAnimation> Animations = new List<ShadowAnimation>();
    // Gloal list of frame definition
    public List<ShadowFrame> Frames = new List<ShadowFrame>();
  }
}
