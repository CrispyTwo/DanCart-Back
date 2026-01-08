using Azure;
using Azure.AI.OpenAI;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using DanCart.DataAccess.Repository.IRepository;
using DanCart.WebApi.Areas.Products.Services.IServices;
using FluentResults;

namespace DanCart.WebApi.Areas.Products.Services;

public class ProductsVectorSearchService : IProductsVectorSearchService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SearchClient _searchClient;
    private readonly AzureOpenAIClient _openAiClient;
    private const string VectorIndexName = "products";
    private const string EmbeddingModelName = "text-embedding-ada-002";

    public record SearchModel(string ProductId, string Name, string Description, float[]? Vector = null);
    public ProductsVectorSearchService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;

        var searchEndpoint = new Uri(configuration["AzureVectorSearch:Endpoint"]!);
        var searchKey = configuration["AzureVectorSearch:ApiKey"]!;
        _searchClient = new SearchClient(searchEndpoint, VectorIndexName, new AzureKeyCredential(searchKey));

        var openAiEndpoint = new Uri(configuration["AzureOpenAI:Endpoint"]!);
        var openAiKey = configuration["AzureOpenAI:ApiKey"]!;
        _openAiClient = new AzureOpenAIClient(openAiEndpoint, new AzureKeyCredential(openAiKey));
    }

    public async Task<Result> MergeOrCreateAsync(Guid productId)
    {
        var product = await _unitOfWork.Product.GetAsync(x => x.Id == productId);
        var embeddingClient = _openAiClient.GetEmbeddingClient(EmbeddingModelName);
        var response = await embeddingClient.GenerateEmbeddingAsync(product!.Description);
        float[] vector = response.Value.ToFloats().ToArray();

        var searchDoc = new SearchModel(product.Id.ToString(), product.Name, product.Description, vector);
        await _searchClient.MergeOrUploadDocumentsAsync([searchDoc]);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid productId)
    {
        await _searchClient.DeleteDocumentsAsync("ProductId", [productId.ToString()]);
        return Result.Ok();
    }

    public async Task<Result<IEnumerable<SearchModel>>> GetRankingAsync(string searchText)
    {
        var searchOptions = new SearchOptions
        {
            VectorSearch = new()
            {
                Queries = { new VectorizableTextQuery(searchText) { Fields = { "Vector" } } }
            },
            Size = 15
        };

        var result = await _searchClient.SearchAsync<SearchModel>(null, searchOptions);
        var docs = result.Value.GetResults().Select(x => x.Document);

        return Result.Ok(docs);
    }
}