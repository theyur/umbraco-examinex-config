using Examine;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Cms.Infrastructure.Persistence;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexPopulator : ContentIndexPopulator
{
    private readonly IContentService _contentService;
    private readonly NewsIndexValueSetBuilder _newsIndexValueSetBuilder;
    private readonly IExamineManager _examineManager;

    public NewsIndexPopulator(ILogger<ContentIndexPopulator> logger, IContentService contentService, IUmbracoDatabaseFactory umbracoDatabaseFactory,
        NewsIndexValueSetBuilder newsIndexValueSetBuilder, IExamineManager examineManager)
        : base(logger, contentService, umbracoDatabaseFactory, newsIndexValueSetBuilder)
    {
        _contentService = contentService;
        _newsIndexValueSetBuilder = newsIndexValueSetBuilder;
        _examineManager = examineManager;

        RegisterIndex("NewsIndex");
    }

    public bool IsRegistered(IIndex index) => throw new NotImplementedException();
    //public void Populate(params IIndex[] indexes)

    protected override void PopulateIndexes(IReadOnlyList<IIndex> indexes)
    {
        _examineManager.TryGetIndex("NewsIndex", out var newsIndex);
        //newsIndex.DeleteFromIndex("*");

        foreach (IIndex index in indexes)
        {
            //Go over all the content and index it if it fits the criteria
            IContent[] roots = _contentService.GetRootContent().ToArray();
            var sets = _newsIndexValueSetBuilder.GetValueSets(roots);
            index.IndexItems(sets);

            foreach (var root in roots)
            {
                const int pageSize = 10000;
                var pageIndex = 0;
                IContent[] descendants;
                //Weird syntax, but it follows the Umbraco docs
                do
                {
                    descendants = _contentService.GetPagedDescendants(root.Id, pageIndex, pageSize, out _).ToArray();
                    IEnumerable<ValueSet> valueSets = _newsIndexValueSetBuilder.GetValueSets(descendants);
                    index.IndexItems(valueSets);

                    pageIndex++;
                }
                while (descendants.Length == pageSize);
            }
        }
    }
}