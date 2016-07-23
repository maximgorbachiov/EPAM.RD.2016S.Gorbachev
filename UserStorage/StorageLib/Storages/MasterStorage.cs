using StorageInterfaces.Entities;
using StorageInterfaces.EventArguments;
using StorageInterfaces.IGenerators;
using StorageInterfaces.IRepositories;
using StorageInterfaces.IStorages;
using StorageInterfaces.IValidators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Storage.Storages
{
    public class MasterStorage : IMasterStorage
    {
        private readonly IValidator validator;
        private readonly IGenerator generator;
        private readonly IRepository repository;

        private List<User> users = new List<User>();

        public List<User> Users
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<AddEventArgs> OnAddUser = delegate { };
        public event EventHandler<DeleteEventArgs> OnDeleteUser = delegate { };

        public MasterStorage(IValidator validator, IGenerator generator, IRepository repository)
        {
            this.validator = validator;
            this.generator = generator;
            this.repository = repository;
        }

        public int AddUser(User user)
        {
            if (generator == null)
            {
                throw new NullReferenceException(nameof(generator));
            }

            if (user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            if (!validator.Validate(user))
            {
                throw new ArgumentException(nameof(user));
            }

            user.Id = generator.Current;
            users.Add(user);
            OnAddUserEvent(new AddEventArgs { User = user });

            return user.Id;
        }

        public void DeleteUser(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            users.Remove(users.Find(user => user.Id == id));
            OnDeleteUserEvent(new DeleteEventArgs { Id = id });
        }

        public List<int> SearchBy(IComparer<User> comparator, User searchingUser)
        {
            if (comparator == null)
            {
                throw new NullReferenceException(nameof(comparator));
            }

            if (searchingUser == null)
            {
                throw new NullReferenceException(nameof(searchingUser));
            }

            return users.Where(user => comparator.Compare(user, searchingUser) == 0)
                .Select(user => user.Id).ToList();
        }

        public void Load()
        {
            ServiceState lastState = repository.Load();
            users = lastState.Users;
            generator.SetGeneratorState(lastState.LastId);
        }

        public void Save()
        {
            repository.Save(new ServiceState { Users = users, LastId = generator.Current });
        }

        protected virtual void OnAddUserEvent(AddEventArgs e)
        {
            OnAddUser.Invoke(this, e);
        }

        protected virtual void OnDeleteUserEvent(DeleteEventArgs e)
        {
            OnDeleteUser.Invoke(this, e);
        }
    }
}
