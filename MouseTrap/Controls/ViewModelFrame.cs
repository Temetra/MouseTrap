using Microsoft.UI.Xaml.Controls;
using MouseTrap.Models;
using MouseTrap.Pages;
using System;
using System.Collections.Generic;

namespace MouseTrap.Controls;

// Pages can't use constructor injection
// Using a custom Frame control to Load and Unload viewmodels on navigation
// This allows event handlers to be unsubscribed etc

internal class ViewModelFrame : Frame
{
    private readonly Dictionary<Type, Func<IViewModel>> factories = [];

    public ViewModelFrame()
    {
        Navigated += ViewModelFrame_Navigated;
        Navigating += ViewModelFrame_Navigating;
    }

    public void AddViewModelFactory(Type pageType, Func<IViewModel> factory)
    {
        factories.Add(pageType, factory);
    }

    private IViewModel GetViewModel(Type pageType)
    {
        if (factories.TryGetValue(pageType, out Func<IViewModel> CreateViewModel))
        {
            return CreateViewModel();
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private void ViewModelFrame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
    {
        if (e.Content is IViewModelPage page)
        {
            var viewmodel = GetViewModel(e.SourcePageType);
            page.Load(viewmodel);
        }
    }

    private void ViewModelFrame_Navigating(object sender, Microsoft.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
    {
        if (sender is Frame frame)
        {
            if (frame.Content is IViewModelPage page)
            {
                page.Unload();
            }
        }
    }
}
