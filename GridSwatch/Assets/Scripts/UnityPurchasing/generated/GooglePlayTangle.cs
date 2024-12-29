// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("Xr2gvmfZtRf/JBSAzpcvCiWaZKQ2hAckNgsADyyAToDxCwcHBwMGBY5lL5+7t9qqYX5bsp3TvQNpgqhwOYa81GluNasb8YgdJvH+3brOPuR8kiQNXpoAR5vpnW0C1JllzpY4VE2kux208WWt1amIMzmyWc9dXQR50OKVo8++szo3vQnqNhr9IIjOEqcMmcp1rH0i/Tl1CMj9JKmFEbU/vB7dAWyn0GZ+CaNFFV8h7FaqMluqwEHFtzj6YxtoBpSXP1d4oXbv4UqymrlloI9AkDQTQs8kn6iANBZSvqGlqZbZm5+RYMzdeY3BG4nsbRhHhBusDyQr43Jx2T1dQ1en6ynEj3eEBwkGNoQHDASEBwcGvLKi4I/CHTpRPCEe0JfzlwQFBwYH");
        private static int[] order = new int[] { 8,8,5,12,4,11,10,10,13,11,12,11,12,13,14 };
        private static int key = 6;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
