﻿using System;
using System.Collections.Generic;
using Team1922.MVVM.Contracts;
using Team1922.MVVM.Framework;
using Team1922.MVVM.Models;

namespace Team1922.MVVM.ViewModels
{
    //This is broken in a few ways; namely not being bound to the IO service at all
    internal class PIDControllerSRXViewModel : ViewModelBase<PIDControllerSRX>, IPIDControllerSRXProvider
    {
        public PIDControllerSRXViewModel(ICANTalonProvider parent) : base(parent)
        {
        }

        #region IPIDControllerSRXProvider
        public int AllowableCloseLoopError
        {
            get
            {
                return ModelReference.AllowableCloseLoopError;
            }

            set
            {
                var temp = ModelReference.AllowableCloseLoopError;
                SetProperty(ref temp, value);
                ModelReference.AllowableCloseLoopError = temp;
            }
        }

        public double CloseLoopRampRate
        {
            get
            {
                return ModelReference.CloseLoopRampRate;
            }

            set
            {
                var temp = ModelReference.CloseLoopRampRate;
                SetProperty(ref temp, value);
                ModelReference.CloseLoopRampRate = temp;
            }
        }

        public double P
        {
            get
            {
                return ModelReference.P;
            }

            set
            {
                var temp = ModelReference.P;
                SetProperty(ref temp, value);
                ModelReference.P = temp;
            }
        }

        public double I
        {
            get
            {
                return ModelReference.I;
            }

            set
            {
                var temp = ModelReference.I;
                SetProperty(ref temp, value);
                ModelReference.I = temp;
            }
        }

        public double D
        {
            get
            {
                return ModelReference.D;
            }

            set
            {
                var temp = ModelReference.D;
                SetProperty(ref temp, value);
                ModelReference.D = temp;
            }
        }

        public double F
        {
            get
            {
                return ModelReference.F;
            }

            set
            {
                var temp = ModelReference.F;
                SetProperty(ref temp, value);
                ModelReference.F = temp;
            }
        }

        public int IZone
        {
            get
            {
                return ModelReference.IZone;
            }

            set
            {
                var temp = ModelReference.IZone;
                SetProperty(ref temp, value);
                ModelReference.IZone = temp;
            }
        }

        public CANTalonDifferentiationLevel SourceType
        {
            get
            {
                return ModelReference.SourceType;
            }

            set
            {
                var temp = ModelReference.SourceType;
                SetProperty(ref temp, value);
                ModelReference.SourceType = temp;
            }
        }
        #endregion

        #region IProvider
        public string Name
        {
            get
            {
                return "PID Controller";
            }
        }
        #endregion

        #region ViewModelBase
        protected override string GetValue(string key)
        {
            switch (key)
            {
                case "Name":
                    return Name;
                case "AllowableCloseLoopError":
                    return AllowableCloseLoopError.ToString();
                case "CloseLoopRampRate":
                    return CloseLoopRampRate.ToString();
                case "P":
                    return P.ToString();
                case "I":
                    return I.ToString();
                case "D":
                    return D.ToString();
                case "F":
                    return F.ToString();
                case "IZone":
                    return IZone.ToString();
                case "SourceType":
                    return SourceType.ToString();
                default:
                    throw new ArgumentException($"\"{key}\" Is Inaccessible or Does Not Exist");
            }
        }

        protected override void SetValue(string key, string value)
        {
            switch (key)
            {
                case "AllowableCloseLoopError":
                    AllowableCloseLoopError = SafeCastInt(value);
                    break;
                case "CloseLoopRampRate":
                    CloseLoopRampRate = SafeCastDouble(value);
                    break;
                case "P":
                    P = SafeCastDouble(value);
                    break;
                case "I":
                    I = SafeCastDouble(value);
                    break;
                case "D":
                    D = SafeCastDouble(value);
                    break;
                case "F":
                    F = SafeCastDouble(value);
                    break;
                case "IZone":
                    IZone = SafeCastInt(value);
                    break;
                case "SourceType":
                    SourceType = SafeCastEnum<CANTalonDifferentiationLevel>(value);
                    break;
                default:
                    throw new ArgumentException($"\"{key}\" is Read-Only or Does Not Exist");
            }

        }

        public override string ModelTypeName
        {
            get
            {
                var brokenName = ModelReference.GetType().ToString().Split('.');
                return brokenName[brokenName.Length - 1];
            }
        }
        #endregion
    }
}