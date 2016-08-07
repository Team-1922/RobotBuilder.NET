﻿using System;
using Team1922.MVVM.Contracts;
using Team1922.MVVM.Framework;
using Team1922.MVVM.Models;

namespace Team1922.MVVM.ViewModels
{
    internal class ContinuousCommandViewModel : ViewModelBase, IContinuousCommandProvider
    {
        ContinuousCommand _commandModel;

        public ContinuousCommandViewModel()
        {
            _eventTargetProvider.PropertyChanged += _eventTargetProvider_PropertyChanged;
        }

        private void _eventTargetProvider_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged($"EventTarget.{e.PropertyName}");
        }

        public IEventTargetProvider EventTarget
        {
            get
            {
                return _eventTargetProvider;
            }
        }

        public string Name
        {
            get
            {
                return _commandModel.Name;
            }

            set
            {
                var temp = _commandModel.Name;
                SetProperty(ref temp, value);
                _commandModel.Name = temp;
            }
        }

        public override string this[string key]
        {
            get
            {
                switch(key)
                {
                    case "Name":
                        return Name;
                    case "EventTarget":
                        return "EventTarget";
                    default:
                        throw new ArgumentException($"\"{key}\" Is Inaccessible or Does Not Exist");
                }
            }

            set
            {
                switch(key)
                {
                    case "Name":
                        Name = value;
                        break;
                    default:
                        throw new ArgumentException($"\"{key}\" is Read-Only or Does Not Exist");
                }
            }
        }

        public void SetContinuousCommand(ContinuousCommand continuousCommand)
        {
            _commandModel = continuousCommand;
            _eventTargetProvider.SetEventTarget(_commandModel.EventTarget);
        }

        #region Private Fields
        EventTargetViewModel _eventTargetProvider = new EventTargetViewModel();
        #endregion
    }
}