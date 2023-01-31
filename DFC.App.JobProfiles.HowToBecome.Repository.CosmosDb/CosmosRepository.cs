using DFC.App.JobProfiles.HowToBecome.Data.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.JobProfiles.HowToBecome.Repository.CosmosDb
{
    [ExcludeFromCodeCoverage]
    public class CosmosRepository<T> : ICosmosRepository<T>
        where T : IDataModel
    {
        private readonly CosmosDbConnection cosmosDbConnection;
        private readonly CosmosClient cosmosClient;
        private readonly Container container;

        public CosmosRepository(CosmosDbConnection cosmosDbConnection, CosmosClient cosmosClient, IHostingEnvironment env)
        {
            this.cosmosDbConnection = cosmosDbConnection;
            this.cosmosClient = cosmosClient;

            if (env.IsDevelopment())
            {
                CreateDatabaseIfNotExistsAsync().GetAwaiter().GetResult();
                CreateCollectionIfNotExistsAsync().GetAwaiter().GetResult();
            }

            container = cosmosClient.GetContainer(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);
        }

        public async Task<bool> PingAsync()
        {
            FeedIterator<T> resultSet = container.GetItemQueryIterator<T>(
                queryDefinition: null,
                requestOptions: new QueryRequestOptions()
                {
                    //unknown partition key need to double check
                    MaxItemCount = 1,
                });

            if (resultSet == null)
            {
                return false;
            }

            var models = await resultSet.ReadNextAsync().ConfigureAwait(false);
            return models.Any();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var models = new List<T>();

            using (FeedIterator<T> resultSet = container.GetItemQueryIterator<T>(
                queryDefinition: null,
                requestOptions: new QueryRequestOptions()
                {
                    //unknown partition key need to double check
                }))
            {
                while (resultSet.HasMoreResults)
                {
                    FeedResponse<T> result = await resultSet.ReadNextAsync().ConfigureAwait(false);
                    models.AddRange(result);
                }
            }

            return models.Any() ? models : null;
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> where)
        {
            FeedIterator<T> resultSet = container.GetItemQueryIterator<T>(
                queryDefinition: null, // probably needs query defined
                requestOptions: new QueryRequestOptions()
                {
                    //unknown partition key need to double check
                    MaxItemCount = 1,
                });

/*            var query = cosmosClient.CreateDocumentQuery<T>(DocumentCollectionUri, new QueryRequestOptions { MaxItemCount = 1 })
                                      .Where(where)          <-----
                                      .AsDocumentQuery();*/

            if (resultSet == null)
            {
                return default(T);
            }

            var models = await resultSet.ReadNextAsync().ConfigureAwait(false);

            if (models != null && models.Count() > 0)
            {
                return models.FirstOrDefault();
            }

            return default(T);
        }

        public async Task<HttpStatusCode> UpsertAsync(T model)
        {
            ItemResponse<T> response = await container.UpsertItemAsync<T>(
                item: model,
                partitionKey: new PartitionKey(model.PartitionKey),
                requestOptions: new ItemRequestOptions
                {
                    IfMatchEtag = model.Etag,
                }).ConfigureAwait(false);

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> DeleteAsync(Guid documentId)
        {
            var existingDocument = await GetAsync(f => f.DocumentId == documentId).ConfigureAwait(false);

            if (existingDocument == null)
            {
                return HttpStatusCode.NotFound;
            }

            ItemResponse<T> response = await container.DeleteItemAsync<T>(
                id: documentId.ToString(),
                partitionKey: new PartitionKey(existingDocument.PartitionKey),
                new ItemRequestOptions
                {
                    IfMatchEtag = existingDocument.Etag,
                }).ConfigureAwait(false);

            return response.StatusCode;
        }

        private async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                cosmosClient.GetDatabase(cosmosDbConnection.DatabaseId);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    await cosmosClient.CreateDatabaseAsync(cosmosDbConnection.DatabaseId).ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                cosmosClient.GetContainer(cosmosDbConnection.DatabaseId, cosmosDbConnection.CollectionId);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    var pkDef = cosmosDbConnection.PartitionKey;
                    Database database = cosmosClient.GetDatabase(cosmosDbConnection.DatabaseId);
                    await database.CreateContainerIfNotExistsAsync(
                        id: cosmosDbConnection.CollectionId,
                        partitionKeyPath: pkDef,
                        throughput: 1000)
                        .ConfigureAwait(false);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}