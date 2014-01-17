#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace AndroidTest
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class OptionsMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry volumeMenuEntry;
        MenuEntry controlMenuEntry;
        MenuEntry helpMenuEntry;
        MenuEntry leaderboardMenuEntry;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public OptionsMenuScreen()
            : base("Options")
        {
            // Create our menu entries.
            volumeMenuEntry = new MenuEntry("Volume");
            controlMenuEntry = new MenuEntry(string.Empty);
            helpMenuEntry = new MenuEntry("Help");
            leaderboardMenuEntry = new MenuEntry(string.Empty);

            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            volumeMenuEntry.Selected += VolumeScreenSelected;
            controlMenuEntry.Selected += ControlScreenSelected;
            helpMenuEntry.Selected += HelpScreenSelected;
            leaderboardMenuEntry.Selected += LeaderBoardScreenSelected;
            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(volumeMenuEntry);
            MenuEntries.Add(controlMenuEntry);
            MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(leaderboardMenuEntry);
            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            volumeMenuEntry.Text = "Volume ";// +currentUngulate;
            controlMenuEntry.Text = "Controls: ";// +languages[currentLanguage];
            helpMenuEntry.Text = "Help: ";// +(frobnicate ? "on" : "off");
            leaderboardMenuEntry.Text = "Leaderboard ";// +elf;
        }


        #endregion

        #region Handle Input


        void VolumeScreenSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new VolumeScreen(), e.PlayerIndex);
        }

        void HelpScreenSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HelpScreen(), e.PlayerIndex);
        }

        void LeaderBoardScreenSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LeaderBoardScreen(), e.PlayerIndex);
        }

        void ControlScreenSelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(), e.PlayerIndex);
        }


        #endregion
    }
}
