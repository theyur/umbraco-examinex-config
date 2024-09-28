using Examine;
using ExamineX.Shared;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Services.Changes;
using Umbraco.Cms.Core.Sync;
using Umbraco.Cms.Infrastructure;
using Umbraco.Cms.Infrastructure.Search;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexingNotificationHandler : INotificationHandler<ContentCacheRefresherNotification>
{
    private readonly IRuntimeState _runtimeState;
    private readonly IUmbracoIndexingHandler _umbracoIndexingHandler;
    private readonly ExamineXManager _examineManager;
    private readonly IContentService _contentService;
    private readonly NewsIndexValueSetBuilder _newsIndexValueSetBuilder;

    public NewsIndexingNotificationHandler(IRuntimeState runtimeState,
        IUmbracoIndexingHandler umbracoIndexingHandler,
        ExamineXManager examineManager,
        IContentService contentService,
        NewsIndexValueSetBuilder newsIndexValueSetBuilder)
    {
        _runtimeState = runtimeState;
        _umbracoIndexingHandler = umbracoIndexingHandler;
        _examineManager = examineManager;
        _contentService = contentService;
        _newsIndexValueSetBuilder = newsIndexValueSetBuilder;
    }

    public void Handle(ContentCacheRefresherNotification notification)
    {
        //Don't waste resources trying to do the rest if we can't
        if (NotificationHandlingIsDisabled()) return;

        if (!_examineManager.TryGetIndex("NewsIndex", out IIndex? index))
        {
            throw new InvalidOperationException("Could not obtain the news index");
        }

        ContentCacheRefresher.JsonPayload[] payloads = GetNotificationPayloads(notification);

        foreach (ContentCacheRefresher.JsonPayload payload in payloads)
        {
            // Remove from index if removed from tree
            if (payload.ChangeTypes.HasType(TreeChangeTypes.Remove))
            {
                index.DeleteFromIndex(payload.Id.ToString());
            }
            // Reindex on node/branch refresh
            else if (payload.ChangeTypes.HasType(TreeChangeTypes.RefreshNode) ||
                     payload.ChangeTypes.HasType(TreeChangeTypes.RefreshBranch))
            {
                IContent? content = _contentService.GetById(payload.Id);
                if (content == null || content.Trashed)
                {
                    index.DeleteFromIndex(payload.Id.ToString());
                    continue;
                }

                IEnumerable<ValueSet> valueSets = _newsIndexValueSetBuilder.GetValueSets(content);
                index.IndexItems(valueSets);
            }
        }
    }

    private bool NotificationHandlingIsDisabled()
    {
        // Only handle events when the site is running.
        if (_runtimeState.Level != RuntimeLevel.Run)
        {
            return true;
        }

        if (_umbracoIndexingHandler.Enabled == false)
        {
            return true;
        }

        if (Suspendable.ExamineEvents.CanIndex == false)
        {
            return true;
        }

        return false;
    }

    private ContentCacheRefresher.JsonPayload[] GetNotificationPayloads(CacheRefresherNotification notification)
    {
        //Ignore unsupported payloads
        if (notification.MessageType != MessageType.RefreshByPayload ||
            notification.MessageObject is not ContentCacheRefresher.JsonPayload[] payloads)
        {
            throw new NotSupportedException();
        }

        return payloads;
    }
}