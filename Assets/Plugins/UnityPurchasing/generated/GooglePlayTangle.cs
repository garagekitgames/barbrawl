#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("rywiLR2vLCcvrywsLb4rvqneERl18ffbna+Aqq+i6qvz5curSYoZn/K656KaZKQC2yNnuxAC4UZoLbiolrnKA7V8MniD5kFVN3snKvb2UJFMkNcv9Wqw5lIDey/g7CD6SNdYbWBrIfF8+ZvNEHEm75S/6DkC73dF9awrOro4coLAr1D7HmlCThD/AUVSECjnK7/d7vJzNQY4PSjXCz7doLj+WiHmOKsVUOcvMS0921sf+r8rHa8sDx0gKyQHq2Wr2iAsLCwoLS7Z/k2gexHoMV2ef8UinMp8CbaMv2WTBYk/GldalnT7r6dpJoFmn5EdH54bQVQOzbIjCQHpq4K7Y0SAcder6UBnBPP9DSJ2AVKS2wKCi6PZUS4bn+pz9M3Mdi8uLC0s");
        private static int[] order = new int[] { 1,6,4,9,9,10,8,9,13,13,10,13,13,13,14 };
        private static int key = 45;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
