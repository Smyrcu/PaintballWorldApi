using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Core.Interfaces;
using PaintballWorld.Infrastructure;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Core.Services
{
    public class FieldManagementService : IFieldManagementService
    {
        private readonly ApplicationDbContext _context;

        public FieldManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateField(Field field)
        {
            _context.Fields.Add(field);
            _context.SaveChanges();
        }

        public FieldTypeId GetFieldTypeIdByStringName(string fieldType) =>
            _context.FieldTypes.Single(x => x.FieldTypeName.Equals(fieldType)).Id;
        
    }
}
