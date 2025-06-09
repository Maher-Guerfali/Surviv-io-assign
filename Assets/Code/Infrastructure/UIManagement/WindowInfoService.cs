using System;
using System.Collections.Generic;
using Code.UI;

namespace Code.Infrastructure.UIManagement
{
	public class WindowInfoService : IWindowInfoService
	{
		private readonly Dictionary<Type, string> _addresses = new()
		{
			{typeof(HudWindow), "UI/HudWindow"}
		};

		private readonly Dictionary<Type, int> _orders = new()
		{
			{typeof(HudWindow), 0}
		};

		public int GetWindowOrder<T>() where T : WindowBase => _orders[typeof(T)];
    
		public string GetWindowAddress<T>() where T : WindowBase
		{
			if (!_addresses.ContainsKey(typeof(T)))
				throw new NotSupportedException($"Window address for {typeof(T)} is not defined");
      
			return _addresses[typeof(T)];
		}
	}
}