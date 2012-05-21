/*==========================================================================
 * Project: SpriterBetaRuntime
 * File: SpriterCharacter.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Spriter character logic
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
  /// Logic for Spriter character
  ///
  /// This class is responsible for updating and drawing each spriter character
  ///
  /// </summary>
  public class SpriterCharacter {
    // current animation sequence being played
    int currentSequence = 0;

    // current index into animation frames
    int currentFrameIdx = 0;

    // current frame definition being displayed
    int currentFrame = 0;

    // how long to hold the current frame
    System.TimeSpan deltaTime;

    // time current frame has been shown, so far
    System.TimeSpan tic;

    // animation data
    SpriterCharacterData character;

    public bool FlipX;
    public bool FlipY;

    /// <summary>
    /// Construct a new spriter character
    /// </summary>
    /// <param name="animationData">Character animation data, loaded from a SCML file</param>
    public SpriterCharacter(SpriterCharacterData animationData) {
      this.character = animationData;
      CurrentSequence = 0;
    }

    /// <summary>
    /// current frame definition being displayed
    /// </summary>
    public int CurrentFrame {
      get { return currentFrame; }
    }

    /// <summary>
    /// current animation sequence being displayed
    /// </summary>
    public int CurrentSequence {
      get { return currentSequence; }
      set {
        if (value < SequenceCount) {
          currentSequence = value;
          currentFrameIdx = 0;
          // reset elapsed time
          tic = new System.TimeSpan(0);
          // get the initial frame duration
          deltaTime = new System.TimeSpan(0, 0, 0, 0, (int)character.animations[currentSequence].GetFrameDuration(0));
          // get the initial frame definition
          currentFrame = character.animations[currentSequence].GetFrameIdx(0);
        }
      }
    }

    /// <summary>
    /// Character position
    /// </summary>
    public Vector2 Position {
      get;
      set;
    }

    /// <summary>
    /// Character name from the SCML file
    /// </summary>
    public string Name {
      get { return character.name; }
    }

    /// <summary>
    /// Current animation name
    /// </summary>
    public string CurrentSequenceName {
      get {
        return character.animations[currentSequence].name;
      }
    }

    /// <summary>
    /// Number of animation sequences
    /// </summary>
    public int SequenceCount {
      get {
        return character.animations.Count;
      }
    }

    /// <summary>
    /// Update the character's animation frames
    /// </summary>
    /// <param name="time">gametime since the last call to update</param>
    public void Update(GameTime time) {
      if (tic > deltaTime) {
        // if enough time has passed, update current frame information
        // technically, this is being calculated incorrectly, we should be updating tic more carefully,
        // and allowing for multiple frames to pass during a single update
        // however, at 60fps and typical animation rates, this is fine for now
        tic = new System.TimeSpan(0);
        currentFrameIdx++;
        if (currentFrameIdx == character.animations[currentSequence].GetTotalFrames()) {
          currentFrameIdx = 0;
        }
        int msecs = (int)character.animations[currentSequence].GetFrameDuration(currentFrameIdx);
        deltaTime = new System.TimeSpan(0, 0, 0, 0, msecs);
        currentFrame = character.animations[currentSequence].GetFrameIdx(currentFrameIdx);
      }
      tic += time.ElapsedGameTime;
    }

    Vector2 tmpPosition;
    Vector2 tmpOrigin;
    float tmpRotation;
    SpriteEffects effects;

    /// <summary>
    /// Draw the character
    /// </summary>
    /// <param name="spriteBatch">spritebatch to use for drawing</param>
    public void Draw(SpriteBatch spriteBatch) {
      // draw each component of the current frame
      foreach (SpriterSubFrame sprite in character.frames[currentFrame].sprites) {
        effects = SpriteEffects.None;  // TODO: pre-calculate this, assuming we don't allow xflip, yflip of full sprite
        if (sprite.XFlip != FlipX) effects |= SpriteEffects.FlipHorizontally;
        if (sprite.YFlip != FlipY) effects |= SpriteEffects.FlipVertically;

        tmpOrigin = character.imageHotspots[sprite.ImageIndex];
        if (FlipX) { tmpOrigin.X = character.imageRectangles[sprite.ImageIndex].Width - tmpOrigin.X; }
        if (FlipY) { tmpOrigin.Y = character.imageRectangles[sprite.ImageIndex].Height - tmpOrigin.Y; }

        tmpPosition = Position;
        tmpPosition.X += (sprite.Position.X) * (FlipX ? -1 : 1);
        tmpPosition.Y += (sprite.Position.Y) * (FlipY ? -1 : 1);

        tmpRotation = sprite.Rotation;
        if (FlipX != FlipY) { tmpRotation *= -1; }

        spriteBatch.Draw(character.texture, tmpPosition, character.imageRectangles[sprite.ImageIndex],
          sprite.Tint, tmpRotation, tmpOrigin, sprite.Size, effects, 0);
      }
    }
  }
}
