using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Phenicienn.Areas.Admin.Controllers.BareCode
{
    [Area("Admin")]
    public class BareCodeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

       [HttpPost]
        public ActionResult Index(string barCode)
        {
            //return View();
            //IFormFile file
            byte[] imageByte;
            QRCodeGenerator _qRCode = new QRCodeGenerator();
            QRCodeData  _qRCodeData = _qRCode.CreateQrCode(barCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(_qRCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            using(MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Png);
                imageByte = stream.ToArray();
            }

            ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(imageByte);
            return View();


           
           /* byte[] qrCode = GetByteArrayFromImage(file);

            //Bitmap bitmap = new Bitmap(@"C:\Users\hassan\Desktop\photos\download.png");

            Bitmap bitmap = ByteArrayToImage(qrCode);
            MessagingToolkit.QRCode.Codec.QRCodeDecoder decoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
            var txt = decoder.Decode(new QRCodeBitmapImage(bitmap as Bitmap));
            return View();*/
        }

       

        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }

        public static Bitmap ByteArrayToImage(byte[] source)
        {
            using (var ms = new MemoryStream(source))
            {
                return new Bitmap(ms);
            }
        }

    }
    
}