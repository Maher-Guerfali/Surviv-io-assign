using System;
using UnityEngine;

namespace Code.Infrastructure.UIManagement
{
	public abstract class SubWindowBase : MonoBehaviour, IDisposable
	{
		public event Action OnOpen;
		public event Action OnClose;

		public bool IsOpen { get; private set; }
		protected virtual bool AutoOpenClose => true;

		public WindowBase OwnerWindow { get; private set; }

		private void OnEnable()
		{
			OwnerWindow = GetComponentInParent<WindowBase>();

			if (AutoOpenClose)
			{
				Open();
			}
		}

		private void OnDisable()
		{
			if (AutoOpenClose)
			{
				Close();
			}
		}

		public void Open()
		{
			if (IsOpen)
				return;
      
			IsOpen = true;
			gameObject.SetActive(true);
			ProcessOpen();
			OnOpen?.Invoke();
		}

		public void Close()
		{
			if (!IsOpen)
				return;
      
			IsOpen = false;
			gameObject.SetActive(false);
			ProcessClose();
			OnClose?.Invoke();
		}

		public virtual void Dispose() { }
		protected virtual void ProcessOpen() { }
		protected virtual void ProcessClose() { }

		private void OnDestroy() => Dispose();
	}
}