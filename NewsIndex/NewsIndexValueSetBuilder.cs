using Examine;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Examine;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexValueSetBuilder : ContentValueSetBuilder
{
    private readonly ILogger<NewsIndexValueSetBuilder> _logger;
    private readonly string[] _contentTypes = ["news"];

    public NewsIndexValueSetBuilder(PropertyEditorCollection propertyEditors, UrlSegmentProviderCollection urlSegmentProviders, IUserService userService, IShortStringHelper shortStringHelper,
        ICoreScopeProvider scopeProvider, ILocalizationService localizationService, IContentTypeService contentTypeService, ILogger<ContentValueSetBuilder> logger,
        ILogger<NewsIndexValueSetBuilder> logger2)
        : base(propertyEditors, urlSegmentProviders, userService, shortStringHelper, scopeProvider, true, localizationService, contentTypeService, logger)
    {
        _logger = logger2;
    }

    public override IEnumerable<ValueSet> GetValueSets(params IContent[] content)
    {
        _logger.LogDebug("--------------- Calling GetValueSets for content");

        var contents = content.Where(x => _contentTypes.Contains(x.ContentType.Alias) && x.Published).ToArray();

        return base.GetValueSets(contents);
    }

    /*
    public IEnumerable<ValueSet> GetValueSets(params IContent[] contents)
    {
        foreach (var content in contents.Where(x => _contentTypes.Contains(x.ContentType.Alias) && x.Published))
        {
            _logger.LogDebug("---------------- Indexing content item ID: {id}, name: {name}", content.Id, content.Name);

            var indexValues = new Dictionary<string, object>
            {
                //This is where we assign our fields defined in options
                [UmbracoExamineFieldNames.NodeNameFieldName] = content.Name!,
                ["id"] = content.Id,
                ["parentId"] = content.ParentId,
                ["nodeName"] = content.Name!,
                ["postedDate"] = content.CreateDate,
                ["postedDateTicks"] = content.CreateDate.Ticks
            };

            foreach (var property in content.Properties) indexValues[property.Alias] = property.GetValue();

            var valueSet = new ValueSet(content.Id.ToString(), IndexTypes.Content, content.ContentType.Alias, indexValues);

            yield return valueSet;
        }
    }
*/
}