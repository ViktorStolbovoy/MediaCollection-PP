using System;
using System.Security.Cryptography;

namespace MediaCollection.Auth
{
	/// <summary>
	/// PBKDF2-SHA256 password hashing. Stored separately as base64-encoded salt and hash
	/// plus an iteration count so values can be tuned over time.
	/// </summary>
	public static class PasswordHasher
	{
		public const int DefaultIterations = 100_000;
		public const int SaltBytes = 16;
		public const int HashBytes = 32;

		public sealed class HashResult
		{
			public string SaltBase64 { get; init; }
			public string HashBase64 { get; init; }
			public int Iterations { get; init; }
		}

		public static HashResult Hash(string password, int iterations = DefaultIterations)
		{
			if (string.IsNullOrEmpty(password))
				throw new ArgumentException("Password must not be empty.", nameof(password));

			var salt = RandomNumberGenerator.GetBytes(SaltBytes);
			var hash = Derive(password, salt, iterations);

			return new HashResult
			{
				SaltBase64 = Convert.ToBase64String(salt),
				HashBase64 = Convert.ToBase64String(hash),
				Iterations = iterations
			};
		}

		public static bool Verify(string password, string saltBase64, string hashBase64, int iterations)
		{
			if (string.IsNullOrEmpty(password)
				|| string.IsNullOrEmpty(saltBase64)
				|| string.IsNullOrEmpty(hashBase64)
				|| iterations <= 0)
			{
				return false;
			}

			byte[] salt;
			byte[] expected;
			try
			{
				salt = Convert.FromBase64String(saltBase64);
				expected = Convert.FromBase64String(hashBase64);
			}
			catch (FormatException)
			{
				return false;
			}

			if (expected.Length == 0) return false;

			var actual = Derive(password, salt, iterations, expected.Length);
			return CryptographicOperations.FixedTimeEquals(actual, expected);
		}

		private static byte[] Derive(string password, byte[] salt, int iterations, int outputBytes = HashBytes)
		{
			return Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, outputBytes);
		}
	}
}
