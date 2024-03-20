﻿using System.Drawing;
using System.Drawing.Imaging;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;


namespace PaintballWorld.Core.Services
{
    public class FieldManagementService : IFieldManagementService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public FieldManagementService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public void CreateField(Field field)
        {
            _context.Fields.Add(field);
            _context.SaveChanges();
        }

        public FieldTypeId GetFieldTypeIdByStringName(string fieldType) =>
            _context.FieldTypes.Single(x => x.FieldTypeName.Equals(fieldType)).Id;

        public string SaveRegulationsFile(Stream stream, FieldId fieldId)
        {
            var filename = GetRegulationsFileName(fieldId);
            Directory.CreateDirectory(Constants.RegulationsPath); // TODO: Tworzyć te ścieżki przy starcie apki
            return _fileService.SaveFile(Path.Combine(Constants.RegulationsPath, filename), stream);
        }

        public string SavePhoto(Stream stream, FieldId fieldId)
        {
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(GetPhotoFileName(fieldId));
            var directory = Path.Combine(Constants.FieldPhotosPath, fieldId.Value.ToString());
            Directory.CreateDirectory(directory);

            var originalFilePath = Path.Combine(directory, $"{filenameWithoutExtension}.jpg");
            var thumbnailFilePath = Path.Combine(directory, $"{filenameWithoutExtension}.300.jpg");

            using var image = Image.FromStream(stream);

            image.Save(originalFilePath, ImageFormat.Jpeg);

            using var thumbnail = ResizeImage(image, 300);
            thumbnail.Save(thumbnailFilePath, ImageFormat.Jpeg);

            return originalFilePath;
        }

        public void SaveChanges(Field updRecord)
        {
            var field = _context.Fields.FirstOrDefault(x => x.Id == updRecord.Id);
            if (field == null)
                throw new Exception("Field not found");
            
            field.Address.City = updRecord.Address.City;
            field.Address.Country = updRecord.Address.Country;
            field.Address.Coordinates = updRecord.Address.Coordinates;
            field.Address.HouseNo = updRecord.Address.HouseNo;
            field.Address.Street = updRecord.Address.Street;
            field.Address.PhoneNo = updRecord.Address.PhoneNo;
            field.Address.PostalNumber = updRecord.Address.PostalNumber;
            field.Area = updRecord.Area;
            field.Name = updRecord.Name;
            field.Description = updRecord.Description;
            field.MinPlayers = updRecord.MinPlayers;
            field.MaxPlayers = updRecord.MaxPlayers;
            field.Sets = updRecord.Sets;
            field.MaxSimultaneousEvents = updRecord.MaxSimultaneousEvents;

            _context.Fields.Update(field);
            _context.SaveChanges();

        }

        private static string GetRegulationsFileName(FieldId fieldId) => $"Reg_{fieldId.Value}_{DateTime.Now:yyyy_MM_dd}.pdf";

        private string GetPhotoFileName(FieldId fieldId)
        {
            var photos = _context.Fields.First(x => x.Id == fieldId).Photos;

            if (photos == null || !photos.Any())
                return $"Photo_{fieldId.Value}_0.png";

            var maxNumber = photos.Select(photo => int.TryParse(photo.Path.Split('_').LastOrDefault(), out var number) ? number : 0).Max();

            return $"Photo_{fieldId.Value}_{maxNumber++}.png";

        }

        private static Image ResizeImage(Image image, int size)
        {
            var ratio = (double)size / (image.Width > image.Height ? image.Width : image.Height);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using var g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

    }
}
