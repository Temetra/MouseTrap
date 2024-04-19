using MouseTrap.Models;

namespace MouseTrap.Pages;

internal interface IViewModelPage
{
    public void Load(IViewModel model);
    public void Unload();
}
