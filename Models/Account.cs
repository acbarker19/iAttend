
using System;

namespace iAttend.Models
{
    public class Account
    {
        public long id { get; set; }
        public string firstName { get; set; }
        public string nickname { get; set; } = null;
        public string lastName { get; set; }
        public string email { get; set; }
        public byte[] salt { get; set; }
        public string passwordHash { get; set; }
        public int accountType { get; set; } = (int)AccountType.Student;
        public bool emailVerified { get; set; } = false;
        public DateTime expectedGraduationDate { get; set; }
        public Guid trackId { get; set; }

        public Account(long id, string firstName, string nickname, string lastName, string email, byte[] salt, string passwordHash, int accountType, bool emailVerified, DateTime expectedGraduationDate, Guid trackId)
        {
            this.id = id;
            this.firstName = firstName;
            this.nickname = nickname;
            this.lastName = lastName;
            this.email = email;
            this.salt = salt;
            this.passwordHash = passwordHash;
            this.accountType = accountType;
            this.emailVerified = emailVerified;
            this.expectedGraduationDate = expectedGraduationDate;
            this.trackId = trackId;
        }

        public Account(long id, string firstName, string nickname, string lastName, string email, byte[] salt, string passwordHash, DateTime expectedGraduationDate, Guid trackId)
        {
            this.id = id;
            this.firstName = firstName;
            this.nickname = nickname;
            this.lastName = lastName;
            this.email = email;
            this.salt = salt;
            this.passwordHash = passwordHash;
            this.accountType = accountType;
            this.emailVerified = emailVerified;
            this.expectedGraduationDate = expectedGraduationDate;
            this.trackId = trackId;
        }
    }
}