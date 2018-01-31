using System;
using System.Collections.Generic;

namespace TestsRefactoring.Tests.Builders
{
    public class Builder<T> where T : new()  
    {
        IList<Action<T>> actions = new List<Action<T>>();
        
        public T Build ()    
        {
            var built = new T ();
            foreach (var action in actions) 
            {
                action (built);
            }
            return built;
        }

        public Builder<T> With(Action<T> with)
        {
            actions.Add (with);
            return this;    
        }

        public Builder<T> But()
        {
            return new Builder<T> {actions = new List<Action<T>>(actions)};
        }
    }
}