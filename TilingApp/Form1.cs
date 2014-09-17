using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows;

namespace TilingApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            int tilewidth = Convert.ToInt32(this.textBoxWidth.Text);
            tilewidth = tilewidth <= 0 ? 256 : tilewidth;

            var tileheight = Convert.ToInt32(this.textBoxHeight.Text);
            tileheight = tileheight <= 0 ? 256 : tileheight;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "all files | *.*"; // file types, that will be allowed to upload
            dialog.Multiselect = false; // allow/deny user to upload more than one file at a time
            if (dialog.ShowDialog() == DialogResult.OK) // if user clicked OK
            {
                String path = dialog.FileName; // get name of file
                //using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open), new UTF8Encoding())) // do anything you want, e.g. read it
                //{
                //}

                //var mainImage = System.Windows.Application.GetResourceStream(new Uri("MainImage.jpg", UriKind.Relative)).Stream;
                //WriteableBitmap mainWb = new WriteableBitmap(480, 800, 96,
                //96,
                //System.Windows.Media.PixelFormats.Bgr32,
                //null);
                //var bytesPrPixel = mainWb.Format.BitsPerPixel / 8;
                //System.Windows.Int32Rect rect = new System.Windows.Int32Rect(0,0,width,height);
                //var stride = width * bytesPrPixel;
                ////var pixelBuffer = new Byte(){};
                //var bytes = new byte[bytesPrPixel];
                //mainWb.CopyPixels(rect,bytes, bytesPrPixel, stride);

                Bitmap source = new Bitmap(path);
                var sourceWidth = source.Width;
                var sourceHeight = source.Height;
                var horisoltalNumberOfTiles = Math.Ceiling((double)sourceWidth / tilewidth);
                var verticalNumberOfTiles = Math.Ceiling((double)sourceHeight/tileheight);
                int remainderWidth = sourceWidth % tilewidth;
                int remainderHeight = sourceHeight % tileheight;
                int numFiles = 0;
                string newDirectory = Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), textBoxPrefix.Text);
                Directory.CreateDirectory(newDirectory);
                for(int x = 0 ; x < horisoltalNumberOfTiles;x++ )
                {
                    var calculatedTileWidth = x  +1 == horisoltalNumberOfTiles ? tilewidth - remainderWidth : tilewidth;
                    for (int y = 0; y < verticalNumberOfTiles; y++)
                    {
                        var calculatedTileHeight = y + 1 == verticalNumberOfTiles ? tileheight - remainderHeight: tileheight; 
                        Rectangle section = new Rectangle(new System.Drawing.Point(x * tilewidth, y*tileheight), 
                            new System.Drawing.Size(calculatedTileWidth, calculatedTileHeight));
                        Bitmap croppedImage = CropImage(source, section);
                        var outputPath = Path.Combine(newDirectory,string.Format("{0}_{1}_{2}.png", this.textBoxPrefix.Text, x, y));
                        this.textBoxOutput.AppendText(string.Format("writing {0}_{1}_{2}.png {3}", this.textBoxPrefix.Text, x, y, Environment.NewLine));
                        numFiles ++;
                        croppedImage.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }

                this.textBoxOutput.AppendText(string.Format("Done writing {0} files to {1}", numFiles, newDirectory));
            }

            //Bitmap.Clone(Rectangle, PixelFormat) 
        }

        public Bitmap CropImage(Bitmap source, Rectangle section)
        {
            // An empty bitmap which will hold the cropped image
            Bitmap bmp = new Bitmap(section.Width, section.Height);

            Graphics g = Graphics.FromImage(bmp);

            // Draw the given area (section) of the source image
            // at location 0,0 on the empty bitmap (bmp)
            g.DrawImage(source, 0, 0, section, GraphicsUnit.Pixel);

            return bmp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxWidth_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
