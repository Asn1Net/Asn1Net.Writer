/*
 *  Copyright 2012-2016 The Asn1Net Project
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
/*
 *  Written for the Asn1Net project by:
 *  Peter Polacko <peter.polacko+asn1net@gmail.com>
 */

namespace Net.Asn1.Writer
{
    /// <summary>
    /// Base Asn.1 object class. Defines general properties that every Asn.1 object should have.
    /// </summary>
    public abstract class Asn1ObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1ObjectBase"/> class.
        /// </summary>
        /// <param name="asn1Class">Class of given Asn.1 object.</param>
        /// <param name="constructed">Constructed/Primitive bit of identifier octet.</param>
        /// <param name="asn1Tag">Tag number or type of Asn.1 object.</param>
        protected Asn1ObjectBase(Asn1Class asn1Class, bool constructed, int asn1Tag)
        {
            this.Asn1Class = asn1Class;
            this.Constructed = constructed;
            this.Asn1Tag = asn1Tag;
        }

        /// <summary>
        /// Gets class of given Asn.1 object.
        /// </summary>
        public Asn1Class Asn1Class { get; private set; }

        /// <summary>
        /// Gets a value indicating whether Constructed/Primitive bit of identifier octet is set.
        /// </summary>
        public bool Constructed { get; private set; }

        /// <summary>
        /// Gets tag number or type of Asn.1 object.
        /// </summary>
        public int Asn1Tag { get; private set; }

        /// <summary>
        /// Encodes Asn.1 object.
        /// </summary>
        /// <returns>Encoded Asn.1 object.</returns>
        public abstract byte[] Write();
    }
}
