using MrX.Web.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class User
    {
        [Key] public Guid Id { get; set; }
        [Required(AllowEmptyStrings = false)][MaxLength(200)][MinLength(3)] public string Username { get; set; } = null!;
        [Required(AllowEmptyStrings = false)][MaxLength(500)] public string HashPassword { get; set; } = null!;
        [Required] public UserRule Rule { get; set; } = UserRule.None;
        public List<News> News { get; set; } = [];
        public List<News> LikedNews { get; set; } = [];
        public User SetPassword(string password)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(password);
            HashPassword = PasswordHash.Hash(password);
            return this;
        }
        public bool VeryfiPassword(string password) => PasswordHash.Verify(password, HashPassword);
    }
    [Flags]
    public enum UserRule
    {
        Developer = 0b0001,
        Admin = 0b0010,
        Editor = 0b0100,
        None = 0b1000,
    }
}
