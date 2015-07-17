using System.IO;
using System.Web;
using BL.Users;

namespace UI.WebApp.Helpers
{
    public class AvatarsHelper
    {
        private readonly IUserManager _userManager;

        public AvatarsHelper(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public void ChangeAvatar(User user, HttpPostedFileBase avatar)
        {
            RemoveAvatarFromStore(user);
            var fileName = CopyAvatarToStore(user, avatar);
            user.Avatar = fileName;
            _userManager.Update();
        }

        private string CopyAvatarToStore(User user, HttpPostedFileBase avatar)
        {
            var fileName = string.Format("{0}{1}", user.Login, Path.GetExtension(avatar.FileName));
            var path = string.Format("{0}{1}", FoldersPathes.AvatarsFolder, fileName);
            path = HttpContext.Current.Server.MapPath(path);
            avatar.SaveAs(path);

            return fileName;
        }

        private void RemoveAvatarFromStore(User user)
        {
            var path = FoldersPathes.AvatarsFolder + user.Avatar;
            path = HttpContext.Current.Server.MapPath(path);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}