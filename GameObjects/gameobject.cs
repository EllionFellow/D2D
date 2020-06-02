using D2D.Engine;
using SlimDX;
using SlimDX.Direct2D;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2D.GameObjects
{
    class gameobject
    {
        #region //storage
        Point game_object_position;
        public Point Game_object_position { get => game_object_position; set => game_object_position = value; }
        public int Game_object_state { set; get; }
        public int StateX { get => (Game_object_state-1) * 64; }
        public int StateY { get => 0; }
        public System.Drawing.Bitmap Game_object_bitmap { get; set; }
        public SlimDX.Direct2D.Bitmap D2dbm { get; set; }
        #endregion



        #region //ctor
        public gameobject(System.Drawing.Bitmap bitmap, int x, int y, d2dengine painter, int state, int step)
        {
            game_object_position = new Point(x, y);
            Game_object_bitmap = bitmap;
            Game_object_state = state;
        }
        #endregion
    }
}
