namespace NetBlaze.SharedKernel.HelperUtilities.Constants
{
    public static class ApiRelativePaths
    {
        // Common Api Path
        public const string API_COMMON_PREFIX = "/api";

        // Sample Paths
        public const string SAMPLE_BASE = $"{API_COMMON_PREFIX}/sample";
        public const string SAMPLE_LIST = $"{SAMPLE_BASE}/list";
        public const string SAMPLE_GET = SAMPLE_BASE;
        public const string SAMPLE_ADD = $"{SAMPLE_BASE}/add";
        public const string SAMPLE_UPDATE = $"{SAMPLE_BASE}/update";
        public const string SAMPLE_DELETE = $"{SAMPLE_BASE}/delete";

        // Auth Paths
        public const string AUTH_BASE = $"{API_COMMON_PREFIX}/auth/";
        public const string AUTH_REGISTER = $"{AUTH_BASE}/register";
        public const string AUTH_REGISTER_FIDO_START = $"{AUTH_BASE}/register-fido-user";
        public const string AUTH_REGISTER_FIDO_COMPLETE = $"{AUTH_BASE}/register-user-credential";
        public const string AUTH_LOGIN = $"{AUTH_BASE}/LOGIN";
    }
}