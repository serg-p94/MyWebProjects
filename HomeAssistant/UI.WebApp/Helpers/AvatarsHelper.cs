using System.IO;
using System.Web;
using BL.Users;

namespace UI.WebApp.Helpers
{
    public class AvatarsHelper
    {
        private readonly IUserManager _userManager;

        private const string UserAvatarMale = "user_male.png";
        private const string UserAvatarFemale = "userfe_male.png";

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
            string sourceFileName;
            if (avatar.ContentLength > 0)
            {
                sourceFileName = avatar.FileName;
            }
            else if (user.IsMale)
            {
                sourceFileName = UserAvatarMale;
            }
            else
            {
                sourceFileName = UserAvatarFemale;
            }

            var fileName = string.Format("{0}{1}", user.Login, Path.GetExtension(sourceFileName));
            var dstPath = string.Format("{0}{1}", FoldersPathes.AvatarsFolder, fileName);
            dstPath = HttpContext.Current.Server.MapPath(dstPath);

            if (avatar.ContentLength > 0)
            {
                avatar.SaveAs(dstPath);
            }
            else
            {
                var srcPath = HttpContext.Current.Server.MapPath(FoldersPathes.AvatarsFolder + sourceFileName);
                File.Copy(srcPath, dstPath);
            }

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