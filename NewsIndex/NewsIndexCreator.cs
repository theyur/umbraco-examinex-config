using ExamineX.AzureSearch;
using ExamineX.AzureSearch.Umbraco;
using ExamineX.Shared;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Sync;

namespace ExamineXTest01.NewsIndex;

public class NewsIndexCreator(
    ILoggerFactory loggerFactory,
    string name,
    ILicenseManager licenseManager,
    IOptions<ExamineXConfig> config,
    IOptionsMonitor<AzureSearchIndexOptions> indexOptions,
    IOptions<AzureSearchOptions> azureSearchOptions,
    IServerRoleAccessor serverRoleAccessor)
    : UmbracoAzureSearchContentIndex(loggerFactory, name, new LicenseManagerCollection([licenseManager]), config, indexOptions, azureSearchOptions, serverRoleAccessor);
    //: UmbracoAzureSearchIndex(loggerFactory, name,  new LicenseManagerCollection([licenseManager]), config, indexOptions, azureSearchOptions, serverRoleAccessor);
    //: AzureSearchIndex(loggerFactory, name, licenseManager, config, indexOptions, azureSearchOptions);