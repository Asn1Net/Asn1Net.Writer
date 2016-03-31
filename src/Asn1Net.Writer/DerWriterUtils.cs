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
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Implementation of helper methods
    /// </summary>
    public static class DerWriterUtils
    {
        /// <summary>
        /// Parses subidentifiers of object identifier or relative identifier.
        /// </summary>
        /// <param name="oidParts">Split parts of object or relative identifier.</param>
        /// <param name="res">Result where to write encoded values.</param>
        internal static void ParseSubIdentifiers(int[] oidParts, List<byte> res)
        {
            if (oidParts == null)
            {
                throw new ArgumentNullException(nameof(oidParts));
            }

            if (res == null)
            {
                throw new ArgumentNullException(nameof(res));
            }

            for (var i = 0; i < oidParts.Length; i++)
            {
                var itemToWorkWith = oidParts[i];

                for (var j = 9; j >= 0; j--)
                {
                    var mod = itemToWorkWith / (int)Math.Pow(128, j);
                    if (mod != 0)
                    {
                        if (j != 0)
                        {
                            res.Add((byte)(mod | 0x80));
                        }
                        else
                        {
                            res.Add((byte)(mod & 0x7f));
                        }
                    }

                    itemToWorkWith = itemToWorkWith - (mod * (int)Math.Pow(128, j));
                }
            }
        }
    }
}
