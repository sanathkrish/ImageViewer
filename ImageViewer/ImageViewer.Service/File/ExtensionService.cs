using ImageViewer.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.File
{
    public class ExtensionService
    {
        public async Task GetExtensions(string directoryPath, Action<ExtensionInfo> callback)
        {
            var excludedPaths = new HashSet<string>
            {
                "node_modules",
                "bin",
                "obj",
                "dist",
                "build",
                "release",
                 ".git",
    ".gradle",
    "build",
    "app\\build",
    "bin",
    "obj",
    ".angular",
    ".github",
    ".vscode",
            };
            await Task.Run(() =>
            {
                var extensions = new HashSet<string>();
                var options = new EnumerationOptions
                {
                    RecurseSubdirectories = true,
                    IgnoreInaccessible = true,
                    
                };
                var files = System.IO.Directory.EnumerateFiles(directoryPath, "*.*", options);
                foreach (var file in files)
                {
                    var containsAny = excludedPaths.Any(excludedPath => file.ToLower().Contains(excludedPath));
                    if (containsAny)
                        continue;
                    var extension = System.IO.Path.GetExtension(file).ToLower();
                    callback(new ExtensionInfo { Extension = extension, Path = file });
                }
            });
        }

        public List<string> GetDirectories(string directoryPath)
        {
            var directories = new List<string>();
            var options = new EnumerationOptions
            {
                RecurseSubdirectories = true,
                IgnoreInaccessible = true
            };
            var dirs = System.IO.Directory.GetDirectories(directoryPath);
            foreach (var dir in dirs)
            {
                directories.Add(dir);
            }
            return directories;
        }
    }
}
