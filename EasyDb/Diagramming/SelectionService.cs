namespace EasyDb.Diagramming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="SelectionService" />
    /// </summary>
    internal class SelectionService
    {
        /// <summary>
        /// Defines the designerCanvas
        /// </summary>
        private DesignerCanvas designerCanvas;

        /// <summary>
        /// Defines the currentSelection
        /// </summary>
        private List<ISelectable> currentSelection;

        /// <summary>
        /// Gets the CurrentSelection
        /// </summary>
        internal List<ISelectable> CurrentSelection
        {
            get
            {
                if (currentSelection == null)
                    currentSelection = new List<ISelectable>();

                return currentSelection;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionService"/> class.
        /// </summary>
        /// <param name="canvas">The canvas<see cref="DesignerCanvas"/></param>
        public SelectionService(DesignerCanvas canvas)
        {
            this.designerCanvas = canvas;
        }

        /// <summary>
        /// The SelectItem
        /// </summary>
        /// <param name="item">The item<see cref="ISelectable"/></param>
        internal void SelectItem(ISelectable item)
        {
            this.ClearSelection();
            this.AddToSelection(item);
        }

        /// <summary>
        /// The AddToSelection
        /// </summary>
        /// <param name="item">The item<see cref="ISelectable"/></param>
        internal void AddToSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = true;
                    CurrentSelection.Add(groupItem);
                }
            }
            else
            {
                item.IsSelected = true;
                CurrentSelection.Add(item);
            }
        }

        /// <summary>
        /// The RemoveFromSelection
        /// </summary>
        /// <param name="item">The item<see cref="ISelectable"/></param>
        internal void RemoveFromSelection(ISelectable item)
        {
            if (item is IGroupable)
            {
                List<IGroupable> groupItems = GetGroupMembers(item as IGroupable);

                foreach (ISelectable groupItem in groupItems)
                {
                    groupItem.IsSelected = false;
                    CurrentSelection.Remove(groupItem);
                }
            }
            else
            {
                item.IsSelected = false;
                CurrentSelection.Remove(item);
            }
        }

        /// <summary>
        /// The ClearSelection
        /// </summary>
        internal void ClearSelection()
        {
            CurrentSelection.ForEach(item => item.IsSelected = false);
            CurrentSelection.Clear();
        }

        /// <summary>
        /// The SelectAll
        /// </summary>
        internal void SelectAll()
        {
            ClearSelection();
            CurrentSelection.AddRange(designerCanvas.Children.OfType<ISelectable>());
            CurrentSelection.ForEach(item => item.IsSelected = true);
        }

        /// <summary>
        /// The GetGroupMembers
        /// </summary>
        /// <param name="item">The item<see cref="IGroupable"/></param>
        /// <returns>The <see cref="List{IGroupable}"/></returns>
        internal List<IGroupable> GetGroupMembers(IGroupable item)
        {
            IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
            IGroupable rootItem = GetRoot(list, item);
            return GetGroupMembers(list, rootItem);
        }

        /// <summary>
        /// The GetGroupRoot
        /// </summary>
        /// <param name="item">The item<see cref="IGroupable"/></param>
        /// <returns>The <see cref="IGroupable"/></returns>
        internal IGroupable GetGroupRoot(IGroupable item)
        {
            IEnumerable<IGroupable> list = designerCanvas.Children.OfType<IGroupable>();
            return GetRoot(list, item);
        }

        /// <summary>
        /// The GetRoot
        /// </summary>
        /// <param name="list">The list<see cref="IEnumerable{IGroupable}"/></param>
        /// <param name="node">The node<see cref="IGroupable"/></param>
        /// <returns>The <see cref="IGroupable"/></returns>
        private IGroupable GetRoot(IEnumerable<IGroupable> list, IGroupable node)
        {
            if (node == null || node.ParentID == Guid.Empty)
            {
                return node;
            }
            else
            {
                foreach (IGroupable item in list)
                {
                    if (item.ID == node.ParentID)
                    {
                        return GetRoot(list, item);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// The GetGroupMembers
        /// </summary>
        /// <param name="list">The list<see cref="IEnumerable{IGroupable}"/></param>
        /// <param name="parent">The parent<see cref="IGroupable"/></param>
        /// <returns>The <see cref="List{IGroupable}"/></returns>
        private List<IGroupable> GetGroupMembers(IEnumerable<IGroupable> list, IGroupable parent)
        {
            List<IGroupable> groupMembers = new List<IGroupable>();
            groupMembers.Add(parent);

            var children = list.Where(node => node.ParentID == parent.ID);

            foreach (IGroupable child in children)
            {
                groupMembers.AddRange(GetGroupMembers(list, child));
            }

            return groupMembers;
        }
    }
}
