using Interop.Infrastructure.Events;
using Interop.Infrastructure.Interfaces;
using Interop.Infrastructure.Models;

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Interop.Modules.Targets.ViewModels
{
    public class TargetsViewModel : BindableBase
    {
        IEventAggregator _eventAggregator;
        ITargetService _targetService;
        public DelegateCommand DeleteTargetCommand { get; private set; }

        public TargetsViewModel(IEventAggregator eventAggregator, ITargetService targetService)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException("eventAggregator");
            }

            if (targetService == null)
            {
                throw new ArgumentNullException("targetService");
            }
            _eventAggregator = eventAggregator;
            _targetService = targetService;

            this.DeleteTargetCommand = new DelegateCommand(this.DeleteTarget, this.CanDeleteTarget);

            _eventAggregator.GetEvent<UpdateTargetsEvent>().Subscribe(Update_Targets);
        }
        
        public void Update_Targets(List<Target> targets)
        {
            Targets = targets;

            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                this.DeleteTargetCommand.RaiseCanExecuteChanged();
            });
        }
        
        IList<Target> _targets = new List<Target>();
        public IList<Target> Targets
        {
            get
            {
                return _targets.OrderByDescending(t => t.id).ToList();
            }
            set
            {
                TargetEqualityComparer TargetEqC = new TargetEqualityComparer();
                if (!ScrambledEquals(Targets.OrderBy(t => t), value.OrderBy(t => t), TargetEqC))
                //if(!Enumerable.SequenceEqual(_targets.OrderBy(t => t.id), value.OrderBy(t => t.id),TargetEqC))
                {                    
                    if (SetProperty(ref _targets, value))
                    {
                        //this.OnPropertyChanged(() => this.);
                    }
                }
            }
        }

        private void DeleteTarget()
        {
            if(_targets.Contains(CurrentTarget))
            { 
                _eventAggregator.GetEvent<DeleteTargetEvent>().Publish(CurrentTarget.id);
            }
        }

        private bool CanDeleteTarget()
        {
            return (this._targets.Count > 0);
        }

        public Target _currentTarget = null;
        public Target CurrentTarget
        {
            get
            {
                return _currentTarget;
            }
            set
            {

                if (SetProperty(ref _currentTarget, value))
                {
                    _eventAggregator.GetEvent<TargetImagesEvent>().Subscribe(delegate (ConcurrentDictionary<int, byte[]> dictBytesImage)
                    {
                        if (dictBytesImage.Keys.Contains(CurrentTarget.id))
                        {
                            var currentImage = dictBytesImage[CurrentTarget.id];

                            DisplayedImage = (BitmapSource)new ImageSourceConverter().ConvertFrom(currentImage);

                            //using (var ms = new System.IO.MemoryStream(currentImage))
                            //{
                            //    var image = new BitmapImage();
                            //    image.BeginInit();
                            //    image.CacheOption = BitmapCacheOption.OnLoad; // here
                            //    image.StreamSource = ms;
                            //    image.EndInit();
                            //    DisplayedImage = image;
                            //}
                        }
                    });
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        ImageSource _displayedImage;
        public ImageSource DisplayedImage
        {
            get { return this._displayedImage; }
            set
            {
                if (SetProperty(ref _displayedImage, value))
                {
                    //this.OnPropertyChanged(() => this.);
                }
            }
        }

        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2, IEqualityComparer<T> comparer)
        {
            var cnt = new Dictionary<T, int>(comparer);
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }

        class TargetEqualityComparer : IEqualityComparer<Target>
        {
            public bool Equals(Target b1, Target b2)
            {
                if (b2 == null && b1 == null)
                    return true;
                else if (b1 == null | b2 == null)
                    return false;
                else if (b1.user == b2.user &
                         b1.type == b2.type &
                         b1.shape == b2.shape &
                         b1.orientation == b2.orientation &
                         b1.longitude == b2.longitude &
                         b1.latitude == b2.latitude &
                         b1.id == b2.id &
                         b1.description == b2.description &
                         b1.background_color == b2.background_color &
                         b1.autonomous == b2.autonomous &
                         b1.alphanumeric == b1.alphanumeric &
                         b1.alphanumeric_color == b2.alphanumeric_color)
                    return true;
                else
                    return false;
            }

            public int GetHashCode(Target target)
            {
                int hCode = target.id;

                return hCode.GetHashCode();
            }
        }
    }    
}
