namespace Edb.Environment.Model
{
    using NuGet;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Versioning;

    /// <summary>
    /// Defines the <see cref="ChocolateyPackage" />
    /// </summary>
    public class ChocolateyPackage : IPackage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPackage"/> class.
        /// </summary>
        /// <param name="Id">The Id<see cref="string"/></param>
        public ChocolateyPackage(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Gets the Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the Version
        /// </summary>
        public SemanticVersion Version { get; }

        /// <summary>
        /// Gets the ProjectSourceUrl
        /// </summary>
        public Uri ProjectSourceUrl { get; }

        /// <summary>
        /// Gets the PackageSourceUrl
        /// </summary>
        public Uri PackageSourceUrl { get; }

        /// <summary>
        /// Gets the DocsUrl
        /// </summary>
        public Uri DocsUrl { get; }

        /// <summary>
        /// Gets the WikiUrl
        /// </summary>
        public Uri WikiUrl { get; }

        /// <summary>
        /// Gets the MailingListUrl
        /// </summary>
        public Uri MailingListUrl { get; }

        /// <summary>
        /// Gets the BugTrackerUrl
        /// </summary>
        public Uri BugTrackerUrl { get; }

        /// <summary>
        /// Gets the Replaces
        /// </summary>
        public IEnumerable<string> Replaces { get; }

        /// <summary>
        /// Gets the Provides
        /// </summary>
        public IEnumerable<string> Provides { get; }

        /// <summary>
        /// Gets the Conflicts
        /// </summary>
        public IEnumerable<string> Conflicts { get; }

        /// <summary>
        /// Gets the SoftwareDisplayName
        /// </summary>
        public string SoftwareDisplayName { get; }

        /// <summary>
        /// Gets the SoftwareDisplayVersion
        /// </summary>
        public string SoftwareDisplayVersion { get; }

        /// <summary>
        /// Gets the Title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the Authors
        /// </summary>
        public IEnumerable<string> Authors { get; }

        /// <summary>
        /// Gets the Owners
        /// </summary>
        public IEnumerable<string> Owners { get; }

        /// <summary>
        /// Gets the IconUrl
        /// </summary>
        public Uri IconUrl { get; }

        /// <summary>
        /// Gets the LicenseUrl
        /// </summary>
        public Uri LicenseUrl { get; }

        /// <summary>
        /// Gets the ProjectUrl
        /// </summary>
        public Uri ProjectUrl { get; }

        /// <summary>
        /// Gets a value indicating whether RequireLicenseAcceptance
        /// </summary>
        public bool RequireLicenseAcceptance { get; }

        /// <summary>
        /// Gets a value indicating whether DevelopmentDependency
        /// </summary>
        public bool DevelopmentDependency { get; }

        /// <summary>
        /// Gets the Description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the Summary
        /// </summary>
        public string Summary { get; }

        /// <summary>
        /// Gets the ReleaseNotes
        /// </summary>
        public string ReleaseNotes { get; }

        /// <summary>
        /// Gets the Language
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// Gets the Tags
        /// </summary>
        public string Tags { get; }

        /// <summary>
        /// Gets the Copyright
        /// </summary>
        public string Copyright { get; }

        /// <summary>
        /// Gets the FrameworkAssemblies
        /// </summary>
        public IEnumerable<FrameworkAssemblyReference> FrameworkAssemblies { get; }

        /// <summary>
        /// Gets the PackageAssemblyReferences
        /// </summary>
        public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; }

        /// <summary>
        /// Gets the DependencySets
        /// </summary>
        public IEnumerable<PackageDependencySet> DependencySets { get; }

        /// <summary>
        /// Gets the MinClientVersion
        /// </summary>
        public Version MinClientVersion { get; }

        /// <summary>
        /// Gets the PackageHash
        /// </summary>
        public string PackageHash { get; }

        /// <summary>
        /// Gets the PackageHashAlgorithm
        /// </summary>
        public string PackageHashAlgorithm { get; }

        /// <summary>
        /// Gets the PackageSize
        /// </summary>
        public long PackageSize { get; }

        /// <summary>
        /// Gets the VersionDownloadCount
        /// </summary>
        public int VersionDownloadCount { get; }

        /// <summary>
        /// Gets a value indicating whether IsApproved
        /// </summary>
        public bool IsApproved { get; }

        /// <summary>
        /// Gets the PackageStatus
        /// </summary>
        public string PackageStatus { get; }

        /// <summary>
        /// Gets the PackageSubmittedStatus
        /// </summary>
        public string PackageSubmittedStatus { get; }

        /// <summary>
        /// Gets the PackageTestResultStatus
        /// </summary>
        public string PackageTestResultStatus { get; }

        /// <summary>
        /// Gets the PackageTestResultStatusDate
        /// </summary>
        public DateTime? PackageTestResultStatusDate { get; }

        /// <summary>
        /// Gets the PackageValidationResultStatus
        /// </summary>
        public string PackageValidationResultStatus { get; }

        /// <summary>
        /// Gets the PackageValidationResultDate
        /// </summary>
        public DateTime? PackageValidationResultDate { get; }

        /// <summary>
        /// Gets the PackageCleanupResultDate
        /// </summary>
        public DateTime? PackageCleanupResultDate { get; }

        /// <summary>
        /// Gets the PackageReviewedDate
        /// </summary>
        public DateTime? PackageReviewedDate { get; }

        /// <summary>
        /// Gets the PackageApprovedDate
        /// </summary>
        public DateTime? PackageApprovedDate { get; }

        /// <summary>
        /// Gets the PackageReviewer
        /// </summary>
        public string PackageReviewer { get; }

        /// <summary>
        /// Gets a value indicating whether IsDownloadCacheAvailable
        /// </summary>
        public bool IsDownloadCacheAvailable { get; }

        /// <summary>
        /// Gets the DownloadCacheDate
        /// </summary>
        public DateTime? DownloadCacheDate { get; }

        /// <summary>
        /// Gets the DownloadCache
        /// </summary>
        public IEnumerable<DownloadCache> DownloadCache { get; }

        /// <summary>
        /// Gets the ReportAbuseUrl
        /// </summary>
        public Uri ReportAbuseUrl { get; }

        /// <summary>
        /// Gets the DownloadCount
        /// </summary>
        public int DownloadCount { get; }

        /// <summary>
        /// The GetFiles
        /// </summary>
        /// <returns>The <see cref="IEnumerable{IPackageFile}"/></returns>
        public IEnumerable<IPackageFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetSupportedFrameworks
        /// </summary>
        /// <returns>The <see cref="IEnumerable{FrameworkName}"/></returns>
        public IEnumerable<FrameworkName> GetSupportedFrameworks()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The GetStream
        /// </summary>
        /// <returns>The <see cref="Stream"/></returns>
        public Stream GetStream()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The ExtractContents
        /// </summary>
        /// <param name="fileSystem">The fileSystem<see cref="IFileSystem"/></param>
        /// <param name="extractPath">The extractPath<see cref="string"/></param>
        public void ExtractContents(IFileSystem fileSystem, string extractPath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The OverrideOriginalVersion
        /// </summary>
        /// <param name="version">The version<see cref="SemanticVersion"/></param>
        public void OverrideOriginalVersion(SemanticVersion version)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a value indicating whether IsAbsoluteLatestVersion
        /// </summary>
        public bool IsAbsoluteLatestVersion { get; }

        /// <summary>
        /// Gets a value indicating whether IsLatestVersion
        /// </summary>
        public bool IsLatestVersion { get; }

        /// <summary>
        /// Gets a value indicating whether Listed
        /// </summary>
        public bool Listed { get; }

        /// <summary>
        /// Gets the Published
        /// </summary>
        public DateTimeOffset? Published { get; }

        /// <summary>
        /// Gets the AssemblyReferences
        /// </summary>
        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; }
    }
}
