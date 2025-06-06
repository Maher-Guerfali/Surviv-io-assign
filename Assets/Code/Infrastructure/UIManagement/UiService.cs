using System.Collections.Generic;
using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public class UiService : IUiService
  {
    private readonly IUiFactory _uiFactory;
    private readonly IUIRootProvider _uiRootProvider;
    private readonly IWindowInfoService _windowInfoService;
    private readonly List<WindowBase> _hierarchy = new();

    public UiService(IUiFactory uiFactory, IUIRootProvider uiRootProvider, IWindowInfoService windowInfoService)
    {
      _uiFactory = uiFactory;
      _uiRootProvider = uiRootProvider;
      _windowInfoService = windowInfoService;
    }

    public T GetWindow<T>() where T : WindowBase
    {
      for (int i = _hierarchy.Count - 1; i >= 0; i--)
      {
        WindowBase window = _hierarchy[i];
        
        if (window is T t)
          return t;
      }

      return null;
    }

    public T OpenWindow<T>() where T : WindowBase
    {
      var openedWindow = GetWindow<T>();

      if (openedWindow != null)
        return openedWindow;
      
      var window = _uiFactory.CreateWindow<T>(_uiRootProvider.UiRoot);
      window.Initialize();
      PutInHierarchy(window);
      return window;
    }

    public void CloseWindow<T>() where T : WindowBase
    {
      if (_hierarchy.Count == 0)
        return;
      
      int i = _hierarchy.Count - 1;
      
      for (; i >= 0; i--)
      {
        if (_hierarchy[i] is T window)
        {
          _hierarchy.Remove(window);
          Object.Destroy(window.gameObject);
          break;
        }
      }

      for (; i < _hierarchy.Count; i++)
      {
        WindowBase window = _hierarchy[i];
        _hierarchy.Remove(window);
        Object.Destroy(window.gameObject);
      }
    }
    
    public void CloseWindow(WindowBase windowBase)
    {
      if (_hierarchy.Count == 0)
        return;
      
      int i = _hierarchy.Count - 1;
      
      for (; i >= 0; i--)
      {
        if (_hierarchy[i] == windowBase)
        {
          _hierarchy.Remove(windowBase);
          Object.Destroy(windowBase.gameObject);
          break;
        }
      }

      for (; i < _hierarchy.Count; i++)
      {
        WindowBase window = _hierarchy[i];
        _hierarchy.Remove(window);
        Object.Destroy(window.gameObject);
      }
    }

    public void Back()
    {
      if (_hierarchy.Count > 0)
      {
        WindowBase window = _hierarchy[^1];

        if (window.IsUserCanClose)
        {
          CloseWindow(window);
        }
      }
    }

    private void PutInHierarchy<T>(T window) where T : WindowBase
    {
      window.transform.SetSiblingIndex(_windowInfoService.GetWindowOrder<T>());
      _hierarchy.Add(window);
    }
  }
}