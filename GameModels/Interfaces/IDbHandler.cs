﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerKaLoop.GameModels.Interfaces
{
    public interface  IDbHandler<T>
    {
        T Find(string id);
        List<T> FindAll();
        T Create(T item);
        T Update(string id, T item);
        void Remove(string id);
    }
}
