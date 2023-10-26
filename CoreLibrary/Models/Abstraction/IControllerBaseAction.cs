using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers.Abstraction;

public interface IControllerBaseAction<in TModel>
{
    public IActionResult Index()
    {
        throw new NotImplementedException();
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
    }

    public IActionResult Edit()
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Create(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Delete()
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Delete(TModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> Edit(TModel model)
    {
        throw new NotImplementedException();
    }
}