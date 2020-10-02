using System;
using System.Collections.Generic;
using System.Text;
using VS.Core.Messages;

namespace VS.Core.DomainObjects
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public Entity()
        {
            this.Id = Guid.NewGuid();
        }

        private List<Event> _events;
        public IReadOnlyCollection<Event> Events => _events.AsReadOnly();

        public void ClearEventList()
        {
            _events.Clear();
        }

        public void AddEvent(Event eventObj)
        {
            _events.Add(eventObj);
        }

        public void RemoveEvent(Event eventObj)
        {
            _events.Remove(eventObj);
        }

        #region Comparasion Methods
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public override int GetHashCode()
        {
            return GetType().GetHashCode() + this.Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id = {this.Id}]";
        }
        #endregion
    }
}
