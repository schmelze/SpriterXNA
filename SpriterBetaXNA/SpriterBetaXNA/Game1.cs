/*==========================================================================
 * Project: SpriterBetaXNA
 * File: Game1.cs
 * Copyright(C) 2012 Mark Schmelzenbach
 *
 * SpriterXNA "demo" - embarrassingly simple use of the SpriterBetaRuntime
 *
 * Load a spriter character and display it
 * use the {A} button or spacebar to advance through animation sequences
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
using Microsoft.Xna.Framework.Input;

using SpriterBetaRuntime;

namespace SpriterBetaXNA {
  /// <summary>
  /// This is the main type for the demo
  /// </summary>
  public class Game1 : Microsoft.Xna.Framework.Game {
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    KeyboardState prevKeyState;
    GamePadState prevPadState;

    SpriteFont guiFont;
    Vector2 charNamePos;
    Vector2 animNamePos;

    SpriterCharacter hero;

    public Game1() {
      graphics = new GraphicsDeviceManager(this);
      prevKeyState = Keyboard.GetState();
      prevPadState = GamePad.GetState(PlayerIndex.One);

      graphics.PreferredBackBufferWidth = 1280;
      graphics.PreferredBackBufferHeight = 720;

      Content.RootDirectory = "Content";
    }

    /// <summary>
    /// Perform any initialization it needs to before starting to run.
    /// </summary>
    protected override void Initialize() {
      // TODO: Add initialization logic here
      base.Initialize();
    }

    /// <summary>
    /// LoadContent will be called once per game and is the place to load
    /// all of your content.
    /// </summary>
    protected override void LoadContent() {
      // Create a new SpriteBatch, which can be used to draw textures.
      spriteBatch = new SpriteBatch(GraphicsDevice);

      // load font
      guiFont = Content.Load<SpriteFont>("fonts/gui-font");
      charNamePos = new Vector2(32, 8);
      animNamePos = charNamePos + new Vector2(0, guiFont.LineSpacing);

      // load spriter data
      SpriterCharacterData animData = Content.Load<SpriterCharacterData>("characters/BetaFormatHero");

      // now create a new spriter character with this data
      hero = new SpriterCharacter(animData);

      // position in the middle of the screen
      hero.Position = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);

      // and set animation sequence
      hero.CurrentSequence = 0;
    }

    /// <summary>
    /// UnloadContent will be called once per game and is the place to unload
    /// all content.
    /// </summary>
    protected override void UnloadContent() {
      // TODO: Unload any non ContentManager content here
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Update(GameTime gameTime) {
      // Allows the game to exit
      if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)||(Keyboard.GetState().IsKeyDown(Keys.Escape)))
        this.Exit();

      // silly keyboard/gamepad read routines to see if a button is pressed and released
      KeyboardState newKeyState = Keyboard.GetState();
      GamePadState newPadState = GamePad.GetState(PlayerIndex.One);
      bool changeAnimation = false;
      if ((newPadState.Buttons.A != ButtonState.Pressed)&&(prevPadState.Buttons.A==ButtonState.Pressed)) {
        changeAnimation = true;
      }
      if ((!newKeyState.IsKeyDown(Keys.Space)) && (prevKeyState.IsKeyDown(Keys.Space))) {
        changeAnimation = true;
      }
      prevKeyState = newKeyState;
      prevPadState = newPadState;

      // change displayed animation sequence
      if (changeAnimation) {
        int num = hero.CurrentSequence;
        num++;
        if (num >= hero.SequenceCount)
          num = 0;
        hero.CurrentSequence = num;
      }

      // update hero animation
      hero.Update(gameTime);

      base.Update(gameTime);
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime) {
      GraphicsDevice.Clear(Color.CornflowerBlue);

      // begin sprite batch
      spriteBatch.Begin();

      // update labels
      spriteBatch.DrawString(guiFont, hero.Name, charNamePos, Color.Yellow);
      spriteBatch.DrawString(guiFont, hero.CurrentSequenceName, animNamePos, Color.White);

      // draw our hero
      hero.Draw(spriteBatch);

      // and done
      spriteBatch.End();

      base.Draw(gameTime);
    }
  }
}
