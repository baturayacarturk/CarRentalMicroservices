﻿namespace UserInterface.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public IEnumerable<string> GetUser()
        {
            yield return UserName;
            yield return Email;
            yield return City;
        }
    }
}
