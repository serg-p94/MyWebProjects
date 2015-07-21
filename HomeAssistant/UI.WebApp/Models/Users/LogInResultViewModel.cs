using BL.Users;

namespace UI.WebApp.Models.Users
{
    public struct LogInResultViewModel
    {
        public bool HasResult { get; set; }
        public UserValidationResult Result { get; set; }
    }
}