/*==========================================================================
 * Project: SpriterBetaPipelineExtension
 * File: SpriterBetaImporter.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * Basic XML parser for the SpriterBeta format, this only parses a single
 * character - and only understands animations and frames.
 *
 * Not very fault tolerent, it assumes a well-formed XML file
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/

using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;

using TImport = SpriterBetaPipelineExtension.ImportCharacterData;

namespace SpriterBetaPipelineExtension {
  /// <summary>
  /// This class will be instantiated by the XNA Framework Content Pipeline
  /// to import a file from disk into the ImportCharacterData class for processing
  /// </summary>
  [ContentImporter(".SCML", DisplayName = "Spriter Beta Importer", DefaultProcessor = "SpriterBetaProcessor")]
  public class SpriterBetaImporter : ContentImporter<TImport> {
    public override TImport Import(string filename, ContentImporterContext context) {
      ImportCharacterData input = new ImportCharacterData();

      // basic XML reader, only pull what we are interested in
      XmlTextReader reader = new XmlTextReader(filename);

      // skip to first character
      reader.ReadToFollowing("char");

      // and pull the name
      reader.ReadToFollowing("name");
      input.name = reader.ReadElementContentAsString();

      int state = 0;
      string xmlNodeText, nodeName;
      ImportAnimation animation = null;  // class to build up an animation during parse
      ImportFrame frame = null;          // class to build up a frame definition during parse
      ShadowSubFrame sprite = null;      // class to build up sprite stats during parse

      nodeName = string.Empty;
      xmlNodeText = string.Empty;
      while (reader.Read()) {
        switch (reader.NodeType) {
          // open element
          case XmlNodeType.Element:
            nodeName = reader.Name;
            if (nodeName == "anim") {
              // start filling a new animation
              state = 1;
              animation = new ImportAnimation();
            } else if ((state == 1) && (nodeName == "frame")) {
              // frame information inside an "anim" node (contains frame name reference)
              state = 2;
            } else if (nodeName == "frame") {
              // actual frame definition (contains sprites)
              // so start filling a new frame object
              state = 3;
              frame = new ImportFrame();
            } else if ((state == 3) && (nodeName == "sprite")) {
              // start filling a sprite stats definition
              sprite = new ShadowSubFrame();
              sprite.Tint=Color.White;
              state = 4;
            }
            break;

          // element body
          case XmlNodeType.Text:
            xmlNodeText = reader.Value;
            break;

          // close element tag
          case XmlNodeType.EndElement:
            // fill in the current working item as appropriate
            if ((state == 1) && (nodeName == "name")) {
              animation.name = xmlNodeText;
            } else if ((state == 2) && (nodeName == "name")) {
              animation.frameName.Add(xmlNodeText);
            } else if ((state == 2) && (nodeName == "duration")) {
              float f = float.Parse(xmlNodeText)*10f;
              animation.frameDuration.Add(f);
            } else if ((state == 3) && (nodeName == "name")) {
              frame.frameName = xmlNodeText;
              input.frameNames.Add(xmlNodeText, input.frames.Count);
            } else if (state == 4) {
              if (nodeName == "image") {
                int idx;
                // see if image already exists
                if (!input.imageNames.TryGetValue(xmlNodeText, out idx)) {
                  // a new image filename, so add to dictionary with new index
                  idx = input.imageFiles.Count;
                  input.imageNames.Add(xmlNodeText, idx);
                  input.imageFiles.Add(xmlNodeText);
                }
                sprite.ImageIdx = idx;
              } else if (nodeName == "color") {
                int i = int.Parse(xmlNodeText);
                // isolate RGB channels and update color, preserving existing opacity
                sprite.Tint = new Color(i & 0xff, (i >> 8) & 0xff, (i >> 16) & 0xff, sprite.Tint.A);
              } else if (nodeName == "opacity") {
                // opacity ranges from 0-100, so convert to 0-255;
                float f = float.Parse(xmlNodeText)*25.5f;
                f=MathHelper.Clamp(f, 0, 255);
                // update color with new opacity information
                sprite.Tint = new Color(sprite.Tint.R, sprite.Tint.G, sprite.Tint.B, (int)f);
              } else if (nodeName == "angle") {
                // convert angle to radians, clamp to -/+ pi and negate
                // negation is required to match the rotation seen in Spriter
                float f = float.Parse(xmlNodeText);
                f = MathHelper.WrapAngle(MathHelper.ToRadians(f));
                sprite.Angle = -f;
              } else if (nodeName == "xflip") {
                int i = int.Parse(xmlNodeText);
                sprite.Xflip = (i > 0);
              } else if (nodeName == "yflip") {
                int i = int.Parse(xmlNodeText);
                sprite.Yflip = (i > 0);
              } else if (nodeName == "width") {
                // this will be converted to scale during processing
                float f = float.Parse(xmlNodeText);
                sprite.Size.X = f;
              } else if (nodeName == "height") {
                // this will be converted to scale during processing
                float f = float.Parse(xmlNodeText);
                sprite.Size.Y = f;
              } else if (nodeName == "x") {
                float f = float.Parse(xmlNodeText);
                sprite.Position.X = f;
              } else if (nodeName == "y") {
                float f = float.Parse(xmlNodeText);
                sprite.Position.Y = f;
              }
            }

            // when an element is closed, store the working classes on the main input class
            if (reader.Name == "anim") {
              input.anim.Add(animation);
              animation = null;
              state = 0;
            } else if ((state == 2) && (reader.Name == "frame")) {
              // update state variable to distinguish frame reference vs frame definition nodes
              state = 1;
            } else if ((state == 4) && (reader.Name == "sprite")) {
              frame.frameInfo.Add(sprite);
              state = 3;
              sprite = null;
            } else if ((state == 3) && (reader.Name == "frame")) {
              input.frames.Add(frame);
              frame = null;
              state = 0;
            }

            // clear data for next element
            xmlNodeText = string.Empty;
            nodeName = string.Empty;
            break;

          default:
            break;
        }
      }
      return input;
    }
  }
}
