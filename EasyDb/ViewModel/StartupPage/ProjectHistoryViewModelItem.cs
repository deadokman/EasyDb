using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using EasyDb.ProjectManagment.Annotations;
using EasyDb.ProjectManagment.Configuration;

namespace EasyDb.ViewModel.StartupPage
{
    /// <summary>
    /// Represents view model item of project history
    /// </summary>
    public class ProjectHistoryViewModelItem : INotifyPropertyChanged, IComparable<ProjectHistoryViewModelItem>
    {
        private bool _mouseHovering;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectHistoryViewModelItem"/> class.
        /// Represents view model item of project history
        /// </summary>
        /// <param name="histItem">History item model</param>
        public ProjectHistoryViewModelItem([NotNull] ProjectHistItem histItem)
        {
            HistItem = histItem ?? throw new ArgumentNullException(nameof(histItem));
        }

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Hist item model
        /// </summary>
        public ProjectHistItem HistItem { get; }

        /// <summary>
        /// Item group name
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Item pinned
        /// </summary>
        public bool IsPinned
        {
            get => HistItem.Pinned;
            set => HistItem.Pinned = value;
        }

        /// <summary>
        /// Project file location
        /// </summary>
        public string FolderPath
        {
            get => HistItem.ProjectFileLocation;
        }

        /// <summary>
        /// Project name
        /// </summary>
        public string ProjectName
        {
            get => HistItem.ProjectName;
        }

        /// <summary>
        /// Autolaunch
        /// </summary>
        public bool AutoLaunch
        {
            get => HistItem.Autolaunch;
            set
            {
                HistItem.Autolaunch = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Record last access time
        /// </summary>
        public DateTime LastAccess
        {
            get => HistItem.LastAccess.Date;
        }

        /// <summary>
        /// Mouse hovering element
        /// </summary>
        public bool MouseHovering
        {
            get => _mouseHovering;
            set
            {
                _mouseHovering = value;
                OnPropertyChanged();
            }
        }

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object. </summary>
        /// <param name="other">An object to compare with this instance. </param>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order. </returns>
        public int CompareTo(ProjectHistoryViewModelItem other)
        {
            if (other?.HistItem == null)
            {
                return 0;
            }

            if (other.HistItem.LastAccess.Date > this.HistItem.LastAccess.Date)
            {
                return 1;
            }

            if (other.HistItem.LastAccess.Date < this.HistItem.LastAccess.Date)
            {
                return -1;
            }

            if (other.HistItem.LastAccess.Date == this.HistItem.LastAccess.Date)
            {
                return 0;
            }

            return 0;
        }

        /// <summary>
        /// Prop changed
        /// </summary>
        /// <param name="propertyName">Prop name</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
