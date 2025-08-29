using KeeperComparer.Interfaces;

namespace KeeperComparer.Services
{
    public class EmailComparer : IEmailComparer
    {
        public bool AreEqual(string? a, string? b)
        {
            var normA = Normalize(a);
            var normB = Normalize(b);

            if (string.IsNullOrEmpty(normA) || string.IsNullOrEmpty(normB))
                return false;

            return normA == normB;
        }

        private string Normalize(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            return email.Trim().ToLowerInvariant();
        }
    }
}