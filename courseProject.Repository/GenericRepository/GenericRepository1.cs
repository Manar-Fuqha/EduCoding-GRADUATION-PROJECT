﻿using Microsoft.EntityFrameworkCore;
using courseProject.Core.IGenericRepository;
using courseProject.Repository.Data;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;


namespace courseProject.Repository.GenericRepository
{
    public class GenericRepository1<T> : IGenericRepository1<T> where T : class
    {
        private readonly projectDbContext dbContext;
        private IDbContextTransaction _currentTransaction;


        public GenericRepository1(projectDbContext dbContext )
        {
           
            this.dbContext = dbContext;
         
        }


      

       


        public async Task<IReadOnlyList<T>> GetAllEmployeeAsync()
        {
           
         return (IReadOnlyList<T>)await dbContext.users.Where(x=>x.role.ToLower()=="subadmin" || x.role.ToLower()=="instructor" || x.role.ToLower()=="main-subadmin")
                .Where(x => x.IsVerified == true).ToListAsync();
           
           
        }


       

        public async Task<int> saveAsync()
        => await dbContext.SaveChangesAsync();

        


        


        public async Task updateEmployeeAsync(T entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            
        }

        //Begins a new transaction asynchronously if there is no current transaction.
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                return null;
            }

            _currentTransaction = await dbContext.Database.BeginTransactionAsync();
            return _currentTransaction;
        }



       

    }
}
