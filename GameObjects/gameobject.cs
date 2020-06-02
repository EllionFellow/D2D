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
        System.Drawing.Bitmap game_object_bitmap;
        Point game_object_position;
        int game_object_state;

        #endregion

        #region //ctor
        public gameobject(System.Drawing.Bitmap bitmap, int x, int y)
        {
            game_object_position = new Point(x, y);
            game_object_bitmap = bitmap;
            game_object_state = 0;
        }
        #endregion

        public Point Game_object_position { get => game_object_position; set => game_object_position = value; }
        public int Game_object_state { set => game_object_state = value; get => game_object_state; }
        public int StateX { get => (game_object_state-1) * 64; }
        public int StateY { get => 0; }
        public System.Drawing.Bitmap Game_object_bitmap { get => game_object_bitmap; }
    }
}
