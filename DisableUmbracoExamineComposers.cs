using Examine;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Examine;
using Umbraco.Cms.Infrastructure.Persistence;

namespace ExamineXTest01;

public class DisableUmbracoExamineComposers(
    ILoggerFactory loggerFactory,
    IContentService contentService,
    IUmbracoDatabaseFactory umbracoDatabaseFactory,
    IContentValueSetBuilder contentValueSetBuilder)
    : ContentIndexPopulator(loggerFactory.CreateLogger<ContentIndexPopulator>(), contentService, umbracoDatabaseFactory, contentValueSetBuilder)
{
    private readonly bool _isMaster = true;

    public override bool IsRegistered(IIndex index) => _isMaster;

    protected override void PopulateIndexes(IReadOnlyList<IIndex> indexes)
    {
        if (!_isMaster) return;

        base.PopulateIndexes(indexes);
    }
}