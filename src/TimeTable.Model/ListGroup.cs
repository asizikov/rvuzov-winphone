using System.Collections.Generic;

namespace TimeTable.Model
{
    public class ListGroup<T> : IEnumerable<T>
    {
        public ListGroup(string name, IEnumerable<T> items)
        {
            Title = name;
            Items = new List<T>(items);
        }

        public override bool Equals(object obj)
        {
            var that = obj as ListGroup<T>;

            return (that != null) && (this.Title.Equals(that.Title));
        }

        public string Title
        {
            get;
            set;
        }

        public IList<T> Items
        {
            get;
            set;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion
    }
}
