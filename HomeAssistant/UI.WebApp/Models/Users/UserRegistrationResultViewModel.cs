using BL.Users;

namespace UI.WebApp.Models.Users
{
    public struct UserRegistrationResultViewModel
    {
        public bool HasResult { get; set; }
        public UserRegistrationResult Result { get; set; }  
    }
}