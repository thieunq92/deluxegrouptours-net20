using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using CMS.Core.Service.Files;
using CMS.Modules.Gallery.Domain;

namespace CMS.Modules.Gallery
{
    public class GalleryPathBuilder
    {
        private IFileService _fileService;

        public string PhysicalDir;

        public GalleryPathBuilder(IFileService fileService)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// The physical directory where the files are located.
        /// </summary>
        /// <remarks>
        /// If there is no setting for the physical directory, the default of
        /// Application_Root/UserFiles/Gallery is used.
        ///	</remarks>
        public string FileDir
        {
            get
            {
                if (this.PhysicalDir == null)
                {
                    this.PhysicalDir = HttpContext.Current.Server.MapPath("~/UserFiles/Gallery");
                    CheckPhysicalDirectory(this.PhysicalDir);
                }
                return this.PhysicalDir;
            }
            set
            {
                this.PhysicalDir = value;
                CheckPhysicalDirectory(this.PhysicalDir);
            }
        }

        public void CheckPhysicalDirectory(string physicalDir)
        {
            // Check existence
            if (!System.IO.Directory.Exists(physicalDir))
            {
                throw new Exception(String.Format("The Gallery module didn't find the physical directory {0} on the server.", physicalDir));
            }

            // Check if the diretory is writable
            if (!this._fileService.CheckIfDirectoryIsWritable(physicalDir))
            {
                throw new Exception(String.Format("The physical directory {0} is not writable.", physicalDir));
            }
        }


        public string GetAlbumDirectory(int albumId)
        {
            return HttpContext.Current.Server.MapPath(this.FileDir + System.IO.Path.DirectorySeparatorChar + albumId);
        }

        public string GetAlbumVirtualDirectory(int albumId)
        {
            return FileDir + System.IO.Path.DirectorySeparatorChar + albumId;
        }

        public string GetThumbPath(Photo photo)
        {
            if (!photo.Album.UseSimpleViewer)
            {
                return GetOldThumbPath(photo);
            }
            return HttpContext.Current.Server.MapPath(FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar+ "thumbnails" + System.IO.Path.DirectorySeparatorChar
                + photo.FileName);
        }

        public string GetThumbDirectory(Album album)
        {
            return HttpContext.Current.Server.MapPath(FileDir + System.IO.Path.DirectorySeparatorChar
                + album.Id + System.IO.Path.DirectorySeparatorChar + "thumbnails");
        }

        [Obsolete("Hàm cũ, giữ lại cho việc convert")]
        public string GetOldThumbPath(Photo photo)
        {
            string[] parts = photo.FileName.Split('.');

            if (parts.Length > 2)
            {
                int ii;
                for (ii = photo.FileName.Length; ii > 0; ii--)
                {
                    if (photo.FileName[ii - 1] == '.')
                    {
                        parts = new string[2];
                        parts[0] = photo.FileName.Substring(0, ii - 2);
                        parts[1] = photo.FileName.Substring(ii);
                        break;
                    }
                }
            }

            return HttpContext.Current.Server.MapPath(this.FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar
                + parts[0] + "-thumb." + parts[1]);
        }

        /// <summary>
        /// Gets the path to original photo uploaded 
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public string GetPhotoOriginPath(Photo photo)
        {
            if (photo.Album.UseSimpleViewer)
            {
                return GetPath(photo);
            }
            string[] parts = photo.FileName.Split('.');

            if (parts.Length > 2)
            {
                int ii;
                for (ii = photo.FileName.Length; ii > 0; ii --)
                {
                    if (photo.FileName[ii-1] == '.')
                    {
                        parts = new string[2];
                        parts[0] = photo.FileName.Substring(0, ii - 2);
                        parts[1] = photo.FileName.Substring(ii);
                        break;
                    }
                }
            }
                //throw new Exception("An image is only allowed to have one dot in the name: " + photo.FileName);

            return HttpContext.Current.Server.MapPath( this.FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar
                + parts[0] + "-origin." + parts[1]);
        }

        [Obsolete("Hàm cũ, giữ lại cho việc convert")]
        public string GetOldPhotoOriginPath(Photo photo)
        {
            string[] parts = photo.FileName.Split('.');

            if (parts.Length > 2)
            {
                int ii;
                for (ii = photo.FileName.Length; ii > 0; ii--)
                {
                    if (photo.FileName[ii - 1] == '.')
                    {
                        parts = new string[2];
                        parts[0] = photo.FileName.Substring(0, ii - 2);
                        parts[1] = photo.FileName.Substring(ii);
                        break;
                    }
                }
            }
            //throw new Exception("An image is only allowed to have one dot in the name: " + photo.FileName);

            return HttpContext.Current.Server.MapPath(this.FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar
                + parts[0] + "-origin." + parts[1]);
        }

        public string GetPath(Photo photo)
        {
            if (!photo.Album.UseSimpleViewer)
            {
                return GetOldPath(photo);
            }
            return HttpContext.Current.Server.MapPath(this.FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar
                + photo.FileName);            
        }

        [Obsolete("Hàm cũ giữ lại cho việc convert")]
        public string GetOldPath(Photo photo)
        {
            return HttpContext.Current.Server.MapPath(this.FileDir + System.IO.Path.DirectorySeparatorChar
                + photo.Album.Id + System.IO.Path.DirectorySeparatorChar
                + photo.FileName);
        }        
    }
}
