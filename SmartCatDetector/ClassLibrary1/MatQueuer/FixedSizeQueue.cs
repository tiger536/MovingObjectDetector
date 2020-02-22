using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary1.Implementations
{
    public class FixedSizeQueue<T> : Queue<T>
        where T: IDisposable
    {
        private readonly int _maxSize;
        private object lockObkect = new object();
        public FixedSizeQueue(int maxSize)
        {
            _maxSize = maxSize;
        }
      

        public new void Enqueue(T obj)
        {
            if (base.Count < _maxSize)
            {
                lock (lockObkect)
                {
                    if (base.Count < _maxSize)
                    {
                        base.Enqueue(obj);
                    }
                }
            }
            else
            {
                lock(lockObkect)
                {
                    while (base.Count >= _maxSize) { var a = Dequeue(); a.Dispose(); }
                    base.Enqueue(obj);
                }
            }
        }

        public List<T> DequeueAll()
        {
            var returnList = new List<T>();
            lock (lockObkect)
            {
                while (base.Count >0) { returnList.Add( base.Dequeue()); }
            }
            return returnList;
        }
    }
}
