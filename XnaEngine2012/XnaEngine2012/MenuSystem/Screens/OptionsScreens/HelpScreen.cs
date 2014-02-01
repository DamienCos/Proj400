#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace Blocker
{
    class HelpScreen : MenuScreen
    {
        #region Fields

        MenuEntry helpMenuEntry;


        //static int volume = 0;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public HelpScreen()
            : base("Help")
        {
            // Create our menu entries.
            helpMenuEntry = new MenuEntry(string.Empty);


            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            helpMenuEntry.Selected += HelpMenuEntrySelected;//UngulateMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(helpMenuEntry);

            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            helpMenuEntry.Text = "Help text here: ";
        }


        #endregion

        #region Handle Input

        /// <summary>
        /// Event handler for when the help menu entry is selected.
        /// </summary>
        void HelpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            SetMenuEntryText();
        }


        #endregion
    }
}
