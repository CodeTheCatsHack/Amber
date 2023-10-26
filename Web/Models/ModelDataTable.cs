using CoreLibrary.Models.EFModels;

namespace Web.Models;

public class ModelDataTable<TModel>
{
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

    public List<TModel>? Models { get; set; }
    public int Skip { get; set; }
    public int Take { get; set; } = 10;

    public string? Search { get; set; }
}

public class ModelDataNewTable<TSat>
{
    public Map? Item1 { get; set; }

    public Dictionary<int, (List<Satellite>, List<Satellite>)> Item2 { get; set; } = new();
}