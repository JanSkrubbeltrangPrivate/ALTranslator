using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Dapper;

namespace DAL.Services
{
    public class SQLiteService<T> : IDataRepository<T> where T: IDataMember, IDisposable
    {
        private string _connectionString;
        private SQLiteConnection _connection;

        public SQLiteService(string ConnectionString)
        {
            _connectionString = ConnectionString;
            _connection = new(ConnectionString);
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            if (!(_connection.State == ConnectionState.Open))
                _connection.Open();
            string Query = $"Delete from {nameof(T)}s where Id = {entity.Id}";
            _connection.ExecuteScalar<T>(Query);
        }

        public async Task DeleteAsync(T entity)
        {
            if (!(_connection.State == ConnectionState.Open))
                await _connection.OpenAsync();
            string Query = $"Delete from {nameof(T)}s where Id = {entity.Id}";
            await _connection.ExecuteScalarAsync<T>(Query);
        }

        public T Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
            _connection.Dispose();
        }
    }
}