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

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Net.Asn1.Writer.Tests
{
    public partial class DerWriterTests
    {
        [TestCase("30 0D 06 09 2A 86 48 86 F7 0D 01 01 0B 05 00 ", "1.2.840.113549.1.1.11")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteSequence(string correctResultHex, string oid)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);
            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, 
                    new System.Collections.Generic.List<Asn1ObjectBase>() { new Asn1ObjectIdentifier(oid), new Asn1Null() } );
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("30 00")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteEmptySequence(string correctResultHex)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);

            using (var ms = new MemoryStream())
            {
                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, new System.Collections.Generic.List<Asn1ObjectBase>());
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("30 82 01 22 30 0D 06 09 2A 86 48 86 F7 0D 01 01 01 05 00 03 82 01 0F 00 30 82 01 0A 02 82 01 01 00 B1 D4 DC 3C AF FD F3 4E ED C1 67 AD E6 CB 22 E8 B7 E2 AB 28 F2 F7 DC 62 70 08 D1 0C AF D6 16 6A 21 B0 36 4B 17 0D 36 63 04 AE BF EA 20 51 95 65 66 F2 BF B9 4D A4 0C 29 EB F5 15 B1 E8 35 B3 70 10 94 D5 1B 59 B4 26 0F D6 83 57 59 9D E1 7C 09 DD E0 13 CA 4D 6F 43 9B CD CF 87 3A 15 A7 85 DD 66 83 ED 93 0C FE 2B 6D 38 1C 79 88 90 CF AD 58 18 2D 51 D1 C2 A3 F2 47 8C 6F 38 09 B9 B8 EF 4C 93 0B CB 83 94 87 EA E0 A3 B5 D9 7B 9B 6B 0F 43 F9 CA EE 80 0D 28 A7 76 F1 25 F4 C1 35 3C F6 74 AD DE 6A 33 82 7B DC FD 4B 76 A7 C2 EE F2 6A BF A9 24 A6 5F E7 2E 7C 0E DB C3 74 73 FA 7E C6 D8 CF 60 EB 36 56 21 B6 C1 8A B8 24 82 4D 78 24 BA E9 1D A1 8A A7 87 BE 66 25 69 BF BE 3B 72 6E 4F E0 E4 85 25 08 B1 91 89 B8 D6 74 65 76 9B 2C 4F 62 1F A1 FA 3A BE 9C 24 BF 9F CA B0 C5 C0 67 8D 02 03 01 00 01")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteComplexSequence(string correctResultHex)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);

            using (var ms = new MemoryStream())
            {
                var content = new System.Collections.Generic.List<Asn1ObjectBase>();
                content.Add(new Asn1Sequence(Asn1Class.Universal, new System.Collections.Generic.List<Asn1ObjectBase>()
                {
                    new Asn1ObjectIdentifier("1.2.840.113549.1.1.1"),
                    new Asn1Null(),
                }));

                content.Add(new Asn1BitString(Convert.FromBase64String("MIIBCgKCAQEAsdTcPK/9807twWet5ssi6Lfiqyjy99xicAjRDK/WFmohsDZLFw02YwSuv+ogUZVlZvK/uU2kDCnr9RWx6DWzcBCU1RtZtCYP1oNXWZ3hfAnd4BPKTW9Dm83PhzoVp4XdZoPtkwz+K204HHmIkM+tWBgtUdHCo/JHjG84Cbm470yTC8uDlIfq4KO12Xubaw9D+crugA0op3bxJfTBNTz2dK3eajOCe9z9S3anwu7yar+pJKZf5y58DtvDdHP6fsbYz2DrNlYhtsGKuCSCTXgkuukdoYqnh75mJWm/vjtybk/g5IUlCLGRibjWdGV2myxPYh+h+jq+nCS/n8qwxcBnjQIDAQAB"), 0));

                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, content);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }

        [TestCase("30 7A 30 24 06 08 2B 06 01 05 05 07 30 01 86 18 68 74 74 70 3A 2F 2F 6F 63 73 70 2E 64 69 67 69 63 65 72 74 2E 63 6F 6D 30 52 06 08 2B 06 01 05 05 07 30 02 86 46 68 74 74 70 3A 2F 2F 63 61 63 65 72 74 73 2E 64 69 67 69 63 65 72 74 2E 63 6F 6D 2F 44 69 67 69 43 65 72 74 53 48 41 32 45 78 74 65 6E 64 65 64 56 61 6C 69 64 61 74 69 6F 6E 53 65 72 76 65 72 43 41 2E 63 72 74")]
        [Category("SEQUENCE")]
        [Test]
        public void WriteComplexSequence2(string correctResultHex)
        {
            var encoded = Helpers.GetExampleBytes(correctResultHex);

            using (var ms = new MemoryStream())
            {
                var content = new System.Collections.Generic.List<Asn1ObjectBase>();
                content.Add(new Asn1Sequence(Asn1Class.Universal, new System.Collections.Generic.List<Asn1ObjectBase>()
                {
                    new Asn1ObjectIdentifier("1.3.6.1.5.5.7.48.1"),
                    new Asn1ContextSpecific(false, 6, Convert.FromBase64String("aHR0cDovL29jc3AuZGlnaWNlcnQuY29t"))
                }));

                content.Add(new Asn1Sequence(Asn1Class.Universal, new System.Collections.Generic.List<Asn1ObjectBase>()
                {
                    new Asn1ObjectIdentifier("1.3.6.1.5.5.7.48.2"),
                    new Asn1ContextSpecific(false, 6, Convert.FromBase64String("aHR0cDovL2NhY2VydHMuZGlnaWNlcnQuY29tL0RpZ2lDZXJ0U0hBMkV4dGVuZGVkVmFsaWRhdGlvblNlcnZlckNBLmNydA=="))
                }));

                var asn1Obj = new Asn1Sequence(Asn1Class.Universal, content);
                new DerWriter(ms).Write(asn1Obj);

                var res = Enumerable.SequenceEqual(encoded, ms.ToArray());
                Assert.IsTrue(res);
            }
        }
    }
}
