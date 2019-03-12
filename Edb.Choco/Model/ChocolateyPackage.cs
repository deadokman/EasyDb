using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using NuGet;

namespace Edb.Environment.Model
{
    public class ChocolateyPackage : IPackage
    {
        public string Id { get; }
        public SemanticVersion Version { get; }
        public Uri ProjectSourceUrl { get; }
        public Uri PackageSourceUrl { get; }
        public Uri DocsUrl { get; }
        public Uri WikiUrl { get; }
        public Uri MailingListUrl { get; }
        public Uri BugTrackerUrl { get; }
        public IEnumerable<string> Replaces { get; }
        public IEnumerable<string> Provides { get; }
        public IEnumerable<string> Conflicts { get; }
        public string SoftwareDisplayName { get; }
        public string SoftwareDisplayVersion { get; }
        public string Title { get; }
        public IEnumerable<string> Authors { get; }
        public IEnumerable<string> Owners { get; }
        public Uri IconUrl { get; }
        public Uri LicenseUrl { get; }
        public Uri ProjectUrl { get; }
        public bool RequireLicenseAcceptance { get; }
        public bool DevelopmentDependency { get; }
        public string Description { get; }
        public string Summary { get; }
        public string ReleaseNotes { get; }
        public string Language { get; }
        public string Tags { get; }
        public string Copyright { get; }
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; }
        public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; }
        public IEnumerable<PackageDependencySet> DependencySets { get; }
        public Version MinClientVersion { get; }
        public string PackageHash { get; }
        public string PackageHashAlgorithm { get; }
        public long PackageSize { get; }
        public int VersionDownloadCount { get; }
        public bool IsApproved { get; }
        public string PackageStatus { get; }
        public string PackageSubmittedStatus { get; }
        public string PackageTestResultStatus { get; }
        public DateTime? PackageTestResultStatusDate { get; }
        public string PackageValidationResultStatus { get; }
        public DateTime? PackageValidationResultDate { get; }
        public DateTime? PackageCleanupResultDate { get; }
        public DateTime? PackageReviewedDate { get; }
        public DateTime? PackageApprovedDate { get; }
        public string PackageReviewer { get; }
        public bool IsDownloadCacheAvailable { get; }
        public DateTime? DownloadCacheDate { get; }
        public IEnumerable<DownloadCache> DownloadCache { get; }
        public Uri ReportAbuseUrl { get; }
        public int DownloadCount { get; }
        public IEnumerable<IPackageFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FrameworkName> GetSupportedFrameworks()
        {
            throw new NotImplementedException();
        }

        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        public void ExtractContents(IFileSystem fileSystem, string extractPath)
        {
            throw new NotImplementedException();
        }

        public void OverrideOriginalVersion(SemanticVersion version)
        {
            throw new NotImplementedException();
        }

        public bool IsAbsoluteLatestVersion { get; }
        public bool IsLatestVersion { get; }
        public bool Listed { get; }
        public DateTimeOffset? Published { get; }
        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; }
    }
}
