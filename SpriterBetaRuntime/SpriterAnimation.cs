/*==========================================================================
 * Project: SpriterBetaRuntime
 * File: SpriterAnimation.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * SpriterAnimation container
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace SpriterBetaRuntime {
  /// <summary>
  /// Class containing all data for a given animation
  ///
  /// An animation is conceptually a number of frames that loop
  /// An eye-blink might look like:
  /// Frame 1: Frame Definition 1, 500 msec (open eye)
  /// Frame 2: Frame Definition 2, 10 msec  (eye partially closed)
  /// Frame 3: Frame Definition 3, 20 msec  (eye closed)
  /// Frame 4: Frame Definition 2, 10 msec  (eye partially closed, again)
  ///
  /// The class is a parallel list of frame definition indicies, frame durations
  /// </summary>
  public class SpriterAnimation {
    [ContentSerializer]
    // animation name
    public string name;

    [ContentSerializer]
    // list of frame definitions
    List<int> frameIdx = null;

    [ContentSerializer]
    // list of frame durations
    List<float> frameDuration = null;

    /// <summary>
    /// Return the duration for a given animation frame
    /// </summary>
    /// <param name="idx">the animation frame to retrieve</param>
    /// <returns>the duration of the animation frame in msecs</returns>
    public float GetFrameDuration(int idx) {
      return frameDuration[idx];
    }

    /// <summary>
    /// Return the frame definition index for a given animation frame
    /// </summary>
    /// <param name="idx">the animation frame to retrieve</param>
    /// <returns>the frame definition index to use</returns>
    public int GetFrameIdx(int idx) {
      return frameIdx[idx];
    }

    /// <summary>
    /// Return total number of frames in the animation
    /// </summary>
    /// <returns>the total number of frames in the animation</returns>
    public int GetTotalFrames() {
      return frameIdx.Count;
    }
  }
}
