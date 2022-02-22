using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ScrumBoardLib.model;

namespace ScrumBoard.Services
{
    public interface IServiceGeneric<T> where T : IModelWithId
    {
        List<T> GetAll();
        T GetById(int id);
        T Create(T newElement);
        T Delete(int id);
        T Modify(T modifiedElement);

    }
}
