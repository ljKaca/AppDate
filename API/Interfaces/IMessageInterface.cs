using API.DTO;
using API.Entities;
using API.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
   public interface IMessageInterface
    {
        public void AddGroup(Group group);
        public Task<Connection> GetConnection(string connectionId);
        public Task<Group> GetMessageGroup(string groupName);
        public void RemoveConnection(Connection connection);

        public Task<Group> GetGroupForConnection(string connectionId);
        void AddMessage(Message message);
        void DeleteMessage(Message message);

        Task<Message> GetMessage(int id);
        Task<PageList<MessageDTO>> GetMessageForUser(MessageParams messageParams);

        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
        
    }
}
