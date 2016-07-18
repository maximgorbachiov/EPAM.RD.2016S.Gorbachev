using System;
using System.Collections.Generic;
using StorageLib.Entities;
using StorageLib.EventArguments;

namespace StorageLib.Strategies
{
    public class SlaveStrategy : Strategy
    {
        public SlaveStrategy(List<User> users, MasterStrategy master) : base(users)
        {
            master.OnAddUser += AddUserHandler;
            master.OnDeleteUser += DeleteUserHandler;
        }

        public override int Add(User user)
        {
            throw new InvalidOperationException();
        }

        public override void Delete(int id)
        {
            throw new InvalidOperationException();
        }

        private void AddUserHandler(object sender, AddEventArgs e)
        {
            users.Add((User)(e.User.Clone()));
        }

        private void DeleteUserHandler(object sender, DeleteEventArgs e)
        {
            users.Remove(users.Find(user => user.Id == e.Id));
        }
    }
}
