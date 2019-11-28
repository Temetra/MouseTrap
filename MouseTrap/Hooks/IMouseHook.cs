using MouseTrap.Data;
using System;

namespace MouseTrap.Hooks
{
	public interface IMouseHook : IDisposable
	{
		void StartHook(Dimensions region);
		void StopHook();
		void SetRegion(Dimensions region);
		void RestrictMouseToRegion();
		void UnrestrictMouse();
	}
}
