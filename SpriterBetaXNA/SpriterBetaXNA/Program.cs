/*==========================================================================
 * Project: SpriterBetaXNA
 * File: Program.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * SpriterXNA "demo" - embarassingly simple use of the SpriterBetaRuntime
 *
 *==========================================================================
 * Author:
 *    Mark Schmelzenbach <schmelze@gmail.com>
 *==========================================================================*/
using System;

namespace SpriterBetaXNA {
#if WINDOWS || XBOX
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main(string[] args) {
      using (Game1 game = new Game1()) {
        game.Run();
      }
    }
  }
#endif
}

