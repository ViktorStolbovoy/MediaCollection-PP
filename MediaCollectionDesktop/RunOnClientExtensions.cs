using System;
using System.Diagnostics;

namespace MediaCollection
{
	internal static class RunOnClientExtensions
	{
		public static void RunOnClient(this DeviceLocationResult locationResult)
		{
			if (locationResult == null) throw new ArgumentNullException(nameof(locationResult));
			if (locationResult.DeviceKind != DeviceType.Local)
				throw new InvalidOperationException("RunOnClient can be used only with Local devices.");

			string command = locationResult.ClientCommand();
			if (string.IsNullOrWhiteSpace(locationResult.DeviceData))
			{
				Process.Start(command);
			}
			else
			{
				Process.Start(locationResult.DeviceData, command);
			}
		}
	}
}
