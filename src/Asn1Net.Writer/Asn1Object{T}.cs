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
    /// Generalized ASN.1 object. Adds generalized property to hold different types of content fore each type of ASN.1 object.
    /// </summary>
    /// <typeparam name="T">Type of content of ASN.1 object.</typeparam>
    public abstract class Asn1Object<T> : Asn1ObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Asn1Object{T}"/> class.
        /// </summary>
        /// <param name="asn1Class">Class of given Asn.1 object.</param>
        /// <param name="constructed">Constructed/Primitive bit of identifier octet.</param>
        /// <param name="asn1Tag">Tag number or type of Asn.1 object.</param>
        /// <param name="content">Content to be encoded.</param>
        protected Asn1Object(Asn1Class asn1Class, bool constructed, int asn1Tag, T content)
            : base(asn1Class, constructed, asn1Tag)
        {
            this.Content = content;
        }

        /// <summary>
        /// Gets Content of ASN.1 object to be encoded.
        /// </summary>
        public T Content { get; private set; }
    }
}
