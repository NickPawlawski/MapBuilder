using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapBuilder.Tile;
using MapBuilder.Biomes;

namespace MapBuilder
{
    public partial class MapBuilder : Form, IMessageFilter
    {
        private Control _control;
        private Form _mainForm;

        private GroupBox _mapGroupBox;
        private GroupBox _reportGroupBox;
        private GroupBox _controlBox;

        private Control[] _reportControls = new Control[10];

        private BuildMap _bm;

        public MapBuilder()
        {
            
            InitializeComponent();
            Application.AddMessageFilter(this);
            ControlsToMove.Add(this);

            
        }

        private void MapBuilder_Load(object sender, EventArgs e)
        {
            //CheckForIllegalCrossThreadCalls = false;
            _control = new Control();
            _mainForm = this;
            // Set form styles.
            _mainForm.FormBorderStyle = FormBorderStyle.None;
            _mainForm.BackColor = Color.Black; 
            _mainForm.Size = new Size(1500, 900);
            _control.Size = new Size((int)(_mainForm.Width * .005), (int)(_mainForm.Height * .005));

            // Create form elements.
            CreateGroupBoxes();
            BuildMap bm = new BuildMap(_mapGroupBox);
            _bm = bm;
            BuildControlBox();
            BuildTileDetailBox();
        }


        #region Private Methods
        private void BuildTileDetailBox()
        {
            Label dropLabel = CreateLabel(_reportGroupBox,.4,.1,_control,_control,.01,.01,Color.Black,"Drops Remaining", new Font("serif", 15));
            Label dropLabelRemaining = CreateLabel(_reportGroupBox, .4, .1, dropLabel, _control, .01, .01, Color.Black, "Drops Remaining", new Font("serif", 15));
            _reportControls[0] = dropLabelRemaining;
        }



        private void SetReportBox(object sender, EventArgs e)
        {
            int sum = 0;

            for(int i = 0; i < _bm.MapChanges.Count; i++)
            {
                sum += _bm.MapChanges[i].Count;
            }
            _reportControls[0].Text = sum.ToString();
            _reportControls[0].Refresh();
        }

        private void BuildControlBox()
        {
            Button startButton = CreateButton(_controlBox, .5, .1, _control, _control, .01, .02, Color.ForestGreen, Color.Black, "Start", new Font("serif",15));
            startButton.BringToFront();
            startButton.Click += _bm.Build;
            Button DropButton = CreateButton(_controlBox, .5, .1, _control, startButton, .01, .02, Color.ForestGreen, Color.Black, "SetDrops", new Font("serif", 15));
            DropButton.BringToFront();
            _bm.DropButton = DropButton;
            DropButton.Click += SetReportBox;
        }

        /// <summary>
        /// Sets up general boxes for UI.
        /// </summary>
        private void CreateGroupBoxes()
        {
            var mapBox = CreateGroupBox(_mainForm, .75, .95, _control, _control, .01, .025, Color.Black);
            _mapGroupBox = mapBox;
            _mapGroupBox.Font = new Font("serif",1000);
            _mapGroupBox.BringToFront();

            var reportBox = CreateGroupBox(_mainForm, .2, .45, _mapGroupBox, _control, .025, .025, Color.Gray);
            _reportGroupBox = reportBox;
            _reportGroupBox.Text = "Report";
            _reportGroupBox.BringToFront();

            var controlBox = CreateGroupBox(_mainForm,.2,.45,_mapGroupBox,_reportGroupBox,.025,.025, Color.Gray);
            _controlBox = controlBox;
            _controlBox.Text = "Controls";
            _controlBox.BringToFront();

        }

        #region Generic FormItem Creation

        /// <summary>
        /// Creates a single generic groupBox item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftControl"></param>
        /// <param name="aboveControl"></param>
        /// <param name="xPadding"></param>
        /// <param name="yPadding"></param>
        /// <param name="backColor"></param>
        /// <returns>Instance of new groupBox item.</returns>
        private GroupBox CreateGroupBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            GroupBox groupBox = new GroupBox();
            parent.Controls.Add(groupBox);
            groupBox.Height = ReturnHitConvert(height, parent);
            groupBox.Width = ReturnWidConvert(width, parent);
            groupBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            groupBox.BackColor = backColor;
            return groupBox;
        }


        /// <summary>
        /// Creates a single generic textBox item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="leftControl"></param>
        /// <param name="aboveControl"></param>
        /// <param name="xPadding"></param>
        /// <param name="yPadding"></param>
        /// <returns>Instance of new textBox item.</returns>
        private TextBox CreateTextbox(Control parent, double height, double width, Control leftControl,
            Control aboveControl, double xPadding, double yPadding)
        {
            TextBox textBox = new TextBox();
            parent.Controls.Add(textBox);
            textBox.AutoSize = false;
            textBox.Height = ReturnHitConvert(height, parent);
            textBox.Width = ReturnWidConvert(width, parent);
            textBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            textBox.BringToFront();
            return textBox;
        }


        /// <summary>
        /// Creates a single generic label item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftControl"></param>
        /// <param name="aboveControl"></param>
        /// <param name="xPadding"></param>
        /// <param name="yPadding"></param>
        /// <param name="foreColor"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns>Instance of new label item.</returns>
        private Label CreateLabel(Control parent, double width, double height, Control leftControl, Control aboveControl,
            double xPadding, double yPadding, Color foreColor, string text, Font font)
        {
            Label label = new Label();
            parent.Controls.Add(label);
            label.Height = ReturnHitConvert(height, parent);
            label.Width = ReturnWidConvert(width, parent);
            label.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            label.ForeColor = foreColor;
            label.Text = text;
            label.Font = font;
            return label;
        }


        /// <summary>
        /// Creates a single generic button item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftControl"></param>
        /// <param name="aboveControl"></param>
        /// <param name="xPadding"></param>
        /// <param name="yPadding"></param>
        /// <param name="backColor"></param>
        /// <param name="foreColor"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns>Instance of new button item.</returns>
        private Button CreateButton(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding,
            Color backColor, Color foreColor, string text, Font font)
        {
            Button button = new Button();
            parent.Controls.Add(button);
            button.Height = ReturnHitConvert(height, parent);
            button.Width = ReturnWidConvert(width, parent);
            button.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            button.BackColor = backColor;
            button.ForeColor = foreColor;
            button.Text = text;
            button.Font = font;
            return button;
        }



        /// <summary>
        /// Creates a single generic pictureBox item.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftControl"></param>
        /// <param name="aboveControl"></param>
        /// <param name="xPadding"></param>
        /// <param name="yPadding"></param>
        /// <param name="backColor"></param>
        /// <returns>Instance of new pictureBox item.</returns>
        // ReSharper disable once UnusedMember.Local
        private PictureBox CreatePictureBox(Control parent, double width, double height, Control leftControl,
            Control aboveControl, double xPadding, double yPadding, Color backColor)
        {
            PictureBox pictureBox = new PictureBox();
            parent.Controls.Add(pictureBox);
            pictureBox.Location =
                new Point(
                    ReturnWidConvert(
                        (leftControl.Location.X + leftControl.Size.Width) / (double)parent.Width + xPadding, parent),
                    ReturnHitConvert(
                        (aboveControl.Location.Y + aboveControl.Size.Height) / (double)parent.Height + yPadding, parent));
            pictureBox.Height = ReturnHitConvert(height, parent);
            pictureBox.Width = ReturnWidConvert(width, parent);
            pictureBox.BackColor = backColor;
            return pictureBox;
        }

        #endregion Generic FormItem Creation


        

        /// <summary>
        /// Creates the controls within the submit groupbox
        /// </summary>
        /// <param name="parent"></param>
        private void CreateSubmitButtons(Control parent)
        {
            /**Version Label
            var versionLabel = CreateLabel(parent, .4, .4, _control, _control, .01, .01, Color.FromArgb(255, 255, 185, 00),
                "Version 1.6.1", new Font("Century Gothic", ReturnFontConvert(13)));
            UtilityControls[(int)ControlEnums.VersionLabel] = versionLabel;
        */

            

        }


        #region Window Scaling

        private static int ReturnWidConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Width);
        }


        private static int ReturnHitConvert(double integer, Control control)
        {
            return (int)Math.Round(integer * control.Height);
        }


        private static int ReturnFontConvert(double fontSize)
        {
            return (int)Math.Round(fontSize * Screen.PrimaryScreen.Bounds.Width / 1920);
        }


        private static void PaintBorderlessGroupBox(object sender, PaintEventArgs p)
        {
            var box = (GroupBox)sender;
            p.Graphics.Clear(Color.FromArgb(68, 36, 22));
            p.Graphics.DrawString(box.Text, box.Font, Brushes.Black, 0, 0);
        }

        #endregion

        #endregion Private Methods

        #region PLZ NO TOUCH
        public const int WmNclbuttondown = 0xA1;
        public const int HtCaption = 0x2;
        public const int WmLbuttondown = 0x0201;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public HashSet<Control> ControlsToMove = new HashSet<Control>();

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WmLbuttondown || !ControlsToMove.Contains(FromHandle(m.HWnd))) return false;
            ReleaseCapture();
            SendMessage(Handle, WmNclbuttondown, HtCaption, 0);
            return true;
        }
        #endregion PLZ NO TOUCH



    }
}
