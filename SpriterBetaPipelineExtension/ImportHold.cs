/*==========================================================================
 * Project: SpriterBetaPipelineExtension
 * File: ImportHold.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Temporary classes for holding import data, includes extra information
 * necessary for cross-references
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpriterBetaPipelineExtension {
  /// <summary>
  /// Class to hold all information necessary to fill the shadow class
  /// frameName is used to store an index into the eventual subsprite data
  /// </summary>
  public class ImportFrame {
    // frame name (will be converted to index in processing)
    public string frameName;
    // list of sprite information for this frame
    public List<ShadowSubFrame> frameInfo = new List<ShadowSubFrame>();
  }
  /// <summary>
  /// Class to hold all information necessary to fill the shadow class
  /// This class holds the frame name and frame duration lists for a given animation
  /// </summary>
  public class ImportAnimation {
    // animation name
    public string name;
    // list of frame names (will be coverted to index in processing)
    public List<string> frameName=new List<string>();
    // list of frame durations
    public List<float> frameDuration=new List<float>();
  }
  /// <summary>
  /// Class to hold all information necessary to fill the shadow class
  /// This includes extra dictionary look-up for indexing
  /// </summary>
  public class ImportCharacterData {
    // name of character
    public string name;

    // list of animation data
    public List<ImportAnimation> anim = new List<ImportAnimation>();
    // list of frame definitions
    public List<ImportFrame> frames = new List<ImportFrame>();
    // list of unique sprite filenames
    public List<string> imageFiles = new List<string>();

    public List<Vector2> imageHotSpots = new List<Vector2>();

    // extra index information
    // imageName to imageFile list index
    public Dictionary<string, int> imageNames = new Dictionary<string, int>();
    // frameName to frames list index
    public Dictionary<string, int> frameNames = new Dictionary<string, int>();
  }
}
