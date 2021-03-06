﻿//This file is part of cDashboard
//cDashboard - An information-based overlay for Microsoft Windows
//cSticky - A sticky note for widget for cDashboard
//(C) Charles Machalow under the MIT License
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cDashboard
{
    public partial class cSticky : cForm
    {
        public cSticky()
        {
            InitializeComponent();
        }

        /// <summary>
        /// form loading void
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cSticky_Load(object sender, EventArgs e)
        {
            //release lock on functions
            CompletedForm_Load = true;
            //focus on the rtb so that user can type right after making new sticky
            rtb.Focus();
        }

        /// <summary>
        /// save the text to a file when editted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_TextChanged(object sender, EventArgs e)
        {
            //to avoid file locks
            if (CompletedForm_Load == true)
            {
                rtb.SaveFile(SETTINGS_LOCATION + this.Name + ".rtf");
            }
        }

        /// <summary>
        /// handles key downs within the rtb
        /// used for formatting of selected text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtb_KeyDown(object sender, KeyEventArgs e)
        {
            //Ctrl + B makes selected text bold
            if (e.Control && e.KeyCode == Keys.B)
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style ^ FontStyle.Bold);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            //Ctrl + I makes selected text italicized
            if (e.Control && e.KeyCode == Keys.I)
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style ^ FontStyle.Italic);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            //Ctrl + U makes selected text underlined
            if (e.Control && e.KeyCode == Keys.U)
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style ^ FontStyle.Underline);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            //Ctrl + S makes selected text strikethrough
            if (e.Control && e.KeyCode == Keys.S)
            {
                rtb.SelectionFont = new Font(rtb.SelectionFont, rtb.SelectionFont.Style ^ FontStyle.Strikeout);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        #region MenuStrip

        /// <summary>
        /// change backcolor of the sticky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeBackcolorToolStripMenuItem_Click(object sender, EventArgs e)
        {

            DialogResult dr = colorDialog1.ShowDialog();

            //only do something, if user clicks ok
            if (dr == DialogResult.OK)
            {
                rtb.BackColor = colorDialog1.Color;
                menustrip.BackColor = colorDialog1.Color;
                this.BackColor = colorDialog1.Color;

                replaceSetting(new string[] { "cSticky", this.Name, "BackColor" }, new string[] { "cSticky", this.Name, "BackColor", rtb.BackColor.R.ToString(), rtb.BackColor.G.ToString(), rtb.BackColor.B.ToString() });
            }
        }

        /// <summary>
        /// change the font of the current cSticky
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //make fontdialog reflect current font of rtb
            fontDialog1.Font = rtb.SelectionFont;

            DialogResult dr = fontDialog1.ShowDialog();

            //only do something, if user clicks ok
            if (dr == DialogResult.OK)
            {
                rtb.Font = fontDialog1.Font;
                rtb.ForeColor = fontDialog1.Color;
            }
        }

        /// <summary>
        /// brings up the save file dialog to save the current cSticky's richtext
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveThisTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        /// <summary>
        /// saves the rtb text to the savefiledialog1's filename
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            rtb.SaveFile(saveFileDialog1.FileName);
        }

        /// <summary>
        /// form close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// used to move form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menustrip_MouseDown(object sender, MouseEventArgs e)
        {
            dragForm(ref e);
        }
        #endregion

        #region ContextMenu

        /// <summary>
        /// undo recent text changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            rtb.Undo();
        }

        /// <summary>
        /// standard cut content to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Cut();
        }

        /// <summary>
        /// standard copy content to clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Copy();
        }

        /// <summary>
        /// standard paste content from clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.Paste();
        }

        /// <summary>
        /// standard text-based select all function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtb.SelectAll();
        }

        #endregion

        /// <summary>
        /// grabs all relevant Windows Messages
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            formResizer(ref m);
        }

    }
}
