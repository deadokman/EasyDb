namespace Edb.Environment.Delegates
{
    using Edb.Environment.EventArgs;
    using Edb.Environment.Interface;

    /// <summary>
    /// The ConnectionError
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ConnectionError(IEDbConnectionLink sender, ConnectionErrorEventArgs e);
}
