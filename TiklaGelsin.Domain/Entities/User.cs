using System;
using TiklaGelsin.Domain.Exceptions;

namespace TiklaGelsin.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(Guid id, string username, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("Username cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new DomainException("Password hash cannot be empty.");

            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new DomainException("New password hash cannot be empty.");

            PasswordHash = newPasswordHash;
        }
    }
}
