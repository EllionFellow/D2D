using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using D2D.GameObjects;
using SlimDX;
using SlimDX.Direct2D;
using SlimDX.Direct3D10;
using SlimDX.DirectWrite;

namespace D2D.Engine
{
    class d2dengine
    {
        #region //storage
        public Control targetControl;
        public WindowRenderTarget d2dWindowRenderTarget;
        public SlimDX.Direct2D.Factory d2dFactory;
        public SlimDX.DirectWrite.Factory wrtFactory;
        public DebugLevel debugLevel;
        public System.Drawing.Imaging.BitmapData bitmapData;
        public DataStream dataStream;
        public PixelFormat d2dPixelFormat;
        public BitmapProperties d2dBitmapProperties;
        public List<gameobject> ToDraw;
        #endregion


        #region //ctor
        public d2dengine(Control container, List<gameobject> theList)
        {
            
            ToDraw = theList;
            targetControl = container;
            debugLevel = DebugLevel.None;
            d2dFactory = new SlimDX.Direct2D.Factory(SlimDX.Direct2D.FactoryType.Multithreaded, debugLevel);
            wrtFactory = new SlimDX.DirectWrite.Factory(SlimDX.DirectWrite.FactoryType.Shared);
            d2dWindowRenderTarget = new WindowRenderTarget(d2dFactory, new WindowRenderTargetProperties()
            {
                Handle = targetControl.Handle,
                PixelSize = targetControl.Size,
                PresentOptions = PresentOptions.Immediately
            });
        }
        #endregion

        #region //MainLoop

        public void Render()
        {
            d2dWindowRenderTarget.BeginDraw();
            d2dWindowRenderTarget.Clear(new Color4(Color.Black));
            int sizex,sizey;
            foreach (gameobject curr in ToDraw)
            {
                if (curr.Game_object_state == 0)
                {
                    sizex = targetControl.Width;
                    sizey = targetControl.Height;
                }
                else
                {
                    sizex = 200;
                    sizey = 200;
                }
                if (curr.D2dbm == null)
                {
                    d2dPixelFormat = new PixelFormat(SlimDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);
                    d2dBitmapProperties = new BitmapProperties();
                    d2dBitmapProperties.PixelFormat = d2dPixelFormat;
                    
                    bitmapData = curr.Game_object_bitmap.LockBits(new Rectangle(new Point(0, 0), new Size(curr.Game_object_bitmap.Width, curr.Game_object_bitmap.Height)), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    dataStream = new DataStream(bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, true, false);
                    SlimDX.Direct2D.Bitmap temp = new SlimDX.Direct2D.Bitmap(d2dWindowRenderTarget, new Size(curr.Game_object_bitmap.Width, curr.Game_object_bitmap.Height), dataStream, bitmapData.Stride, d2dBitmapProperties);
                    curr.D2dbm = temp;
                    curr.Game_object_bitmap.UnlockBits(bitmapData);
                    d2dWindowRenderTarget.DrawBitmap(curr.D2dbm);
                    dataStream.Dispose();
                }
                else
                {
                    
                    //d2dWindowRenderTarget.DrawBitmap(curr.D2dbm, new Rectangle(curr.Game_object_position.X, curr.Game_object_position.Y, targetControl.Width, targetControl.Height));
                    d2dWindowRenderTarget.DrawBitmap(curr.D2dbm, new Rectangle(curr.Game_object_position.X, curr.Game_object_position.Y, sizex, sizey), 1.0f, InterpolationMode.Linear, new Rectangle(1024 * curr.Game_object_state, 0, 1024, curr.Game_object_bitmap.Height));
                }
            }



            d2dWindowRenderTarget.DrawRectangle(new SolidColorBrush(d2dWindowRenderTarget, new Color4(Color.Red)), new Rectangle(20, 20, targetControl.Width - 40, targetControl.Height - 40));
            d2dWindowRenderTarget.DrawText(Form1.counter.ToString(),new TextFormat(wrtFactory, "Arial", SlimDX.DirectWrite.FontWeight.Normal, SlimDX.DirectWrite.FontStyle.Normal, FontStretch.Normal, 18, "en-us") , new Rectangle(new Point(0, 0), new Size(200, 20)), new SolidColorBrush(d2dWindowRenderTarget, new Color4(Color.Blue)));
            d2dWindowRenderTarget.EndDraw();
            Form1.tempcounter++;
        }

        #endregion
    }
}
