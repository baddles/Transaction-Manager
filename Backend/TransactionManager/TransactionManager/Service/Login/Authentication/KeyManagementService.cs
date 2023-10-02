using System.Collections.Concurrent;
namespace TransactionManager.Service.Login.Authentication
{
	public static class KeyManagementService
	{
		private static readonly ConcurrentDictionary<string, byte[]> keyStates = new ConcurrentDictionary<string, byte[]>();
		private static DateTime lastClearCacheTime = DateTime.UtcNow;
		public static byte[] initializeInstance(string state)
		{
			RSAService rsa = new RSAService();
			addNewKeyState(state, rsa.ExportPrivateKey());
			return rsa.ExportPrivateKey();
		}
		private static void addNewKeyState(string key, byte[] privateKey)
		{
			string actualKeyValue = key + "&$TIMEOUT_AT="+ DateTime.UtcNow.AddMinutes(10).ToString();
			keyStates.AddOrUpdate(actualKeyValue, privateKey, (k, existingValue) => privateKey);
		}
		public static byte[] getPrivateKey(string key)
		{
			IEnumerable<string> findKey = keyStates.Keys.Where(k => k.StartsWith(key + "&$TIMEOUT_AT="));

			if (findKey.Count() >= 1)
			{
				keyStates.TryGetValue(findKey.FirstOrDefault(), out byte[] priKey);
				bool isSame = true;
				if (findKey.Count() != 1)
				{
					foreach (string keys in findKey)
					{
						keyStates.TryGetValue(keys, out byte[] secondKey);
						if (!ByteArraysAreEqual(priKey, secondKey))
						{
                            throw new IndexOutOfRangeException("One state shouldn't have two different private keys at the same time");
                        }
                    }
                }
				if (priKey != null)
				{
					return priKey;
				}
            }
			throw new InvalidDataException("Can't find private key with given state");
		}
        private static bool ByteArraysAreEqual(byte[] array1, byte[] array2)
        {
            if (array1 == null && array2 == null)
            {
                return true;
            }
            if (array1 == null || array2 == null)
            {
                return false;
            }
            if (array1.Length != array2.Length)
            {
                return false;
            }
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
        private static void removeExpiredKey(DateTime currentTime)
		{
			int keyExpireRemoved = 0;
			foreach (var key in keyStates)
			{
                string[] parts = key.Key.Split(new[] { "&$TIMEOUT_AT=" }, StringSplitOptions.None);
				if (parts.Length >= 2 && DateTime.TryParse(parts[1], out DateTime parsedDateTime))
				{
					if (parsedDateTime < currentTime)
					{
						Remove(key.Key, true);
					}
				}
				else
				{
					if (parts.Length < 2)
					{
                        throw new InvalidOperationException("Delimeter '&$TIMEOUT_AT=' not found in the string: " + key.Key + ".\nNumber of key managed to removed: " + keyExpireRemoved);
                    }
					else
					{
						throw new InvalidOperationException("Failed to parse DateTime");
					}
                }
            }
        }

		private static bool Remove(string key, bool keyWithTimestamp)
		{
			byte[] removedValue;
			if (keyWithTimestamp == true)
			{
                return keyStates.TryRemove(key, out removedValue);
            }
            else
			{
				IEnumerable<string> matchingKeys = keyStates.Keys.Where(key => key.StartsWith(key + "&$TIMEOUT_AT="));
				foreach (string keyIteration in matchingKeys)
				{
					bool removed = keyStates.TryRemove(keyIteration, out _);
					if (removed == false)
					{
						return false;
					}
				}
				return true;

            }
        }
		public static void RemoveKey(string key)
		{
			Remove(key, false);
			DateTime currentTime = DateTime.UtcNow;
			if (currentTime - lastClearCacheTime > TimeSpan.FromMinutes(10))
			{
				removeExpiredKey(currentTime);
                lastClearCacheTime = currentTime;
            }
        }
	}
}

