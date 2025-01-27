﻿using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext context) => this.catalogContext = context;

        public async Task CreateProduct(Product product)
            => await catalogContext.Products.InsertOneAsync(product);

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            DeleteResult deleteResult = await catalogContext.Products.DeleteOneAsync(filter);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Product> GetProduct(string id) 
            => await catalogContext.Products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<Product>> GetProducts() 
            => await catalogContext.Products.Find(p => true).ToListAsync();

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await catalogContext.Products.ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}