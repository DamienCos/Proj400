#region Using Statements
using Microsoft.Xna.Framework;
#endregion

namespace Blocker
{
    class ControlsScreen : MenuScreen
    {
        #region Fields

        MenuEntry controlsMenuEntry;


        static int volume = 0;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ControlsScreen()
            : base("Controls")
        {
            // Create our menu entries.
            controlsMenuEntry = new MenuEntry(string.Empty);


            SetMenuEntryText();

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            controlsMenuEntry.Selected += VolMenuEntrySelected;//UngulateMenuEntrySelected;

            back.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(controlsMenuEntry);

            MenuEntries.Add(back);
        }


        /// <summary>
        /// Fills in the latest values for the options screen menu text.
        /// </summary>
        void SetMenuEntryText()
        {
            controlsMenuEntry.Text = "Control Text here";

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
