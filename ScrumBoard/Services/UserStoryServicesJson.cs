using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ScrumBoard.util;
using ScrumBoardLib.model;

namespace ScrumBoard.Services
{
    public class UserStoryServicesJson:IUserStoryService
    {
        private const String FileName = @"data\UserStory.json";
        private List<UserStory> _dataBuffer = null;
        private static int nextId = 0;

        public UserStoryServicesJson()
        {
            _dataBuffer = JsonFileReader.ReadJsonGeneric<UserStory>(FileName);

            // hack
            if (_dataBuffer.Count == 0)
            {
                _dataBuffer.Add(new UserStory(1,"create", "As user i would like to ..."));
                _dataBuffer.Add(new UserStory(2,"Get", "As user i would like to ..."));
                _dataBuffer.Add(new UserStory(3,"Delete", "As user i would like to ..."));
                _dataBuffer.Add(new UserStory(4,"Modify", "As user i would like to ..."));

                SaveChanges();
            }

            nextId = _dataBuffer.Max(u => u.Id) + 1;
        }


        public List<UserStory> GetAll()
        {
            return new List<UserStory>(_dataBuffer);
        }

        public UserStory GetById(int id)
        {
            UserStory us = _dataBuffer.Find(u => u.Id == id);
            if (us == null)
            {
                throw new KeyNotFoundException();
            }

            return us;
        }

        public UserStory Create(UserStory newUserStory)
        {
            newUserStory.Id = nextId++;
            _dataBuffer.Add(newUserStory);

            SaveChanges(); // remember to save changes to the json file
            return newUserStory;
        }

        public UserStory Delete(int id)
        {
            UserStory us = GetById(id);
            _dataBuffer.Remove(us);

            SaveChanges(); // remember to save changes to the json file
            return us;
        }

        public UserStory Modify(UserStory modifiedUserStory)
        {
            int ix = _dataBuffer.FindIndex(u => u.Id == modifiedUserStory.Id);
            if (ix == -1)
            {
                throw new KeyNotFoundException();
            }

            _dataBuffer[ix] = modifiedUserStory;

            SaveChanges(); // remember to save changes to the json file
            return modifiedUserStory;
        }

        private void SaveChanges()
        {
            JsonFileWriter.WriteToJsonGeneric(_dataBuffer, FileName);
        }
    }
}
