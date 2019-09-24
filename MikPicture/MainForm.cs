using System;
using System.Windows.Forms;
using Granges.MikPicture.Core;

namespace Granges.MikPicture
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Click event of the btnBrowseSource control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnBrowseSource_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtFolderSource.Text = folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnGO control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void BtnGO_Click(object sender, EventArgs e)
        {
            ResizeManager resizeManager = new ResizeManager(txtFolderSource.Text, (long)nudQuality.Value);
            resizeManager.Resizing += new EventHandler<ResizeEventArgs>(ResizeManager_Resizing);
            resizeManager.Ended += new EventHandler(ResizeManager_Ended);
            resizeManager.Run();
        }

        /// <summary>
        /// Handles the Resizing event of the resizeManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MikPicture.ResizeEventArgs"/> instance containing the event data.</param>
        private void ResizeManager_Resizing(object sender, ResizeEventArgs e)
        {
            lblInformation.Text = string.Format("Conversion de '{0}'", e.Filename);
            //Application.DoEvents();
        }

        /// <summary>
        /// Handles the Ended event of the resizeManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ResizeManager_Ended(object sender, EventArgs e)
        {
            lblInformation.Text = "Conversion terminée.";
        }
    }
}