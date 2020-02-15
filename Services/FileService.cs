using System;
using RAAA.DataModel;
using System.Collections.Generic;
using System.Text;

namespace RAAA.Services
{
    public interface IFileService
    {
        public void LoadFileMetaData();

        public File GetFileMetaData(string Path);

        public bool OpenFileAsAdmin(string Path);
    }

    class FileService
    {

    }
}
