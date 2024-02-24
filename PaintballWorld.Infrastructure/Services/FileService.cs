using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Logging;
using Microsoft.Extensions.Logging;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FileService> _logger;

        public FileService(ApplicationDbContext context, ILogger<FileService> logger)
        {
            _context = context;
            _logger = logger;
        }


        public string GetAttachmentPathById(AttachmentId attachmentId)
        {
            var attachmentPath = _context.Attachments.Single(x => x.Id == attachmentId).Path;
            if (attachmentPath is not null) 
                return attachmentPath;
            var errorMessage = $"Attachment with Id {attachmentId} does not exist";
            _logger.LogWarning(errorMessage);
            throw new Exception(errorMessage);
        }

        public string SaveFile(string path, Stream fileStream)
        {
            using var file = File.Create(path);
            fileStream.CopyTo(file);
            return path; 
        }

    }
}
