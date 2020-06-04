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
        Bitmap bgBitmap = Properties.Resources._1;
        d2dengine painter;
        List<gameobject> everyone = new List<gameobject>();
        public Form1()
        {
            everyone.Add(new gameobject(bgBitmap, 0, 0, painter,0,0));
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
                everyone[0].Game_object_position = new Point(counter/100,tempcounter/100);
                if (everyone.Count>1)
                {
                    if (everyone[1].Game_object_state != 3)
                    {
                        everyone[1].Game_object_state++;
                    }
                    else
                    {
                        everyone[1].Game_object_state = 1;
                    }
                }
                    await Task.Run(painter.Render);
                
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        private void Form1_SizeChanged(object sender, System.EventArgs e)
        {
            painter.targetControl.Width = Width;
            painter.targetControl.Height = Height;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    var raven = new gameobject(Properties.Resources.thecrow, 20, 20, painter,1,1024);
                    everyone.Add(raven);
                    break;
                default:
                    break;
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            counter = tempcounter;
            tempcounter = 0;
        }
    }
}
