using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MonkeyJobTool.Helpers
{
    public class ImageHelper
    {
        //ResizeImage(GetFromBase64(@"iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAA8gAAAPIBlLUtiQAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAMnSURBVFiFrddPiNVVFAfwz280S/FPg0JjmoJjI/FqVRC1iKY0CnNhiwoikEohimhTRLWaFkVQURAtpsXUIsKyCYJwZ9QuLDB6FBEYRpkSJmhpNdNtcc+rO2/evN97zrtwePzOPff7/d7fuefc36tSSvoZVVUtRQPX4rpwHw5rppRm+wJMKfVkWInXcA5pAfsDL2FFz7g9kt+GH7oQt9v3uGUgAjDRB3G7Pb0oAdiJfxYhYBbbL0gANuLXRZC37BeMXIiATwZA3rKDfQnAtmLxcezBKtyE13GiA8nxqJIbsBp7cbKY39yPgKeKhTd2mL8UXxUxh7GqQ9z2IubxfgR8Hos+65Kiy3EU32Bdl7iW0E+7CkCFMewuTv7+miq5ElfUxHxcVMSu4KjmCMB47KY9r9N1dVxb5xzsgHsU4zGvgT/RxMN4pAg8hSUF2HK8g/cxXPjX4UO8jWWF/yKcKfAeCo5mcDZgGmexFo/i2Ta1NxeAuwr/g4V/b+HfUfhvb8N6Qa6oNcE5TS6pSQwVh6+05wvAtXGovlWUFUbxHb7EmsL/Sge8AzE3GdyOtXIt1/p0EfwFLl5E/pfjSIE32UpR8ByDKcxga0xsjuDT2DKAQzgW52AGq8O3NZ6nYETuWOfxFt4IAbu7gF6FTW0kjS7x9wbmq8FxPl7/SCtgIz6IXSd8VLOrplwh23BN7LBZs+ZQYP+G97BhTiOKoEvkJvREDdi+AHtZvhsS7q9ZM4G/FWXdScCWALuzh9yOR/ouw/U9xN8d2BtK/1Jzx8r4PalmpJQOFY8n6uLl7wJYUTqrVjuEqqqG5dyekl/XIMcyDMsfrOc6CggRP+F3HBiwgHuQUkqjc7wdcvWiXCab6vLaRy8YxV+YmDe3QPAZTA1QwLtyic/b1EIL7pI71X0DIH8gsHZ2nO+y8Dm5Jzy2CPIn5dJ7ZsGYLouH4tUlvInRPojH5Jab4rfqW0ABdofcemewH7difYe49dght/RZ+dru+qekJwEBvkT+kvnZ/1frWfmqPSKXbcv/o/yBMtQL9rw+UDeqqhrB1WGNOCdfx1tqppR66Yr/jX8BP2Kylh1/D3UAAAAASUVORK5CYII="), 26, 26)
        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Image GetFromBase64(string base64String)
        {
            byte[] data = Convert.FromBase64String(base64String);
            using (var stream = new MemoryStream(data, 0, data.Length))
            {
                Image image = Image.FromStream(stream);
                return image;
            }
        }

        public static Icon GetIconWithNotificationCount(string text, Icon sourceIcon, Color? textColor = null, Color? backgroundColor = null, int fontSize = 12, string fontName = "Tahoma", Color? iconBorderColor = null, bool useEllipseAsBackground=false)
        {
            Icon createdIcon;
            using (Bitmap bitmap = new Bitmap(16, 16))
            {
                Icon icon = sourceIcon;
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    using (SolidBrush drawBrush = new SolidBrush(textColor??Color.White))
                    {
                        using (Font drawFont = new Font(fontName, fontSize, FontStyle.Regular))
                        {
                            var measure = TextRenderer.MeasureText(text, drawFont);
                            var x = sourceIcon.Width - measure.Width;
                            if (x < -1)
                                x = -1;
                            else
                                x += iconBorderColor.HasValue ? 3 : 4;


                            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                            graphics.DrawIcon(icon, 0, 0);
                            if (useEllipseAsBackground)
                            {
                                graphics.FillEllipse(new SolidBrush(backgroundColor ?? Color.Black), x-4, sourceIcon.Height - measure.Height + 1, measure.Width, measure.Height);
                                graphics.DrawString(text, drawFont, drawBrush, x-2, sourceIcon.Height - measure.Height + 1);
                            }
                            else
                            {
                                if (backgroundColor.HasValue)
                                    graphics.FillRectangle(new SolidBrush(backgroundColor.Value), x, sourceIcon.Height - measure.Height + 2, measure.Width, measure.Height - 1);
                                graphics.DrawString(text, drawFont, drawBrush, x, sourceIcon.Height - measure.Height + 1);
                                if(iconBorderColor.HasValue)
                                    graphics.DrawRectangle(new Pen(iconBorderColor.Value,1), 0, 0, 15, 15);
                                
                            }
                            
                            createdIcon = Icon.FromHandle(bitmap.GetHicon());
                        }
                    }
                }
            }

            return createdIcon;
        }
    }
}
