// See https://aka.ms/new-console-template for more information

var manager = new ElasticsearchManager();
manager.CreateIndex();
manager.IndexSingleDocument();
manager.IndexMultipleDocuments();
await manager.SearchAllDocuments();
await manager.StructuredSearchWithDateRangeFilter();
await manager.UnstructuredSearchWithMatchQuery();
await manager.PaginationWithParameters(0, 10);
await manager.SearchWithSourceFiltering();
await manager.ComplexSearch();
manager.DeleteAllDocuments();