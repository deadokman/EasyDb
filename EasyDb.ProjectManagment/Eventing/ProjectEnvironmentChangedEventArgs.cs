namespace EasyDb.ProjectManagment.Eventing
{
    using System;

    /// <summary>
    /// Defines the <see cref="ProjectEnvironmentChangedEventArgs" />
    /// </summary>
    public class ProjectEnvironmentChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectEnvironmentChangedEventArgs"/> class.
        /// </summary>
        /// <param name="sender">The sender<see cref="object"/></param>
        public ProjectEnvironmentChangedEventArgs(object sender)
            : base()
        {
        }
    }
}
