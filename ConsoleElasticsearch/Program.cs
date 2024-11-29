// See https://aka.ms/new-console-template for more information


var manager = new ElasticClientsManager();
await manager.ExecuteSearch();

//manager.CreateIndex();
//manager.DeleteAllDocuments();
//manager.IndexSingleDocument();
//manager.IndexMultipleDocuments();
//await manager.DelaySeconds(2);
//await manager.SearchAllDocuments();
//await manager.StructuredSearchWithDateRangeFilter();
//await manager.UnstructuredSearchWithMatchQuery();
//await manager.PaginationWithParameters(0, 10);
//await manager.SearchWithSourceFiltering();
//await manager.ComplexSearch();
//await manager.AnalyzeText("employees", "english", "Good morninge");
//await manager.AnalyzeText("employees", "arabic", "ضباح الخير");
/*Analyzers Built-in
Standard
Whitespace
Keyword
Pattern
Language
Fingerprint
....,
.....,
....
*/