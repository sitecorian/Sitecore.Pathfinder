// � 2015-2016 Sitecore Corporation A/S. All rights reserved.

using System.ComponentModel.Composition;
using System.IO;
using Sitecore.Pathfinder.Diagnostics;
using Sitecore.Pathfinder.Extensions;
using Sitecore.Pathfinder.IO;
using Sitecore.Pathfinder.Packaging.ProjectPackages;
using Sitecore.Pathfinder.Tasks.Building;

namespace Sitecore.Pathfinder.Tasks
{
    public class CopyDependencies : BuildTaskBase
    {
        [ImportingConstructor]
        public CopyDependencies([NotNull] IFileSystemService fileSystem, [NotNull] IProjectPackageService projectPackages) : base("copy-dependencies")
        {
            FileSystem = fileSystem;
            ProjectPackages = projectPackages;
        }

        [NotNull]
        protected IFileSystemService FileSystem { get; }

        [NotNull]
        protected IProjectPackageService ProjectPackages { get; }

        public override void Run(IBuildContext context)
        {
            context.Trace.TraceInformation(Msg.D1000, Texts.Copying_dependencies___);

            if (!IsProjectConfigured(context))
            {
                return;
            }

            var sourceDirectory = context.Configuration.GetString(Constants.Configuration.CopyDependencies.SourceDirectory);
            sourceDirectory = Path.Combine(context.ProjectDirectory, sourceDirectory);
            if (!FileSystem.DirectoryExists(sourceDirectory))
            {
                context.Trace.TraceInformation(Msg.D1003, Texts.Dependencies_directory_not_found__Skipping, sourceDirectory);
                return;
            }

            foreach (var pair in context.Configuration.GetSubKeys("copy-package"))
            {
                var key = "copy-package:" + pair.Key;

                var destinationDirectory = context.Configuration.GetString(key + ":copy-to-directory");
                if (string.IsNullOrEmpty(destinationDirectory))
                {
                    context.Trace.TraceError(Msg.D1001, Texts.Destination_directory_not_found, key + ":copy-to-directory");
                    continue;
                }

                destinationDirectory = PathHelper.NormalizeFilePath(destinationDirectory).TrimStart('\\');
                destinationDirectory = PathHelper.Combine(context.Configuration.GetString(Constants.Configuration.DataFolderDirectory), destinationDirectory);

                FileSystem.CreateDirectory(destinationDirectory);

                foreach (var packageInfo in ProjectPackages.GetPackages())
                {
                    if (ProjectPackages.CopyPackageToWebsite(packageInfo, destinationDirectory))
                    {
                        context.Trace.TraceInformation(Msg.D1002, Texts.Copying_dependency, packageInfo.Id + "." + packageInfo.Version);
                    }
                }
            }
        }
    }
}
