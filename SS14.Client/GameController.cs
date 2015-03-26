﻿using SS14.Client.Interfaces.Configuration;
using SS14.Client.Interfaces.Input;
using SS14.Client.Interfaces.Network;
using SS14.Client.Interfaces.Resource;
using SS14.Client.Interfaces.State;
using SS14.Client.Interfaces.UserInterface;
using SS14.Client.Services.State.States;
using SS14.Shared.IoC;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SFML.Graphics;
using SFML.Window;
using SS14.Client.Graphics.CluwneLib;
using SS14.Client.Graphics.CluwneLib.Event;
using SS14.Client.Graphics.CluwneLib.Render;
using Color = System.Drawing.Color;
using KeyArgs = SFML.Window.KeyEventArgs;
using SS14.Client.Graphics.CluwneLib.Sprite;
using SS14.Client.Graphics.CluwneLib.Timing;



namespace SS14.Client
{
    public class GameController 
    {


        #region Fields

        private IConfigurationManager _configurationManager;
      //  private Input _input;
        private INetworkGrapher _netGrapher;
        private INetworkManager _networkManager;
        private IStateManager _stateManager;
        private IUserInterfaceManager _userInterfaceManager;
        private IResourceManager _resourceManager;

        #endregion

        #region Properties

        #endregion

        #region Methods

        #region Constructors

        public GameController()
        {
            _configurationManager = IoCManager.Resolve<IConfigurationManager>();
            IoCManager.Resolve<IConfigurationManager>().Initialize("./config.xml");

            _resourceManager = IoCManager.Resolve<IResourceManager>();

            _resourceManager.LoadBaseResources();
            _resourceManager.LoadLocalResources();
          

            //Initialization of private members
           
            _networkManager = IoCManager.Resolve<INetworkManager>();
            _netGrapher = IoCManager.Resolve<INetworkGrapher>();
            _stateManager = IoCManager.Resolve<IStateManager>();
            _userInterfaceManager = IoCManager.Resolve<IUserInterfaceManager>();

        
           

            //Setup
            SetupCluwne();
            SetupInput();


            #region testing
            TextSprite CluwneEngineText = new TextSprite("TEST", "CluwneEngine", _resourceManager.GetFont("CALIBRI"));
            CluwneEngineText.Position = new Shared.Maths.Vector2(450,600);
            CluwneEngineText.Color = Color.DarkRed;
            CluwneEngineText.Text = " SS14: Running on CluwneEngine";

            TextSprite VersionText = new TextSprite("TEST", "version", _resourceManager.GetFont("CALIBRI"));
            VersionText.Position = new Shared.Maths.Vector2(500, 650);
            VersionText.Color = Color.Gold;
            VersionText.Text = "( Running SFML v2.0 ) " ;

            TextSprite ProjNotDeadText = new TextSprite("TEST", "ProjNoDed", _resourceManager.GetFont("CALIBRI"));
            ProjNotDeadText.Position = new Shared.Maths.Vector2(512, 700);
            ProjNotDeadText.Color = Color.Gold;
            ProjNotDeadText.Text = "  Project != Dead :)";
            

            //CluwneLib.drawHollowRectangle(100, 100, 100, 100, .6f, Color.Blue);
            //CluwneLib.drawPoint(134, 223, Color.Beige);
            //CluwneLib.drawCircle(121, 142, 20, Color.Crimson);


            Texture Cluwnelogo = new Texture(_resourceManager.GetImage("Textures/CluwneLibLogo.png"));

            CluwneSprite CluwneEngineLogo = new CluwneSprite(Cluwnelogo);
            CluwneEngineLogo.Position = new SFML.System.Vector2f(150,100);

            Texture _Tiles = new Texture(_resourceManager.GetImage("Textures/0_Tiles.png"));

            CluwneSprite _TilesSprite = new CluwneSprite(_Tiles);
            _TilesSprite.Position = new SFML.System.Vector2f(0, 0);


            Texture _Items = new Texture(_resourceManager.GetImage("Textures/0_Items.png"));

            CluwneSprite _TilesItems = new CluwneSprite(_Items);
            _TilesItems.Position = new SFML.System.Vector2f(0, 500);

            Texture _Objects = new Texture(_resourceManager.GetImage("Textures/0_Objects.png"));

            CluwneSprite _TilesObjects = new CluwneSprite(_Objects);
            _TilesObjects.Position = new SFML.System.Vector2f(970, 400);

            Texture _Decals = new Texture(_resourceManager.GetImage("Textures/0_Decals.png"));

            CluwneSprite _TilesDecals = new CluwneSprite(_Decals);
            _TilesDecals.Position = new SFML.System.Vector2f(1000, 0);



            _TilesObjects.Draw();
            _TilesDecals.Draw();
            _TilesItems.Draw();
            _TilesSprite.Draw();
            CluwneEngineText.Draw();
            VersionText.Draw();
            CluwneEngineLogo.Draw();
            ProjNotDeadText.Draw();

            CluwneLib.Screen.Display();

            //States Testing
            _stateManager.RequestStateChange<MainScreen>();
	    System.Threading.Thread.Sleep(1000);

	    FrameEventArgs _frameEvent = new FrameEventArgs(
			    new TimingData(new SFML.System.Clock()));

             while(CluwneLib.Screen.IsOpen == true)
                {
                    CluwneLib.Clear(Color.Black);
                    CluwneLib.Screen.DispatchEvents();
                    CluwneLibIdle(this, _frameEvent);
                    CluwneLib.Screen.Display();
                }



        }
            #endregion

        #endregion



        #region EventHandlers

        private void CluwneLibIdle(object sender, FrameEventArgs e)
        {
            _networkManager.UpdateNetwork();
            _stateManager.Update(e);

            _userInterfaceManager.Update(e.FrameDeltaTime);
            _userInterfaceManager.Render();

            _netGrapher.Update();
        }

        private void MainWindowLoad(object sender, EventArgs e)
        {

            _stateManager.RequestStateChange<MainScreen>();
        }

        private void MainWindowResizeEnd(object sender, EventArgs e)
        {
	    _stateManager.FormResize();
        }

        #region Input Handling

        /// <summary>
        /// Handles any keydown events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.KeyboardInputEventArgs"/> instance containing the event data.</param>
        private void KeyDownEvent(object sender, KeyArgs e)
        {
            if(_stateManager!=null)
                _stateManager.KeyDown(e);

            switch (e.Code)
            {
                case Keyboard.Key.F3:
                    IoCManager.Resolve<INetworkGrapher>().Toggle();
                    break;
            }
        }

        /// <summary>
        /// Handles any keyup events.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.KeyboardInputEventArgs"/> instance containing the event data.</param>
        private void KeyUpEvent(object sender, KeyArgs e)
        {
            if (_stateManager != null) 
                _stateManager.KeyUp(e);
        }

        /// <summary>
        /// Handles mouse wheel input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.MouseInputEventArgs"/> instance containing the event data.</param>
        private void MouseWheelMoveEvent(object sender, MouseWheelEventArgs e)
        {
            if (_stateManager != null) 
                _stateManager.MouseWheelMove(e);
        }

        /// <summary>
        /// Handles any mouse input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.MouseInputEventArgs"/> instance containing the event data.</param>
        private void MouseMoveEvent(object sender, MouseMoveEventArgs e)
        {
            if (_stateManager != null) 
                _stateManager.MouseMove(e);
        }

        /// <summary>
        /// Handles any mouse input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.MouseInputEventArgs"/> instance containing the event data.</param>
        private void MouseDownEvent(object sender, MouseButtonEventArgs e)
        {
            if (_stateManager != null) 
                _stateManager.MouseDown(e);
        }

        /// <summary>
        /// Handles any mouse input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GorgonLibrary.InputDevices.MouseInputEventArgs"/> instance containing the event data.</param>
        private void MouseUpEvent(object sender, MouseButtonEventArgs e)
        {
            if (_stateManager != null) 
                _stateManager.MouseUp(e);
        }

        #endregion

        #endregion

        #region Privates

//        private void SetupGorgon()
//        {
//            uint displayWidth = _configurationManager.GetDisplayWidth();
//            uint displayHeight = _configurationManager.GetDisplayHeight();
//            bool fullscreen = _configurationManager.GetFullscreen();
//            var refresh = (int) _configurationManager.GetDisplayRefresh();
//            Size = new Size((int) displayWidth, (int) displayHeight);
//
//            //TODO. Find first compatible videomode and set it if no configuration is present. Else the client might crash due to invalid videomodes on the first start.
//
//            CluwneLib.Initialize();
//            //Gorgon.SetMode(this);
//            CluwneLib.SetMode(this, (int) displayWidth, (int) displayHeight, BackBufferFormats.BufferRGB888, !fullscreen,
//                           false, false, refresh);
//            CluwneLib.Screen.BackgroundColor = Color.FromArgb(50, 50, 50);
//            CluwneLib.CurrentClippingViewport = new Viewport(0, 0, CluwneLib.Screen.Width, CluwneLib.Screen.Height);
//            CluwneLib.DeviceReset += MainWindowResizeEnd;
//            //Gorgon.MinimumFrameTime = PreciseTimer.FpsToMilliseconds(66);
//            CluwneLib.Idle += GorgonIdle;
//        }

        private void SetupCluwne()
        {
            uint displayWidth = _configurationManager.GetDisplayWidth();
            uint displayHeight = _configurationManager.GetDisplayHeight();
            bool fullscreen = _configurationManager.GetFullscreen();
            var refresh = (int) _configurationManager.GetDisplayRefresh();

          
             

            CluwneLib.Go();
           
            CluwneLib.SetMode((int) displayWidth, (int) displayHeight, fullscreen,false,false,refresh);
            CluwneLib.Screen.BackgroundColor = CluwneLib.SystemColorToSFML(Color.Black);
            CluwneLib.CurrentClippingViewport = new Viewport(0, 0, CluwneLib.Screen.Size.X, CluwneLib.Screen.Size.Y);
            CluwneLib.Screen.Resized += MainWindowResizeEnd;
            CluwneLib.Idle += CluwneLibIdle;
        }

        private void SetupInput()
        {

            //Cursor.Hide();
     

            CluwneLib.Screen.KeyPressed  += KeyDownEvent;
            CluwneLib.Screen.KeyReleased += KeyUpEvent;

            CluwneLib.Screen.MouseButtonPressed  += MouseDownEvent;
            CluwneLib.Screen.MouseButtonReleased += MouseUpEvent;
            CluwneLib.Screen.MouseMoved          += MouseMoveEvent;
            CluwneLib.Screen.MouseWheelMoved     += MouseWheelMoveEvent;

            IoCManager.Resolve<IKeyBindingManager>().Initialize();
        }

        #endregion

        #endregion
    }
}
    