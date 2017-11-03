using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel
{
	public class TemplateTreeViewModel : INotifyPropertyChanged
	{
		private ICommand refreshCommand;

		private readonly ObservableCollection<TemplateTreeItemViewModel> list =
			new ObservableCollection<TemplateTreeItemViewModel>();

		private bool hasError;
		private string errorText;

		public TemplateTreeViewModel(ITemplateSource source)
		{
			templateSource = source;
			List.Clear();
		}

		public ObservableCollection<TemplateTreeItemViewModel> List
		{
			get { return list; }
		}

		public bool Loading { get; set; }

		public bool CanRefresh
		{
			get { return templateSource.CanRefresh; }
		}

		public ITemplateSource templateSource { get; set; }

		public ICommand RefreshCommand
		{
			get
			{
				if (null == refreshCommand)
					refreshCommand = new DelegateCommand(ExecuteRefreshCommand, CanExecuteRefreshCommand);

				return refreshCommand;
			}
		}

		public bool HasError
		{
			get { return hasError; }
			set
			{
				if (hasError != value)
				{
					hasError = value;
					OnPropertyChanged("HasError");
				}
			}
		}

		public string ErrorText
		{
			get { return errorText; }
			set
			{
				if (errorText != value)
				{
					errorText = value;
					OnPropertyChanged("ErrorText");
				}
			}
		}

		public void Load()
		{
			Loading = true;
			List.Clear();
			HasError = false;
			ErrorText = "";
			List.Add(new TemplateTreeItemViewModel("Loading..."));

			Task.Factory.StartNew<List<TemplateTreeItemViewModel>>(LoadAsync)
				.ContinueWith(previousTask =>
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						List.Clear();
						if (!String.IsNullOrEmpty(templateSource.Error))
						{
							HasError = true;
							ErrorText = templateSource.Error;
						}
						else
							foreach (TemplateTreeItemViewModel item in previousTask.Result)
								List.Add(item);

						Loading = false;
					});
				});
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private List<TemplateTreeItemViewModel> LoadAsync()
		{
			var returnList = new List<TemplateTreeItemViewModel>();
			var folderDictionary = new Dictionary<string, TemplateTreeItemViewModel>();

			foreach (Template template in  templateSource.Load())
			{
				string folderName = "";
				TemplateTreeItemViewModel folder = null;
				if (!String.IsNullOrEmpty(template.Path))
					foreach (string pathSection in  template.Path.Split('.'))
					{
						folderName += pathSection;

						if (!folderDictionary.ContainsKey(folderName))
						{
							folderDictionary[folderName] = new TemplateTreeItemViewModel(pathSection, folder);
							if (folder == null)
								returnList.Add(folderDictionary[folderName]);
							else
								folder.Children.Add(folderDictionary[folderName]);
						}


						folder = folderDictionary[folderName];

						folderName += ".";
					}
				folderName += template.Name;
				if (folderDictionary.ContainsKey(folderName))
				{
					folderDictionary[folderName].Template = template;
				}
				else
				{
					var viewModel = new TemplateTreeItemViewModel(template, folder);
					if (folder != null)
						folder.Children.Add(viewModel);
					else
					{
						folderDictionary.Add(folderName, viewModel);
						returnList.Add(viewModel);
					}
				}
			}
			return returnList;
		}

		private bool CanExecuteRefreshCommand()
		{
			return !Loading && !templateSource.RefreshDisabled;
		}

		private void ExecuteRefreshCommand()
		{
			Load();
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}