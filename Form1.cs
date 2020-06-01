using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2D
{
    public partial class Form1 : Form
    {
        int i = 0;
        Control targetControl;
        SlimDX.Direct2D.WindowRenderTarget d2dWindowRenderTarget;
        SlimDX.Direct2D.Factory d2dFactory;
        SlimDX.Direct2D.DebugLevel debugLevel;
        Bitmap bitmap = Properties.Resources.Image_720p;//loaded from embedded resource, can be changed to Bitmap.FromFile(imageFile); to load from hdd!
        System.Drawing.Imaging.BitmapData bitmapData;
        SlimDX.DataStream dataStream;
        SlimDX.Direct2D.PixelFormat d2dPixelFormat;
        SlimDX.Direct2D.BitmapProperties d2dBitmapProperties;
        SlimDX.Direct2D.Bitmap d2dBitmap;
        public Form1()
        {
            bitmapData = bitmap.LockBits(new Rectangle(new Point(0, 0), bitmap.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);//TODO: PixelFormat is very important!!! Check!
            targetControl = this;
            InitializeComponent();
            //Update control styles, works for forms, not for controls. I solve this later otherwise .
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            debugLevel = SlimDX.Direct2D.DebugLevel.None;
            d2dFactory = new SlimDX.Direct2D.Factory(SlimDX.Direct2D.FactoryType.Multithreaded, debugLevel);
            d2dWindowRenderTarget = new SlimDX.Direct2D.WindowRenderTarget(d2dFactory, new SlimDX.Direct2D.WindowRenderTargetProperties()
            {
                Handle = targetControl.Handle,
                PixelSize = targetControl.Size,
                PresentOptions = SlimDX.Direct2D.PresentOptions.Immediately
            });
            //Convert System.Drawing.Bitmap into SlimDX.Direct2D.Bitmap!
            dataStream = new SlimDX.DataStream(bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, true, false);
            d2dPixelFormat = new SlimDX.Direct2D.PixelFormat(SlimDX.DXGI.Format.B8G8R8A8_UNorm, SlimDX.Direct2D.AlphaMode.Premultiplied);
            d2dBitmapProperties = new SlimDX.Direct2D.BitmapProperties();
            d2dBitmapProperties.PixelFormat = d2dPixelFormat;
            d2dBitmap = new SlimDX.Direct2D.Bitmap(d2dWindowRenderTarget, new Size(bitmap.Width, bitmap.Height), dataStream, bitmapData.Stride, d2dBitmapProperties);
            bitmap.UnlockBits(bitmapData);
            MainLoop();
        }

        private async void MainLoop()
        {
            while (true)
            {
                i++;
                if (i>100)
                {
                    i = 0;
                }
                await Task.Delay(1);
            }
        }

        private async void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Paint!
            d2dWindowRenderTarget.BeginDraw();
            d2dWindowRenderTarget.Clear(new SlimDX.Color4(Color.LightSteelBlue));

            d2dWindowRenderTarget.DrawRectangle(new SlimDX.Direct2D.SolidColorBrush(d2dWindowRenderTarget, new SlimDX.Color4(Color.Red)), new Rectangle(20, 20, targetControl.Width - 40, targetControl.Height - 40));
            //Draw SlimDX.Direct2D.Bitmap
            d2dWindowRenderTarget.DrawBitmap(d2dBitmap, new Rectangle(0, i, Width, Height));/**/
            
            if (i>100)
            {
                i = 0;
            }
            d2dWindowRenderTarget.EndDraw();
            bitmap.Dispose();
            await Task.Delay(10);
            Invalidate();
        }
    }
}
