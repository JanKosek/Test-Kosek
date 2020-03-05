using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using MVVM_Obrazky.ViewModel.Abstract;
using System.Threading;

namespace MVVM_Obrazky.ViewModel
{
    public class MainWindowViewModel
    {
        BitmapImage src;
        int width, height;
        int[,] array2D;
       
       
        

        Thread t = new Thread(toGreen);
       
        Command Browse { get; set;}
        Command ToGreen { get; set; }

        public MainWindowViewModel()
        {

            //Browse = new Command(BrowseButton_Click);
            ToGreen = new Command(t.Start);
        }
      
        public static void toGreen()
        {
            BitmapImage src = new BitmapImage();
            int[,] array2D;
            int width = src.PixelWidth;
            int height = src.PixelHeight;

            array2D = new int[src.PixelHeight, src.PixelWidth];

            WriteableBitmap wb = new WriteableBitmap(src);
            int Width = wb.PixelWidth;
            int Height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < Height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < Width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;
                        //bPixel[0] // Blue
                        //bPixel[1] // Green
                        //bPixel[2] // Red
                        int pixel = bPixel[2]; //Red
                        pixel = (pixel << 8) + bPixel[1]; //Green
                        pixel = (pixel << 8) + bPixel[0]; //Blue
                        array2D[row, col] = pixel;

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    array2D[x, y] = array2D[x, y] & 0x00FF00;
                }
            }

            // imgCanvas = Array2DToBitmapImage();
        }
        public void BitmapImageToArray2D()
        {
            array2D = new int[src.PixelHeight, src.PixelWidth];

            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;
                        //bPixel[0] // Blue
                        //bPixel[1] // Green
                        //bPixel[2] // Red
                        int pixel = bPixel[2]; //Red
                        pixel = (pixel << 8) + bPixel[1]; //Green
                        pixel = (pixel << 8) + bPixel[0]; //Blue
                        array2D[row, col] = pixel;

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();
        }

        private void BrowseButton_Click()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "Obrázek (.jpg)|*.jpg|Všechny soubory|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(filename, UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();

                
            }
        }

        private void RotateButton_Click()
        {
            //if (src != null) imgCanvas = src;
        }

        private void RotateButton_Click1()
        {
            width = src.PixelWidth;
            height = src.PixelHeight;

            BitmapImageToArray2D();

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    array2D[x, y] = array2D[x, y] & 0xFF0000;
                }
            }

           // imgCanvas = Array2DToBitmapImage();
        }

        public WriteableBitmap BlurBitmap()
        {
            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row+=2)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col+=2)
                    {
                        byte* bPixel = pImgData + cColStart;
                        byte* bPixel1 = pImgData + cColStart +1;
                        byte* bPixel2 = pImgData + cColStart + 2;
                        byte* bPixel3 = pImgData + cColStart + 3;

                        int blue = (int)((bPixel[0] + bPixel1[0] + bPixel2[0] + bPixel3[0]) / 4);
                        int green = (int)((bPixel[1] + bPixel1[1] + bPixel2[1] + bPixel3[1]) / 4);
                        int red = (int)((bPixel[2] + bPixel1[2] + bPixel2[2] + bPixel3[2]) / 4);

                        bPixel[0] = (byte)((array2D[row, col] & blue));
                        bPixel1[0] = (byte)((array2D[row, col] & blue));
                        bPixel2[0] = (byte)((array2D[row, col] & blue));
                        bPixel3[0] = (byte)((array2D[row, col] & blue));

                        bPixel[0] = (byte)((array2D[row, col] & green) >> 8);
                        bPixel1[0] = (byte)((array2D[row, col] & green) >> 8);
                        bPixel2[0] = (byte)((array2D[row, col] & green) >> 8);
                        bPixel3[0] = (byte)((array2D[row, col] & green) >> 8);

                        bPixel[0] = (byte)((array2D[row, col] & red) >> 16);
                        bPixel1[0] = (byte)((array2D[row, col] & red) >> 16);
                        bPixel2[0] = (byte)((array2D[row, col] & red) >> 16);
                        bPixel3[0] = (byte)((array2D[row, col] & red) >> 16);

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            return wb;
        }
      
        public WriteableBitmap Array2DToBitmapImage()
        {

            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;

                        bPixel[0] = (byte)((array2D[row, col] & 0xFF));// Blue
                        bPixel[1] = (byte)((array2D[row, col] & 0xFF00) >> 8);// Green
                        bPixel[2] = (byte)((array2D[row, col] & 0xFF0000) >> 16);// Red

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            return wb;
        }

        public WriteableBitmap ToGray()
        {
            WriteableBitmap wb = new WriteableBitmap(src);
            int width = wb.PixelWidth;
            int height = wb.PixelHeight;
            int bytesPerPixel = (wb.Format.BitsPerPixel + 7) / 8;
            int stride = wb.BackBufferStride;
            wb.Lock();
            unsafe
            {
                byte* pImgData = (byte*)wb.BackBuffer;
                int cRowStart = 0;
                int cColStart = 0;
                for (int row = 0; row < height; row++)
                {
                    cColStart = cRowStart;
                    for (int col = 0; col < width; col++)
                    {
                        byte* bPixel = pImgData + cColStart;

                        int gray = (int)((bPixel[0] + bPixel[1] + bPixel[2]) / 3);

                        bPixel[0] = (byte)((array2D[row, col] & gray));// Blue
                        bPixel[1] = (byte)((array2D[row, col] & gray) >> 8);// Green
                        bPixel[2] = (byte)((array2D[row, col] & gray) >> 16);// Red

                        cColStart += bytesPerPixel;
                    }
                    cRowStart += stride;
                }
            }
            wb.Unlock();
            wb.Freeze();

            return wb;

        }
    }
}
