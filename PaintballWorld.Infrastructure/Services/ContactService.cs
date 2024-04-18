using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaintballWorld.Infrastructure.Interfaces;
using PaintballWorld.Infrastructure.Models;

namespace PaintballWorld.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;

        public ContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveContactMessage(string email, string content, string topic)
        {
            _context.Contacts.Add(new Contact
            {
                Email = email,
                Content = content,
                Topic = topic
            });
            await _context.SaveChangesAsync();
        } 



    }
}
