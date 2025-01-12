using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Tedx.Helper
{
  
public class QrCodeGenerator
    {
        public static byte[] GenerateQrCode( string text)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            using (var stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }


}

