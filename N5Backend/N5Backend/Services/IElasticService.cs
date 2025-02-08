using Elastic.Clients.Elasticsearch;
using N5Backend.Data;

namespace N5Backend.Services;

public interface IElasticService
{
    Task CreateIndexIfNotExistsAsync(string indexName);
    Task<UpdateResponse<Permission>> AddOrUpdateAsync(Permission permission);
    Task<IndexResponse> AddAsync(Permission permission);
}