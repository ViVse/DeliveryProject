﻿using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class User: IdentityUser 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Avatar { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
