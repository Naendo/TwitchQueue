using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TalkMaster.Shared;

namespace TalkMaster.Domain.Service
{
    public class QueueService
    {
        public Queue<string> Queue = new Queue<string>();

        private List<string> _lastQueue;

        public JoinResponse Join(string userName)
        {
            if (Queue.Contains(userName)) return new JoinResponse { Index = GetIndexOf(userName), IsAdded = false };
            Queue.Enqueue(userName);
            return new JoinResponse { Index = GetIndexOf(userName), IsAdded = true };
        }
        public UpdateResponse Leave(string userName)
        {
            if (!Queue.Contains(userName)) return new UpdateResponse() { IsSuccessfull = false, UpdatedContent = null, UpdateCode = 'l'};

            Queue = new Queue<string>(Queue.Where(user => user != userName));

            return new UpdateResponse() { IsSuccessfull = true, UpdatedContent = Queue.ToArray(), UpdateCode = 'l' };
        }
        public CollectionResponse QWho(int count)
        {
            if (count == 0) return new CollectionResponse() { Count = Queue.Count, Names = Queue.ToArray() };
            else return new CollectionResponse { Count = count, Names = Queue.ToList().GetRange(0, count).ToArray() };
        }
        public CollectionResponse QNext(int count)
        {
            if (count == 0) throw new Exception("There are no people in Queue!");
            if (count > Queue.Count) throw new Exception("There are not as many people in Queue");

            var list = new List<string>();

            for (var i = 0; i < count; i++)
            {
                list.Add(Queue.Dequeue());
            }
            _lastQueue = new List<string>(list);

            return new CollectionResponse() { Count = list.Count, Names = list.ToArray() };
        }
        public CollectionResponse LastQ()
        {
            if (_lastQueue == null) throw new Exception("The Command !qnext wasn´t executed before, therefor a last queue doesnt exist!");

            return new CollectionResponse() { Count = _lastQueue.Count, Names = _lastQueue.ToArray() };
        }
        public UpdateResponse Promote(string userName)
        {
            if (Queue.Peek() == userName.ToLower()) return new UpdateResponse() { IsSuccessfull = false, UpdatedContent = null };

            var queuelist = Queue.ToList();

            var lowercase = userName.ToLower();
            queuelist.Remove(lowercase);

            var newlist = new List<string>();

            newlist.Add(userName.ToLower());
            newlist.AddRange(queuelist);

            Queue = new Queue<string>(newlist);

            return new UpdateResponse() { IsSuccessfull = true, UpdatedContent = QWho(0), UpdateCode = 'p' };

        }
        public PositionResponse Position(string userName)
        {
            if (!Queue.Contains(userName)) return new PositionResponse { Index = 0, HasPosition = false };

            var index = Queue.ToList().IndexOf(userName) + 1;

            return new PositionResponse { Index = GetIndexOf(userName), HasPosition = true };
        }
        public UpdateResponse Clear()
        {
            Queue = new Queue<string>();
            return new UpdateResponse() { IsSuccessfull = true, UpdatedContent = Queue.ToArray(), UpdateCode = 'c' };
        }
        private int GetIndexOf(string userName)
        {
            return Queue.ToArray().ToList().IndexOf(userName) + 1;
        }
    }
}
