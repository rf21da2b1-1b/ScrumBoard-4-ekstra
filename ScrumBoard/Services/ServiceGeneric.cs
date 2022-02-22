using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrumBoard.util;
using ScrumBoardLib.model;

namespace ScrumBoard.Services
{
    public class ServiceGeneric<T>:IServiceGeneric<T> where T : IModelWithId
    {
        private readonly String FileName = @"data.json";
        private List<T> _dataBuffer = null;
        private static int nextId = 0;

        public ServiceGeneric(String filename = null)
        {
            if (filename != null)
            {
                FileName = filename;
            }
            _dataBuffer = JsonFileReader.ReadJsonGeneric<T>(FileName);

            // set next id to max id in buffer or 1 if buffer is empty
            nextId = (_dataBuffer.Count > 0)? _dataBuffer.Max(u => u.Id) + 1 : 1;
        }

        public List<T> GetAll()
        {
            return new List<T>(_dataBuffer);
        }

        public T GetById(int id)
        {
            T t = _dataBuffer.Find(u => u.Id == id);
            if (t == null)
            {
                throw new KeyNotFoundException();
            }

            return t;
        }

        public T Create(T newElement)
        {
            newElement.Id = nextId++;
            _dataBuffer.Add(newElement);

            SaveChanges(); // remember to save changes to the json file
            return newElement;
        }

        public T Delete(int id)
        {
            T t = GetById(id);
            _dataBuffer.Remove(t);

            SaveChanges(); // remember to save changes to the json file
            return t;
        }

        public T Modify(T modifiedElement)
        {
            int ix = _dataBuffer.FindIndex(u => u.Id == modifiedElement.Id);
            if (ix == -1)
            {
                throw new KeyNotFoundException();
            }

            _dataBuffer[ix] = modifiedElement;

            SaveChanges(); // remember to save changes to the json file
            return modifiedElement;
        }

        private void SaveChanges()
        {
            JsonFileWriter.WriteToJsonGeneric(_dataBuffer, FileName);
        }
    }
}
