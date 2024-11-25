﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ContactRepository : RepositoryBase<Contact>, IContactRepository
    {
        private readonly RepositoryContext _dbContext;

        public ContactRepository(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            await _dbContext.Contacts.AddAsync(contact);
            await _dbContext.SaveChangesAsync();

            return contact;
        }

        public void DeleteContact(Contact contact)
        {
            Delete(contact);
        }

        public async Task<(IEnumerable<Contact> Contacts, int Total)> GetAllContactAsync(bool trackChanges, int type, int pageNumber, int pageSize)
        {
            var query = FindAll(trackChanges);


            // Đếm tổng số bản ghi sau khi lọc
            int totalCount = await query.CountAsync();

            // Áp dụng phân trang
            var pagedContacts = await query
                .OrderBy(x => x.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (pagedContacts, totalCount);
        }

        public async Task<Contact> GetContactByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(x => x.Id.Equals(id), trackChanges).FirstOrDefaultAsync();
        }
    }
}
