﻿using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.ViewModels;
using System;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Handles the view state of the UI, and is responsible for locking
	/// </summary>
	public interface ILockingComponent : IDisposable
	{
		// System query delegates
		Func<IntPtr> GetTargetHandle { get; set; }
		Func<string> GetTargetPath { get; set; }
		Action<ViewType> SetViewModel { get; set; }

		// Queries
		ViewType GetLockType();
		ViewType GetPreviousView();

		// Commands
		void SwitchView(ViewType viewType);
		void Unlock();
	}

	public class LockingComponent : ILockingComponent
	{
		// Fields
		private bool _isDisposed;
		private readonly IAppSystem _appSystem;
		private ViewType _currentView;
		private ViewType _previousView;
		private ViewType _lockType;

		// Constructor
		public LockingComponent(IAppSystem appSystem)
		{
			_appSystem = appSystem;
			_appSystem.LockStateChanged += AppSystem_LockStateChanged;
		}

		// Component interface
		public Func<IntPtr> GetTargetHandle { get; set; }
		public Func<string> GetTargetPath { get; set; }
		public Action<ViewType> SetViewModel { get; set; }

		public ViewType GetLockType() => _lockType;
		public ViewType GetPreviousView() => _previousView;

		public void SwitchView(ViewType viewType)
		{
			if (_currentView == ViewType.LockWindow)
			{
				if (viewType == ViewType.LockWindow)
				{
					// Lock is being toggled off 
					PerformViewSwitch(_previousView);
				}
				else
				{
					// Switch
					PerformViewSwitch(viewType);
				}

				// Unlock
				_appSystem.Unlock();
			}
			else
			{
				// Switch
				PerformViewSwitch(viewType);

				// Lock
				if (viewType == ViewType.LockWindow)
				{
					if (_lockType == ViewType.WindowList && GetTargetHandle != default) _appSystem.Lock(GetTargetHandle());
					else if (GetTargetPath != default) _appSystem.Lock(GetTargetPath());
				}
			}
		}

		public void Unlock()
		{
			_appSystem.Unlock();
		}

		private void PerformViewSwitch(ViewType targetViewType)
		{
			// Set view state
			_previousView = _currentView;
			_currentView = targetViewType;
			if (targetViewType == ViewType.LockWindow) _lockType = _previousView;

			// Update view
			SetViewModel?.Invoke(targetViewType);
		}

		// AppSystem events
		private void AppSystem_LockStateChanged(object sender, LockStateChangedEventArgs e)
		{
			if (e.IsLocked == false && _currentView == ViewType.LockWindow)
			{
				SwitchView(_previousView);
			}
		}
		
		// IDisposable
		~LockingComponent()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
				_appSystem.LockStateChanged -= AppSystem_LockStateChanged;
			}

			// Free any unmanaged objects here.

			// Done
			_isDisposed = true;
		}
	}
}
