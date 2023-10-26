namespace Web.Models;

public class ModelDataTable<TModel>
{
    public List<TModel>? Models { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;

    public string? Search { get; set; }

    public ModelDataTable()
    {
    }

    public ModelDataTable(List<TModel> models, int skip, int take)
    {
        Models = models;
        Skip = skip;
        Take = take;
    }

    public ModelDataTable(List<TModel> models)
    {
        Models = models;
    }
}