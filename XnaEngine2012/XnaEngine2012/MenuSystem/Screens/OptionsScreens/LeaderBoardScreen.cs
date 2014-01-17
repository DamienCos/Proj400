#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace AndroidTest
{
    class LeaderBoardScreen : MenuScreen
    {
        #region Fields

        MenuEntry leaderBoardMenuEntry;


        static int volume = 0;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public LeaderBoardScreen()
            : base("LeaderBoard")
        {
            // Create our menu entries.
            leaderBoardMenuEntry = new MenuEntry(string.Empty);


            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            leaderBoardMenuEntry.Selected += VolMenuEntrySelected;//UngulateMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(leaderBoardMenuEntry);

            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            leaderBoardMenuEntry.Text = "leaderBoard text here: " + volume;//currentUngulate;

        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Ungulate menu entry is selected.
        /// </summary>



        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void VolMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            volume++;

            SetMenuEntryText();
        }


        #endregion
    }
}
