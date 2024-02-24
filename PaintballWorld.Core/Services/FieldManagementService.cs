﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var filename = GetPhotoFileName(fieldId);
            var directory = Path.Combine(Constants.FieldPhotosPath, fieldId.Value.ToString());
            Directory.CreateDirectory(directory);
            return _fileService.SaveFile(Path.Combine(directory, filename), stream);
        }

        private static string GetRegulationsFileName(FieldId fieldId) => $"Reg_{fieldId.Value}_{DateTime.Now:yyyy_MM_dd}.pdf";

        private string GetPhotoFileName(FieldId fieldId)
        {
            var photos = _context.Fields.First(x => x.Id == fieldId).Photos;

            if (photos == null || !photos.Any())
                return $"Photo_{fieldId.Value}_1.png";

            var maxNumber = photos.Select(photo => int.TryParse(photo.Path.Split('_').LastOrDefault(), out var number) ? number : 0).Max();

            return $"Photo_{fieldId.Value}_{maxNumber++}.png";

        }

    }
}
