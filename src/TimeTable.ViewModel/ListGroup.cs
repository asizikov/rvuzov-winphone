using System.Collections.Generic;

namespace TimeTable.ViewModel
{
    public class ListGroup<T> : List<T>
    {
        public ListGroup(string name, IEnumerable<T> items) :base(items)
        {
            Title = name;
            
        }

        public string Title { get; set; }
    }
}
