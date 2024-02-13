// See https://aka.ms/new-console-template for more information

var manager = new ElasticsearchManager();
manager.CreateIndex();
manager.DeleteAllDocuments();
manager.IndexSingleDocument();
manager.IndexMultipleDocuments();
await manager.DelaySeconds(3);
await manager.SearchAllDocuments();
await manager.StructuredSearchWithDateRangeFilter();
await manager.UnstructuredSearchWithMatchQuery();
await manager.PaginationWithParameters(0, 10);
await manager.SearchWithSourceFiltering();
await manager.ComplexSearch();
