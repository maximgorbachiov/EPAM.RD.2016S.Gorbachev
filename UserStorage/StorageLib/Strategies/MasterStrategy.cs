using System;
using System.Collections.Generic;
using StorageLib.Entities;
using StorageLib.Interfaces;
using StorageLib.EventArguments;
using FibonachyGenerator.Interfaces;

namespace StorageLib.Strategies
{
    public class MasterStrategy : Strategy
    {
        private IValidator validator;
        private IGeneratorId generator;

        public event EventHandler<AddEventArgs> OnAddUser = delegate { };
        public event EventHandler<DeleteEventArgs> OnDeleteUser = delegate { };

        public MasterStrategy(List<User> users, IValidator validator, IGeneratorId generator) : base(users)
        {
            this.validator = validator;
            this.generator = generator;
        }

        public override int Add(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException(nameof(user));
            }

            if (!validator.Validate(user))
            {
                throw new ArgumentException(nameof(user));
            }

            if (!generator.MoveNext())
            {
                return 0;
            }

            user.Id = generator.Current;
            users.Add(user);
            OnAddUserEvent(new AddEventArgs{User =  user });

            return user.Id;
        }

        public override void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException(nameof(id));
            }

            users.Remove(users.Find(user => user.Id == id));
            OnDeleteUserEvent(new DeleteEventArgs { Id = id });
        }

        protected virtual void OnAddUserEvent(AddEventArgs e)
        {
            if (OnAddUser != null)
            {
                OnAddUser(this, e);
            }
        }

        protected virtual void OnDeleteUserEvent(DeleteEventArgs e)
        {
            if (OnDeleteUser != null)
            {
                OnDeleteUser(this, e);
            }
        }
    }
}
