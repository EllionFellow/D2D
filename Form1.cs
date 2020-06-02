using D2D.Engine;
using D2D.GameObjects;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2D
{
    
    public partial class Form1 : Form
    
    {
        public static int counter = 0, tempcounter = 0;
        Bitmap bgBitmap = Properties.Resources.Image_720p;//loaded from embedded resource, can be changed to Bitmap.FromFile(imageFile); to load from hdd!
        d2dengine painter;
        List<gameobject> everyone = new List<gameobject>();
        public Form1()
        {
            everyone.Add(new gameobject(bgBitmap, 0, 0));
            InitializeComponent();
            painter = new d2dengine(this, everyone);
            StartLoop();
            timer1.Enabled = true;
            timer1.Interval = 1000;
            timer1.Start();
        }

        private async void StartLoop()
        {
            while (true)
            {
                await Task.Run(painter.Render);
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            counter = tempcounter;
            tempcounter = 0;
        }
    }
}
