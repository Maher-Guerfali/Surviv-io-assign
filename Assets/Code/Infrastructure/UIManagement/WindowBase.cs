using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Infrastructure.UIManagement
{
	public abstract class WindowBase : MonoBehaviour, IDisposable
	{
		[SerializeField] private Button _closeButton;
    
		private SubWindowBase[] _subWindows;
		private IUiService _uiService;

		public abstract bool IsUserCanClose { get; }

		[Inject]
		private void Construct(IUiService uiService)
		{
			_uiService = uiService;
		}

		public void Initialize()
		{
			_subWindows = GetComponentsInChildren<SubWindowBase>();
			OnInitialize();
		}

		private void OnEnable()
		{
			OnOpen();
			OpenSubWindows();
			_closeButton?.onClick.AddListener(CloseWindow);
		}

		private void OnDisable()
		{
			OnClose();
			CloseSubWindows();
			_closeButton?.onClick.RemoveListener(CloseWindow);
		}

		private void Update()
		{
			OnUpdate();
		}

		protected void CloseWindow() => _uiService.CloseWindow(this);

		public virtual void Dispose() { }
		protected virtual void OnInitialize() { }
		protected virtual void OnOpen() { }
		protected virtual void OnClose() { }
		protected virtual void OnUpdate() { }

		private void OpenSubWindows()
		{
			for (int i = 0; i < _subWindows?.Length; i++)
			{
				_subWindows[i].Open();
			}
		}

		private void CloseSubWindows()
		{
			for (int i = 0; i < _subWindows?.Length; i++)
			{
				_subWindows[i].Close();
			}
		}

		private void OnDestroy() => Dispose();
	}
}