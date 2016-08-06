﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Team1922.MVVM.Framework
{
    public interface INamedEnumerable<T> : IEnumerable<T>
    {
        string Name { get; }
    }
}
