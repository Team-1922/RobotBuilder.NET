﻿using System;
using Team1922.MVVM.Contracts;
using Team1922.MVVM.Framework;
using Team1922.MVVM.Models;
using Team1922.MVVM.Services;

namespace Team1922.MVVM.ViewModels
{
    internal class RelayOutputViewModel : ViewModelBase, IRelayOutputProvider
    {
        RelayOutput _relayOutputModel;

        public RelayDirection Direction
        {
            get
            {
                return _relayOutputModel.Direction;
            }

            set
            {
                var temp = _relayOutputModel.Direction;
                SetProperty(ref temp, value);
                _relayOutputModel.Direction = temp;
            }
        }

        public int ID
        {
            get
            {
                return _relayOutputModel.ID;
            }

            set
            {
                var temp = _relayOutputModel.ID;
                SetProperty(ref temp, value);
                _relayOutputModel.ID = temp;
            }
        }

        public string Name
        {
            get
            {
                return _relayOutputModel.Name;
            }

            set
            {
                var temp = _relayOutputModel.Name;
                SetProperty(ref temp, value);
                _relayOutputModel.Name = temp;
            }
        }

        public RelayValue Value
        {
            get
            {
                return _relayOutputModel.Value;
            }

            set
            {
                var ioService = IOService.Instance.RelayOutputs[ID];
                switch (value)
                {
                    case RelayValue.Forward:
                        if (Direction == RelayDirection.ForwardOnly || Direction == RelayDirection.Both)
                        {
                            ioService.ValueAsBool = true;
                            ioService.ReverseValueAsBool = false;
                        }
                        break;
                    case RelayValue.Reverse:
                        if (Direction == RelayDirection.ReverseOnly || Direction == RelayDirection.Both)
                        {
                            ioService.ValueAsBool = false;
                            ioService.ReverseValueAsBool = true;
                        }
                        break;
                    case RelayValue.Off:
                        ioService.ValueAsBool = false;
                        ioService.ReverseValueAsBool = false;
                        break;
                    case RelayValue.On:
                        if (Direction == RelayDirection.ForwardOnly)
                        {
                            ioService.ValueAsBool = true;
                            ioService.ReverseValueAsBool = false;
                        }
                        else if (Direction == RelayDirection.ReverseOnly)
                        {
                            ioService.ValueAsBool = false;
                            ioService.ReverseValueAsBool = true;
                        }
                        else
                        {
                            ioService.ValueAsBool = true;
                            ioService.ReverseValueAsBool = true;
                        }
                        break;
                }
                var temp = _relayOutputModel.Value;
                SetProperty(ref temp, value);
                _relayOutputModel.Value = temp;
            }
        }

        public void SetRelayOutput(RelayOutput relayOutput)
        {
            _relayOutputModel = relayOutput;
        }
    }
}