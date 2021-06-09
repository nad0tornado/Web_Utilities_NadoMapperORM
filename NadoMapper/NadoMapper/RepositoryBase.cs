using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using NadoMapper.Models;
using NadoMapper.Conventions;
using Pluralize;
using Pluralize.NET;

namespace NadoMapper
{
    
    public class RepositoryBase<TEntity> where TEntity : ModelBase
    {
        private Pluralizer _pluralizer;
        private string _modelName => typeof(TEntity).Name;
        private string _modelNamePlural => _pluralizer.Pluralize(_modelName);

        /* private DataContext<TEntity> _dataContext = new DataContext<TEntity>();

        public bool VerifyInitialize()
        {
            _dataContext.LoadConnectionString("Data Source=localhost;Initial Catalog=TestDB;Integrated Security=True;");
            _dataContext.VerifyInitialize();
            _pluralizer = new Pluralizer();

            _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringAddPropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreLastModifiedDuringAddPropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreDateAddedDuringUpdatePropertyConvention());
            _dataContext.PropertyConventions.Add(new IgnoreIdDuringAddPropertyConvention());

            return true;
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            VerifyInitialize();

            return _dataContext.GetAllAsync();
        }

        // TODO: Keep private until testable
        private Task<TEntity> GetSingleAsync(NadoMapperParameter parameter)
        {
            VerifyInitialize();
            return _dataContext.GetSingleAsync(parameter);
        }

        public Task<TEntity> GetSingleAsync(long id)
        {
            VerifyInitialize();
            return _dataContext.GetSingleAsync(id);
        }

        // TODO: Keep private until testable
        private Task<TEntity> GetSingleAsync(string procName, IEnumerable<NadoMapperParameter> parameters = null)
        {
            VerifyInitialize();
            return _dataContext.GetSingleAsync(procName, parameters);
        }

        // TODO: Keep private until testable
        private Task<TEntity> GetSingleAsync(string procName, NadoMapperParameter parameter)
        {
            VerifyInitialize();

            //var model =  TODO: How to handle mapping within tasks C#?

            return null; //_dataContext.MapSingle(_dataContext.ExecuteScalarAsync(procName,CRUDType.Read, parameter));
        }

        public Task<TEntity> AddAsync(TEntity item)
        {
            VerifyInitialize();

            return _dataContext.AddAsync(item);
        }

        public Task<TEntity> AddUpdateAsync(TEntity item)
        {
            VerifyInitialize();
            return null;
        }

        public Task<long> UpdateAsync(TEntity item)
        {
            VerifyInitialize();
            return _dataContext.UpdateAsync(item);
        }

        public Task<long> DeleteAsync(TEntity item)
        {
            VerifyInitialize();
            return _dataContext.DeleteAsync(item);
        } */
    }
}