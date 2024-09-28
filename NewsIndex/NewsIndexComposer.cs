using ExamineX.AzureSearch.Umbraco;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Infrastructure.Examine;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.Services.AddExamineXAzureSearchIndex<NewsIndexCreator>("NewsIndex");
        builder.Services.ConfigureOptions<NewsIndexOptions>();
        builder.Services.AddSingleton<NewsIndexValueSetBuilder>();
        builder.Services.AddSingleton<IIndexPopulator, NewsIndexPopulator>();
        builder.AddNotificationHandler<ContentCacheRefresherNotification, NewsIndexingNotificationHandler>();

        //builder.Services.AddTransient<INewsIndexValueSetBuilder, NewsIndexValueSetBuilder>();
    }
}