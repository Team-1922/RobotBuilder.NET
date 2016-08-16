﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Team1922.MVVM.Contracts;
using Team1922.MVVM.Models;

namespace Team1922.MVVM.Services
{
    internal class DataAccess : IDataAccessService
    {
        public IHierarchialAccess DataInstance { get; set; }

        public void AssertPath(string path)
        {
            var temp = DataInstance[path];
        }
        public bool ThrowsExceptions
        {
            get
            {
                return TypeRestrictions.ThrowsExceptionsOnValidationFailure;
            }
            
            set
            {
                TypeRestrictions.ThrowsExceptionsOnValidationFailure = value;
            }
        }

        public bool ClampingValues { get; set; }
    }
}
