using Azure.Search.Documents.Indexes.Models;
using Examine;
using ExamineX.AzureSearch;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Infrastructure.Examine;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexOptions(IUmbracoIndexConfig umbracoIndexConfig) : IConfigureNamedOptions<AzureSearchIndexOptions>
{
    public void Configure(string? name, AzureSearchIndexOptions options)
    {
        if (name != "NewsIndex") return;

        //Name our fields and give them types
        /*
        options.FieldDefinitions = new(
            new("id", AzureSearchFieldDefinitionTypes.Integer),
            new("parentId", AzureSearchFieldDefinitionTypes.Integer),
            new("nodeName", AzureSearchFieldDefinitionTypes.FullText),

            new ("postedDateTicks", FieldDefinitionTypes.Long),
            new ("postedDate", FieldDefinitionTypes.DateTime)
        );
        */

        options.FieldDefinitions = new UmbracoFieldDefinitionCollection();

        options.FieldDefinitions.TryAdd(new("postedDateTicks", FieldDefinitionTypes.Long));
        options.FieldDefinitions.TryAdd(new ("postedDate", FieldDefinitionTypes.DateTime));

        options.AnalyzerName = LexicalAnalyzerName.EnMicrosoft.ToString();
        //options.Validator = umbracoIndexConfig.GetPublishedContentValueSetValidator();
        options.Validator = new ContentValueSetValidator(true, null, ["news"]);
    }

    public void Configure(AzureSearchIndexOptions options) => throw new NotImplementedException();
}

public class NewsIndexContentValueSetValidator : ContentValueSetValidator
{
    public NewsIndexContentValueSetValidator(bool publishedValuesOnly, int? parentId = null, IEnumerable<string>? includeItemTypes = null, IEnumerable<string>? excludeItemTypes = null)
        : base(publishedValuesOnly, parentId, includeItemTypes, excludeItemTypes) { }
}