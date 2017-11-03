using GongSolutions.Wpf.DragDrop;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Twenty57.Linx.Components.Database.UI
{
	public class DragDropController : IDragSource, IDropTarget
	{
		public interface IDraggableRowItem
		{
			bool SupportsDragDrop { get; }
			bool IsDragged { get; set; }
			int RowIndex { get; }

			void MoveToIndex(int index);
		}


		private IDraggableRowItem item;

		public DragDropController(IDraggableRowItem item)
		{
			this.item = item;
		}

		#region IDragSource
		public void StartDrag(IDragInfo dragInfo)
		{
			if (item.SupportsDragDrop)
			{
				dragInfo.Data = new DragData(item);
				dragInfo.Effects = DragDropEffects.Move;
			}
		}

		public void Dropped(IDropInfo dropInfo) { }
		#endregion

		#region IDropTarget
		public void DragOver(IDropInfo dropInfo)
		{
			if (item.SupportsDragDrop)
			{
				DragData dragData = (DragData)dropInfo.Data;
				dragData.Item.MoveToIndex(item.RowIndex);
				dragData.IsHovering = dragData.Item.IsDragged = true;
				dropInfo.Effects = DragDropEffects.Move;
			}
		}

		public void DragLeave(IDropInfo dropInfo)
		{
			((DragData)dropInfo.Data).IsHovering = false;
			dropInfo.Effects = DragDropEffects.None;
		}

		public void Drop(IDropInfo dropInfo)
		{
			DragData dragData = (DragData)dropInfo.Data;
			dragData.IsHovering = dragData.Item.IsDragged = false;
		}
		#endregion


		private class DragData
		{
			private bool isHovering;
			private CancellationTokenSource restoreItemPositionTaskToken = null;

			public DragData(IDraggableRowItem item)
			{
				Item = item;
				OriginalIndex = item.RowIndex;
			}

			public IDraggableRowItem Item { get; private set; }
			public int OriginalIndex { get; private set; }

			public bool IsHovering
			{
				get { return isHovering; }
				set
				{
					if (restoreItemPositionTaskToken != null)
					{
						restoreItemPositionTaskToken.Cancel();
						restoreItemPositionTaskToken = null;
					}

					if ((!(isHovering = value)) && (Item.IsDragged))
					{
						restoreItemPositionTaskToken = new CancellationTokenSource();
						Task.Factory.StartNew((token) =>
						{
							Thread.Sleep(50);
							if (!((CancellationToken)token).IsCancellationRequested)
							{
								Item.IsDragged = false;
								Item.MoveToIndex(OriginalIndex);
							}
						}, restoreItemPositionTaskToken.Token, restoreItemPositionTaskToken.Token);
					}
				}
			}
		}
	}
}
