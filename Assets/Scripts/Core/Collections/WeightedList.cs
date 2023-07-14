using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace Scripts.Core.Collections
{
    /// <summary>
    /// The more weight an object has, the more chance it gets to spawn
    /// </summary>
    /// <typeparam name="T">Generic Typed</typeparam>
    public class WeightedList<T>
    {
        [Serializable]
        public struct Element {
            public T obj;
            public double weight;
            public Element(T obj, double weight = 0f) {
                this.obj = obj;
                this.weight = weight;
            }
        }

        private List<Element> _elements = new List<Element>();
        private double _sumWeight;
        private Random _rand = new Random();
        
        /// <summary>
        /// Returns a random element in the list that has the highest chance to spawn
        /// </summary>
        /// <returns>Element with highest chance</returns>
        public T GetRandomItem() {
            double randWeight = _rand.NextDouble() * _sumWeight;
            return _elements.FirstOrDefault(x => x.weight >= randWeight).obj;
        }
        
        public T GetRandomItem(T ignoredElement) {
            double randWeight = _rand.NextDouble() * _sumWeight;
            return _elements
                .FirstOrDefault(x => x.weight >= randWeight && !x.obj.Equals(ignoredElement)).obj;
        }
        
        /// <summary>
        /// Add element into the weighted list
        /// </summary>
        /// <param name="element">Object of Generic Typed</param>
        /// <param name="weight">Weight, or chance of it being returned</param>
        public void AddElement(T element, double weight = 0f) {
            _sumWeight += weight;
            _elements.Add(new Element(element, _sumWeight));   
        }
        /// <summary>
        /// Remove element from weighted list
        /// </summary>
        /// <param name="element">Element to Remove</param>
        public void Remove(T element) => _elements.Remove(_elements.FirstOrDefault(x => x.obj.Equals(element)));
        
        /// <summary>
        /// Clear all elements in list
        /// </summary>
        public void Clear() => _elements.Clear();
    }
}