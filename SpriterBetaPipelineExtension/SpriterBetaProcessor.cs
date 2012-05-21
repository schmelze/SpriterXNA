/*==========================================================================
 * Project: SpriterBetaPipelineExtension
 * File: SpriterBetaProcessor.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Convert the ImportHold classes into the runtime shadow-class versions
 * Also, creates a sprite atlas from the referrenced sprites.
 *
 *  Note: At the moment, only one sheet is created, which could cause problems
 *  with animations containing a large number of parts.
 *
 * Sprite atlas creation is based on the XNA SpriteSheetSample project
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *    Additional code from XNA SpriteSheetSample
 *==========================================================================*/

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using TInput = SpriterBetaPipelineExtension.ImportCharacterData;
using TOutput = SpriterBetaPipelineExtension.ShadowCharacterData;

namespace SpriterBetaPipelineExtension {
  /// <summary>
  /// This class will be instantiated by the XNA Framework Content Pipeline
  /// to apply custom processing to content data, converting an object of
  /// type TInput to TOutput.
  /// </summary>
  ///
  [ContentProcessor(DisplayName = "Spriter Beta Animation")]
  public class SpriterBetaProcessor : ContentProcessor<TInput, TOutput> {
    public override TOutput Process(TInput input, ContentProcessorContext context) {
      ShadowCharacterData spriterData = new ShadowCharacterData();

      spriterData.Name = input.name;

      // build sprite atlas
      BuildSpriteSheet(input, spriterData, context);

      // convert animation list
      spriterData.Animations = new List<ShadowAnimation>();
      foreach (ImportAnimation anim in input.anim) {
        ShadowAnimation sAnim=new ShadowAnimation();
        sAnim.Name = anim.name;
        sAnim.FrameDuration = anim.frameDuration;
        sAnim.FrameIdx = new List<int>();
        // convert frame name into frame definition index
        foreach (string frameName in anim.frameName) {
          int idx;
          if (input.frameNames.TryGetValue(frameName, out idx)) {
            sAnim.FrameIdx.Add(idx);
          } else {
            // missing frame reference... so throw
            throw new InvalidContentException("Missing frame definition '"+frameName+"'");
          }
        }
        spriterData.Animations.Add(sAnim);
      }

      // frame definition list is basically just a copy
      spriterData.Frames = new List<ShadowFrame>();
      foreach (ImportFrame frame in input.frames) {
        ShadowFrame sframe = new ShadowFrame();
        sframe.Sprites = frame.frameInfo;
        // convert absolute size to scale
        foreach (ShadowSubFrame sprite in sframe.Sprites) {
          Vector2 source = new Vector2(spriterData.ImageRectangles[sprite.ImageIdx].Width, spriterData.ImageRectangles[sprite.ImageIdx].Height);
          if (source.X == 0f) source.X = 1f;
          if (source.Y == 0f) source.Y = 1f;
          sprite.Size = new Vector2(sprite.Size.X / source.X, sprite.Size.Y / source.Y);
        }
        spriterData.Frames.Add(sframe);
      }

      spriterData.ImageHotspots = input.imageHotSpots;

      return spriterData;
    }

    /// <summary>
    /// Convert sprites into sprite sheet object
    /// (Basically from XNA SpriteSheetSample project)
    /// </summary>
    public void BuildSpriteSheet(ImportCharacterData input, ShadowCharacterData sprite, ContentProcessorContext context) {
      List<BitmapContent> sourceSprites = new List<BitmapContent>();

      // Load each sprite texture
      foreach (string inputFilename in input.imageFiles) {
        string spriteName = Path.GetFileNameWithoutExtension(inputFilename);
        ExternalReference<TextureContent> textureReference = new ExternalReference<TextureContent>(inputFilename);
        TextureContent texture = context.BuildAndLoadAsset<TextureContent, TextureContent>(textureReference, "TextureProcessor");
        sourceSprites.Add(texture.Faces[0][0]);
      }

      // Pack all the sprites onto a single texture.
      BitmapContent packedSprites = SpritePacker.PackSprites(sourceSprites, sprite.ImageRectangles, context);
      sprite.Texture.Mipmaps.Add(packedSprites);
    }
  }
}
