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
        private Control targetControl;
        private WindowRenderTarget d2dWindowRenderTarget;
        SlimDX.Direct2D.Factory d2dFactory;
        SlimDX.DirectWrite.Factory wrtFactory;
        DebugLevel debugLevel;
        System.Drawing.Imaging.BitmapData bitmapData;
        DataStream dataStream;
        PixelFormat d2dPixelFormat;
        BitmapProperties d2dBitmapProperties;
        SlimDX.Direct2D.Bitmap d2dBitmap;
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
            d2dPixelFormat = new PixelFormat(SlimDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied);
            d2dBitmapProperties = new BitmapProperties();
            
            foreach (gameobject curr in ToDraw)
            {
                if (curr.Game_object_state != 0)
                {
                    bitmapData = curr.Game_object_bitmap.LockBits(new Rectangle(new Point(curr.StateX, curr.StateY), new Size(64, 64)), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    dataStream = new DataStream(bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, true, false);
                    d2dBitmapProperties.PixelFormat = d2dPixelFormat;
                    d2dBitmap = new SlimDX.Direct2D.Bitmap(d2dWindowRenderTarget, new Size(64, 64), dataStream, bitmapData.Stride, d2dBitmapProperties);
                    curr.Game_object_bitmap.UnlockBits(bitmapData);
                    d2dWindowRenderTarget.DrawBitmap(d2dBitmap, new Rectangle(curr.Game_object_position.X, curr.Game_object_position.Y, 64, 64));
                }
                else
                {
                    bitmapData = curr.Game_object_bitmap.LockBits(new Rectangle(new Point(0,0), new Size(curr.Game_object_bitmap.Width,curr.Game_object_bitmap.Height)), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                    dataStream = new DataStream(bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, true, false);
                    d2dBitmapProperties.PixelFormat = d2dPixelFormat;
                    d2dBitmap = new SlimDX.Direct2D.Bitmap(d2dWindowRenderTarget, new Size(targetControl.Width, targetControl.Height), dataStream, bitmapData.Stride, d2dBitmapProperties);
                    curr.Game_object_bitmap.UnlockBits(bitmapData);
                    d2dWindowRenderTarget.DrawBitmap(d2dBitmap, new Rectangle(0, 0, targetControl.Width, targetControl.Height));
                }
                dataStream.Dispose();
                d2dBitmap.Dispose();
            }



            d2dWindowRenderTarget.DrawRectangle(new SolidColorBrush(d2dWindowRenderTarget, new Color4(Color.Red)), new Rectangle(20, 20, targetControl.Width - 40, targetControl.Height - 40));
            d2dWindowRenderTarget.DrawText(Form1.counter.ToString(),new TextFormat(wrtFactory, "Arial", SlimDX.DirectWrite.FontWeight.Normal, SlimDX.DirectWrite.FontStyle.Normal, FontStretch.Normal, 18, "en-us") , new Rectangle(new Point(0, 0), new Size(200, 20)), new SolidColorBrush(d2dWindowRenderTarget, new Color4(Color.White)));
            d2dWindowRenderTarget.EndDraw();
            Form1.tempcounter++;
        }

        #endregion
    }
}
