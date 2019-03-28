using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using EasyDb.Model;
using EasyDb.ProjectManagment.Annotations;
using EasyDb.ProjectManagment.Configuration;
using EasyDb.ProjectManagment.Intefraces;
using EasyDb.ProjectManagment.ProjectSchema;
using Edb.Environment.DatasourceManager;
using Edb.Environment.Interface;

namespace EasyDb.ProjectManagment
{
    /// <summary>
    /// Manage projects with files and datasources
    /// </summary>
    public class ApplicationEnvironment : IApplicationEnvironment
    {
        private readonly IDataSourceManager _datasourceManager;
        private EasyDbProject _currentProject;
        private XmlSerializer _serializer;
        private const string ProjectInformationDatasource = "ProjectData.xml";
        private readonly string ProjectInformationFilepath;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="datasourceManager">Datasource manager</param>
        public ApplicationEnvironment([NotNull] IDataSourceManager datasourceManager)
        {
            _datasourceManager = datasourceManager ?? throw new ArgumentNullException(nameof(datasourceManager));
            _serializer = new XmlSerializer(typeof(ProjectHistItem[]));
        }

        /// <summary>
        /// Initialize proje
        /// </summary>
        /// <param name="datasourceLibPath">path to datasource drivers libs</param>
        /// <param name="applicationConfigurationPath">Local appforder path</param>
        public void Initialize(string datasourceLibPath, string applicationConfigurationPath)
        {
            _datasourceManager.InitialLoad(datasourceLibPath);
        }

        /// <summary>
        /// Supported datasources
        /// </summary>
        public SupportedSourceItem[] SupportedSources { get; set; }

        /// <summary>
        /// Project opened today
        /// </summary>
        public ObservableCollection<ProjectHistItem> ProjectHistoryItems { get; set; }

        /// <summary>
        /// Current working project
        /// </summary>
        public EasyDbProject CurrentProject
        {
            get => _currentProject;
            set
            {
                _currentProject = value;
            }
        }
    }
}
