using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Enums;
using DAL.Interfaces;
using DAL.Services;

namespace DAL.Factories
{
    public static class DataAccessFactory
    {
        public static IDataRepository<T> Create<T>(DataAccessType Type, string ConnectionString) where T: IDataMember, IDisposable
        {
            switch (Type)
            {
                default:
                    return new SQLiteService<T>(ConnectionString);

            }
        }
    }
}