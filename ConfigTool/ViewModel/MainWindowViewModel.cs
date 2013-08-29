using ConfigTool.Services;
using ReactiveUI;
using ReactiveUI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConfigTool.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        private List<ConfigurationItem> _localCache;

        private bool _IsBusy;

        private ReactiveList<ConfigurationItem> _ConfigurationList;

        private string searchFilter;
        private bool _IsNotBusy;

        public ReactiveCommand Search { get; protected set; }

        public ReactiveCommand Refresh { get; protected set; }


        public MainWindowViewModel()
        {
            this.IsBusy = false;
            this.ConfigurationList = new ReactiveList<ConfigurationItem>();

            var canExecuteSearch = Observable.Return<bool>(!this.IsBusy && this.ConfigurationList != null && this.ConfigurationList.Count > 0);
            var canExecuteRefresh = Observable.Return<bool>(!this.IsBusy);

            // don't want to execute the search if we're still populating the list
            this.Search = new ReactiveCommand(canExecuteSearch);

            this.Search.Subscribe(s => IsBusy = true);

            this.Search.RegisterAsyncFunction(_ =>
            {
                // ToList forces execution off thread.
                return _localCache.FindAll(s => s.Name.IndexOf(this.searchFilter, StringComparison.OrdinalIgnoreCase) >= 0);
            }).Subscribe(items =>
            {
                UpdateConfigurationList(items);
                this.IsBusy = false;
            });


            this.Refresh = new ReactiveCommand(canExecuteRefresh);

            this.Refresh.Subscribe(s => IsBusy = true);

            this.Refresh.RegisterAsyncFunction(_ =>
            {
                ConfigurationService cs = new ConfigurationService();
                return cs.GetAllConfigurations();
            }).Subscribe(items =>
            {
                _localCache = items;
                UpdateConfigurationList(items);
                this.IsBusy = false;
            });

           
            // Execute a filter search every time someone types in the filter box
            this.ObservableForProperty(s => s.SearchFilter)
                .Throttle(TimeSpan.FromMilliseconds(800), RxApp.MainThreadScheduler)
                .Select(s => s.Value).DistinctUntilChanged()
                .Where(s => _localCache != null && this._localCache.Count > 0)
                .Subscribe(s => 
                {
                    Search.Execute(s);
                });
                


        }

        private void UpdateConfigurationList(List<ConfigurationItem> configs)
        {
            this.ConfigurationList.Clear();
            foreach (var item in configs)
            {
                this.ConfigurationList.Add(item);
            }
        }
        

        public bool IsBusy
        {
            get 
            { 
                return _IsBusy; 
            }
            set 
            {
                this.RaiseAndSetIfChanged(ref this._IsBusy, value);
                this.IsNotBusy = !this.IsBusy;
            }
        }

        public bool IsNotBusy
        {
            get
            {
                return _IsNotBusy;
            }
            protected set
            {
                this.RaiseAndSetIfChanged(ref this._IsNotBusy, value);
            }
        }


        public ReactiveList<ConfigurationItem> ConfigurationList
        {
            get
            {
                return _ConfigurationList;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this._ConfigurationList, value);
            }
        }

        

        public string SearchFilter
        {
            get
            {
                return searchFilter;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref this.searchFilter, value);
            }
        }
    }
}
