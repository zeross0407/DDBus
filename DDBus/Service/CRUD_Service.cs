using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using DDBus.Entity;
using DDBus.Models;
using System.Reflection;


namespace DDBus.Services
{
    public interface IMongoRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }



    public  class CRUD_Service<T>
    {
        private  readonly IMongoCollection<T> _collection;
        public CRUD_Service(IOptions<DatabaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);

            string name = GetCollectionName();

            _collection = mongoDatabase.GetCollection<T>(name);
        }


        private string GetCollectionName()
        {
            var attribute = typeof(T).GetCustomAttribute<CollectionNameAttribute>();
            return attribute?.Name ?? typeof(T).Name;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }


        public async Task<T> Get_One_Item_Async(string value, string attribute)
        {
            return await _collection.Find(Builders<T>.Filter.Eq(attribute, value)).FirstOrDefaultAsync();
        }



        public async Task<string> AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
            var propertyInfo = entity.GetType().GetProperty("Id");
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(entity)?.ToString();
            }
            return null;

        }

        public async Task<bool> ExistsAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }


        public async Task UpdateAsync(string id, T entity)
        {
            await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)), entity);
        }




        public async Task Update_by_UUID_Async(string uuid, T entity)
        {
            var existingEntity = await GetBy_UUID_Async(uuid);

            if (existingEntity != null)
            {
                await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("_id", uuid), entity);
            }
            else
            {
                throw new Exception("Entity not found for the provided UUID.");
            }
        }

        public async Task<T> GetBy_UUID_Async(string uuid)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("_id", uuid)).FirstOrDefaultAsync();
        }


        public async Task<List<T>> FindAllAsync(string value, string attribute)
        {
            var filter = Builders<T>.Filter.Regex(attribute, new MongoDB.Bson.BsonRegularExpression(value, "i"));
            return await _collection.Find(filter).ToListAsync();
        }





        public async Task<List<T>> GetBy_UserID_Async(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("userId", new ObjectId(id))).ToListAsync();
        }


        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("_id", new ObjectId(id)));
        }



        public async Task DeleteByUserIdAsync(string userId)
        {
            var filter = Builders<T>.Filter.Eq("userId", userId); 
            await _collection.DeleteManyAsync(filter);
        }



    }
}
