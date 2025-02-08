using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.Options;
using N5Backend.Data;

namespace N5Backend.Services;

public class ElasticService : IElasticService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticSearchConfig _config;

    public ElasticService(IOptions<ElasticSearchConfig> config)
    {
        _config = config.Value;
        var settings = new ElasticsearchClientSettings(new Uri(_config.Uri))
            .DefaultIndex(_config.DefaultIndex)
            .Authentication(new BasicAuthentication(_config.Username, _config.Password));
        _client = new ElasticsearchClient(settings);

        CreateIndexIfNotExistsAsync(_config.DefaultIndex).Wait();
    }

    public async Task CreateIndexIfNotExistsAsync(string indexName)
    {
        if (!(await _client.Indices.ExistsAsync(indexName)).Exists)
        {
            await _client.Indices.CreateAsync(indexName);
        }
    }

    public Task<UpdateResponse<Permission>> AddOrUpdateAsync(Permission permission)
    {
        return _client.UpdateAsync<Permission, Permission>(_config.DefaultIndex, permission.Id, u => u.Doc(permission));
    }

    public Task<IndexResponse> AddAsync(Permission permission)
    {
        return _client.IndexAsync(permission, config => config.Index(_config.DefaultIndex));
    }
}