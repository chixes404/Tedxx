using QRCoder;
using System.IO;


namespace Tedx.Helper
{
  
public class QrCodeGenerator
    {
        public static byte[] GenerateQrCode( string text)
        {
            // Create QR code data
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);

            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }
    }


}

