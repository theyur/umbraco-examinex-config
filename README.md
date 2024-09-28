# umbraco-examinex-config
VS solution for Umbraco 13 website with basic config of ExamineX Azure Search index

This small repo is result of my efforts to have correct configuration for ***Azure Search*** index utilized by **Umbraco CMS** search functionality with *ExamineX* nuget package.

Although there is documentation on index customization in [Umbraco documentation](https://docs.umbraco.com/umbraco-cms/v/13.latest-lts/reference/searching/examine/indexing) and on [ExamineX website](https://examinex.online/), it was not that easy to mix and match the things and configure custom index that can be used by Umbraco CMS website.

This Umbraco website installation contains definition of 2 document types: *Content* and *News*. Both of them are being indexed by *External* and *Internal* indexes, which is by default; also *News* doctype is configured to be indexed by *NewsIndex*, which is maintained by ***Azure Search service*** with *ExamineX*.

Since *ExamineX* package requires license and allows only 3 indexes in Azure Search, the indexes still show red cross in *Settings/Examine* section of Umbraco backend.
