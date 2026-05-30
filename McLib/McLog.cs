using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MediaCollection
{
	/// <summary>
	/// Static facade over <see cref="ILoggerFactory"/> used by library code that
	/// can't take an <see cref="ILogger"/> via DI (e.g. static helpers).
	///
	/// Wire it once at app startup:
	/// <code>
	/// McLog.Factory = app.Services.GetRequiredService&lt;ILoggerFactory&gt;();
	/// </code>
	///
	/// Until then, all loggers handed out are <see cref="NullLogger"/>s, so
	/// calling code is always safe.
	/// </summary>
	public static class McLog
	{
		private static ILoggerFactory s_factory = NullLoggerFactory.Instance;

		public static ILoggerFactory Factory
		{
			get => s_factory;
			set => s_factory = value ?? NullLoggerFactory.Instance;
		}

		public static ILogger<T> For<T>() => s_factory.CreateLogger<T>();

		public static ILogger Get(Type type) =>
			s_factory.CreateLogger(type?.FullName ?? "Default");

		public static ILogger Get(string categoryName) =>
			s_factory.CreateLogger(categoryName ?? "Default");
	}
}
