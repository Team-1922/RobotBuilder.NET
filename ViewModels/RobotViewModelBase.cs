﻿using Team1922.MVVM.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team1922.MVVM.Framework;
using Team1922.MVVM.Models;
using System.Windows.Input;
using System.ComponentModel;
using Team1922.MVVM.Contracts;

namespace Team1922.MVVM.ViewModels
{
    public abstract class RobotViewModelBase : ViewModelBase, IRobotProvider
    {
        protected Robot _robotModel;

        public RobotViewModelBase()
        {
            //GetBooksCommand = new DelegateCommand(OnGetBooks, CanGetBooks);
            AddSubsystemCommand = new DelegateCommand(OnAddSubsystem);
            AddEventHandlerCommand = new DelegateCommand(OnAddEventHandler);
            AddJoystickCommand = new DelegateCommand(OnAddJoystick);

            _subsystemProviders = new CompoundProviderList<ISubsystemProvider>("Subsystems");
            _eventHandlerProviders = new CompoundProviderList<IEventHandlerProvider>("Event Handlers");
            _joystickProviders = new CompoundProviderList<IJoystickProvider>("Joysticks");
        }

        public void SetRobot(Robot robot)
        {
            //clear the old providers
            _subsystemProviders.Items.Clear();
            _eventHandlerProviders.Items.Clear();
            _joystickProviders.Items.Clear();

            //setup the new providers
            _robotModel = robot;
            if (null != _robotModel.Subsystem)
            {
                foreach (var subsystem in _robotModel.Subsystem)
                {
                    if (subsystem == null)
                        continue;
                    var provider = new SubsystemViewModel();
                    provider.SetSubsystem(subsystem);
                    _subsystemProviders.Items.Add(provider);
                }
            }
            if (null != _robotModel.EventHandler)
            {
                foreach (var onWithinRangeEventHandler in _robotModel.EventHandler)
                {
                    if (onWithinRangeEventHandler == null)
                        continue;
                    var provider = new EventHandlerViewModel();
                    provider.SetEventHandler(onWithinRangeEventHandler);
                    _eventHandlerProviders.Items.Add(provider);
                }
            }
            if (null != _robotModel.Joystick)
            {
                foreach (var joystick in _robotModel.Joystick)
                {
                    if (joystick == null)
                        continue;
                    var provider = new JoystickViewModel();
                    provider.SetJoystick(joystick);
                    _joystickProviders.Items.Add(provider);
                }
            }
        }

        protected override List<string> GetOverrideKeys()
        {
            return _keys;
        }

        private INotifyPropertyChanged _selectedElement;
        public INotifyPropertyChanged SelectedElement
        {
            get { return _selectedElement; }
            set
            {
                if (SetProperty(ref _selectedElement, value))
                {
                    EventAggregator<ItemSelectEvent>.Instance.Publish(this, new ItemSelectEvent { SelectedElement = _selectedElement });
                }
            }
        }

        public IEnumerable<ISubsystemProvider> Subsystems
        {
            get { return _subsystemProviders.Items; }
        }
        public IEnumerable<IEventHandlerProvider> EventHandlers
        {
            get { return _eventHandlerProviders.Items; }
        }
        public IEnumerable<IJoystickProvider> Joysticks
        {
            get { return _joystickProviders.Items; }
        }

        private void OnAddSubsystem()
        {
            EventAggregator<AddSubsystemEvent>.Instance.Publish(this, new AddSubsystemEvent());
        }
        private void OnAddEventHandler()
        {
            EventAggregator<AddEventHandlerEvent>.Instance.Publish(this, new AddEventHandlerEvent());
        }
        private void OnAddJoystick()
        {
            EventAggregator<AddJoystickEvent>.Instance.Publish(this, new AddJoystickEvent());
        }
        private void OnAddAnalogInput()
        {
            if (_selectedElement is Subsystem)
            {
                EventAggregator<AddAnalogInputEvent>.Instance.Publish(this, new AddAnalogInputEvent(_selectedElement as Subsystem));
            }
        }
        private void OnAddCANTalon()
        {
            if (_selectedElement is Subsystem)
            {
                EventAggregator<AddCANTalonEvent>.Instance.Publish(this, new AddCANTalonEvent(_selectedElement as Subsystem));
            }
        }
        private void OnAddDigitalInput()
        {
            if (_selectedElement is Subsystem)
            {
                EventAggregator<AddDigitalInputEvent>.Instance.Publish(this, new AddDigitalInputEvent(_selectedElement as Subsystem));
            }
        }
        private void OnAddPWMMotorController()
        {
            if (_selectedElement is Subsystem)
            {
                EventAggregator<AddPWMMotorControllerEvent>.Instance.Publish(this, new AddPWMMotorControllerEvent(_selectedElement as Subsystem));
            }
        }
        private void OnAddQuadEncoder()
        {
            if (_selectedElement is Subsystem)
            {
                EventAggregator<AddQuadEncoderEvent>.Instance.Publish(this, new AddQuadEncoderEvent(_selectedElement as Subsystem));
            }
        }

        public void UpdateInputValues()
        {
            foreach(var subsystem in _subsystemProviders.Items)
            {
                subsystem.UpdateInputValues();
            }
        }

        public ICommand AddSubsystemCommand { get; }
        public ICommand AddEventHandlerCommand { get; }
        public ICommand AddJoystickCommand { get; }

        public int TeamNumber
        {
            get
            {
                return _robotModel.TeamNumber;
            }

            set
            {
                _robotModel.TeamNumber = value;
            }
        }

        public int AnalogInputSampleRate
        {
            get
            {
                return _robotModel.AnalogInputSampleRate;
            }

            set
            {
                _robotModel.AnalogInputSampleRate = value;
            }
        }

        public IEnumerable<IProvider> Children
        {
            get
            {
                return _children.Values;
            }
        }

        public string Name
        {
            get
            {
                return "Robot";
            }
        }

        protected override string GetValue(string key)
        {
            switch(key)
            {
                case "AnalogInputSampleRate":
                    return AnalogInputSampleRate.ToString();
                case "Joysticks":
                    return Joysticks.ToString();
                case "Name":
                    return Name;
                case "EventHandlers":
                    return EventHandlers.ToString();
                case "Subsystems":
                    return Subsystems.ToString();
                case "TeamNumber":
                    return TeamNumber.ToString();
                default:
                    throw new ArgumentException($"\"{key}\" Is Inaccessible or Does Not Exist");
            }
        }

        protected override void SetValue(string key, string value)
        {
            switch (key)
            {
                case "AnalogInputSampleRate":
                    AnalogInputSampleRate = SafeCastInt(value);
                    break;
                case "TeamNumber":
                    TeamNumber = SafeCastInt(value);
                    break;
                default:
                    throw new ArgumentException($"\"{key}\" is Read-Only or Does Not Exist");
            }
            
        }

        #region Private Fields
        Dictionary<string, IProvider> _children = new Dictionary<string, IProvider>();
        readonly List<string> _keys = new List<string>(){ "AnalogInputSampleRate", "ContinuousCommands", "Joysticks", "Name", "OnChangeEventHandlers","OnWithinRangeEventHandlers","Subsystems","TeamNumber" };
        CompoundProviderList<ISubsystemProvider> _subsystemProviders
        {
            get
            {
                return _children["_subsystemProviders"] as CompoundProviderList<ISubsystemProvider>;
            }

            set
            {
                _children["_subsystemProviders"] = value;
            }
        }
        CompoundProviderList<IEventHandlerProvider> _eventHandlerProviders
        {
            get
            {
                return _children["_eventHandlerProviders"] as CompoundProviderList<IEventHandlerProvider>;
            }

            set
            {
                _children["_eventHandlerProviders"] = value;
            }
        }
        CompoundProviderList<IJoystickProvider> _joystickProviders
        {
            get
            {
                return _children["_joystickProviders"] as CompoundProviderList<IJoystickProvider>;
            }

            set
            {
                _children["_joystickProviders"] = value;
            }
        }
        #endregion
    }
}
