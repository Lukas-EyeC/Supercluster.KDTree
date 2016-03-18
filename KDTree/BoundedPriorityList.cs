﻿// <copyright file="BoundedPriorityList.cs" company="Eric Regina">
// Copyright (c) Eric Regina. All rights reserved.
// </copyright>

namespace Supercluster.KDTree
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// A list of limited length that remains sorted BY TPriority/>.
    /// Useful for nearest neighbor searches.
    /// Insert is O(log n). Retreval is O(1)
    /// </summary>
    /// <typeparam name="TElement">The type of element the list maintains.</typeparam>
    /// <typeparam name="TPriority">The type tht the elements are prioritized by.</typeparam>
    public class BoundedPriorityList<TElement, TPriority> : IEnumerable<TElement>
        where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// The list holding the actual elements
        /// </summary>
        private readonly List<TElement> elementList;

        /// <summary>
        /// The list of priorities for each element.
        /// There is a one-to-one correspondence between the
        /// priority list ad the element list.
        /// </summary>
        private readonly List<TPriority> priorityList;

        /// <summary>
        /// Gets the element with the largest priority.
        /// </summary>
        public TElement MaxElement => this.elementList[this.elementList.Count - 1];

        /// <summary>
        /// Gets the largest priority.
        /// </summary>
        public TPriority MaxPriority => this.priorityList[this.priorityList.Count - 1];

        /// <summary>
        /// Gets the element with the lowest priority.
        /// </summary>
        public TElement MinElement => this.elementList[0];

        /// <summary>
        /// Gets the smallest priority.
        /// </summary>
        public TPriority MinPriority => this.priorityList[0];

        /// <summary>
        /// Gets the maximum allows capacity for the <see cref="BoundedPriorityList{TElement,TPriority}"/>
        /// </summary>
        public int Capacity { get; }

        /// <summary>
        /// Returns true if the list is at maximum capacity.
        /// </summary>
        public bool IsFull => this.Count == this.Capacity;

        /// <summary>
        /// Returns the count of items currently in the list.
        /// </summary>
        public int Count => this.priorityList.Count;

        /// <summary>
        /// Indexer for the internal element array.
        /// </summary>
        /// <param name="index">The index in the array.</param>
        /// <returns>The element at the specified index.</returns>
        public TElement this[int index] => this.elementList[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedPriorityList{TElement, TPriority}"/> class.
        /// </summary>
        /// <param name="capacity">The maximum capacity of the list.</param>
        public BoundedPriorityList(int capacity)
        {
            this.Capacity = capacity;
            this.priorityList = new List<TPriority>(capacity);
            this.elementList = new List<TElement>(capacity);
        }

        /// <summary>
        /// Attempts to add the provided  <paramref name="item"/>. If the list
        /// is currently at maximum capacity and the elements priority is greater
        /// than or equal to the hiest priority, the <paramref name = "item"/> is not inserted. If the
        /// <paramref name = "item"/> is eligable for insertion, the upon insertion the <paramref name = "item"/> that previously
        /// had the largest priority is removed from the list.
        /// This is an O(log n) operation.
        /// </summary>
        /// <param name="item">The item to be inserted</param>
        /// <param name="priority">The priority of th given item.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(TElement item, TPriority priority)
        {
            if (this.Count >= this.Capacity)
            {
                if (this.priorityList[this.priorityList.Count - 1].CompareTo(priority) < 0)
                {
                    return;
                }

                var index = this.priorityList.BinarySearch(priority);
                index = index >= 0 ? index : ~index;

                this.priorityList.Insert(index, priority);
                this.elementList.Insert(index, item);

                this.priorityList.RemoveAt(this.priorityList.Count - 1);
                this.elementList.RemoveAt(this.elementList.Count - 1);
            }
            else
            {
                var index = this.priorityList.BinarySearch(priority);
                index = index >= 0 ? index : ~index;

                this.priorityList.Insert(index, priority);
                this.elementList.Insert(index, item);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        public IEnumerator<TElement> GetEnumerator()
        {
            return this.elementList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}