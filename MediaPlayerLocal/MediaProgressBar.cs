using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaPlayerLocal
{
    public partial class MediaProgressBar : UserControl
    {
        private const int offsetX = 2, offsetY = 8;
        private bool _isDragging = false;

        public float Value { get; set; } = 0.0f; // 0.0 to 1.0
        public bool IsDragging {  get { return _isDragging; } }
        public event EventHandler<float> SeekPerformed;

        public MediaProgressBar()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            Rectangle mainRect = new Rectangle(offsetX, offsetY, 
                this.Width - offsetX * 2, this.Height - offsetY * 2);
            
            g.DrawRectangle(new Pen(Brushes.Gray), mainRect);

            int progressWidth = (int)(mainRect.Width * Value);
            
            Rectangle fillRect = new Rectangle(offsetX, offsetY, 
                progressWidth, this.Height - offsetY * 2);

            // 2. Draw Progress (the "Played" part)
            g.FillRectangle(Brushes.DeepSkyBlue, fillRect);

            // 3. Draw "Thumb" (the handle)
            Rectangle handleRect = new Rectangle(offsetX + progressWidth - 2, 0, 4, this.Height);
            g.FillRectangle(Brushes.Orange, handleRect);

        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if(e.X > offsetX && e.X < this.Width - offsetX)
            {

                _isDragging = true;
                UpdateValueFromMouse(e.X);

                // Capture the mouse so it continues to track even if the cursor 
                // moves slightly outside the control's height while dragging.
                this.Capture = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (_isDragging)
            {
                UpdateValueFromMouse(e.X);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (_isDragging)
            {
                _isDragging = false;
                this.Capture = false;

                // Final notification to the Presenter to perform the actual Seek
                SeekPerformed?.Invoke(this, Value);
            }
        }

        private void UpdateValueFromMouse(int mouseX)
        {
            // Constrain the mouseX within the playable bar bounds
            int constrainedX = Math.Max(offsetX, Math.Min(mouseX, this.Width - offsetX));

            int relMouseX = constrainedX - offsetX;
            Value = (float)relMouseX / (this.Width - offsetX * 2);

            this.Invalidate(); // Forces OnPaint to be called immediately
        }
    }
}
