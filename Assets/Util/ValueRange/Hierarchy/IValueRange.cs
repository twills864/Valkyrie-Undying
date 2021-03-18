using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Util
{
    /// <summary>
    /// Represents a range of values between a given start value
    /// and a given end value.
    ///
    /// It was elected to make this functionality an interface
    /// instead of an abstract base class in order to keep the
    /// struct properties continuous with the host object's memory.
    /// </summary>
    /// <typeparam name="T">The type of range to represent.</typeparam>
    public interface IValueRange<T> where T : struct
    {
        /// <summary>
        /// The start value of this range.
        /// </summary>
        T StartValue { get; set; }

        /// <summary>
        /// The end value of this range.
        /// </summary>
        T EndValue { get; set; }

        /// <summary>
        /// The difference between the end and start values of this range.
        /// </summary>
        T RangeDelta { get; }

        /// <summary>
        /// The value represented at a given ratio of this range.
        /// </summary>
        /// <param name="ratio">The given ratio.</param>
        /// <returns>The represented value.</returns>
        T ValueAtRatio(float ratio);
    }
}
