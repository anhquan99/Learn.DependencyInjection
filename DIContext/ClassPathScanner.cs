using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class ClassPathScanner
{
    // this code is very much simplified; it works, but do not use it in production!
    public static HashSet<Type> GetAllClassesInPackage(string packageName)
    {
        var path = packageName.Replace('.', '/');
        var classLoader = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name == path);
        if (classLoader == null) return null;
        var resources = classLoader.GetTypes().Select(a => a.FullName);
        var classes = new HashSet<Type>();
        foreach (var resource in resources)
        {
            var className = resource.Replace('/', '.').Replace(".class", "");
            var type = Type.GetType(className);
            if (type != null)
            {
                classes.Add(type);
            }
        }

        return classes;
    }

    private static List<Type> FindClasses(DirectoryInfo directory, string packageName)
    {
        var classes = new List<Type>();
        if (!directory.Exists)
        {
            return classes;
        }

        var files = directory.GetFiles();
        foreach (var file in files)
        {
            if (file is null) continue;
            if (file.Attributes.HasFlag(FileAttributes.Directory))
            {
                classes.AddRange(FindClasses(file.Directory, packageName + "." + file.Name));
            }
            else if (file.Extension == ".class")
            {
                var className = packageName + '.' + file.Name.Substring(0, file.Name.Length - 6);
                var type = Type.GetType(className);
                if (type != null)
                {
                    classes.Add(type);
                }
            }
        }

        return classes;
    }
}