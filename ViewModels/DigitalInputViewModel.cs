﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team1922.MVVM.Contracts;
using Team1922.MVVM.Framework;
using Team1922.MVVM.Models;
using Team1922.MVVM.Services;

namespace Team1922.MVVM.ViewModels
{
    internal class DigitalInputViewModel : ViewModelBase, IDigitalInputProvider
    {
        DigitalInput _digitalInputModel;

        public int ID
        {
            get
            {
                return _digitalInputModel.ID;
            }

            set
            {
                var temp = _digitalInputModel.ID;
                SetProperty(ref temp, value);
                _digitalInputModel.ID = temp;
            }
        }

        public string Name
        {
            get
            {
                return _digitalInputModel.Name;
            }

            set
            {
                var temp = _digitalInputModel.Name;
                SetProperty(ref temp, value);
                _digitalInputModel.Name = temp;
            }
        }

        public bool Value
        {
            get
            {
                return _digitalInputModel.Value;
            }

            private set
            {
                var temp = _digitalInputModel.Value;
                SetProperty(ref temp, value);
                _digitalInputModel.Value = temp;
            }
        }

        public void SetDigitalInput(DigitalInput digitalInput)
        {
            _digitalInputModel = digitalInput;
        }

        public void UpdateInputValues()
        {
            Value = IOService.Instance.DigitalInputs[ID].ValueAsBool;
        }
    }
}
