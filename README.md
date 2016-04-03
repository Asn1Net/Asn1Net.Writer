# Asn1Net.Writer
[![Build status](https://ci.appveyor.com/api/projects/status/6qixi5gio2kimlqf?svg=true)](https://ci.appveyor.com/project/PeterPolacko/asn1net-writer)

Managed library for writing ASN.1 objects.

## Getting started ##
Asn1Net.Writer can easily encode any ASN.1 object:

```csharp
public void WriteUtf8String()
{
	// prepare a stream to write objects to
	using (var ms = new MemoryStream())
	{
		// prepare what to write
		var asn1Obj = new Asn1Utf8String("Hello World of ASN.1");
		new DerWriter(ms).Write(asn1Obj);
		
		// that's it. Now use the stream as you like.
	}
}
```

More examples can be found in the [Tests project](https://github.com/Asn1Net/Asn1Net.Writer/tree/master/src/Tests).

## Standing on the shoulders of giants ##
Asn1Net.Writer is developed with help of the following tools, libraries and applications:

- [Microsoft Visual Studio Community 2015](https://www.visualstudio.com/en-us/products/visual-studio-community-vs.aspx)
- [Asn1Editor](http://www.codeproject.com/Articles/4910/ASN-Editor)
- [License Header Manager](https://visualstudiogallery.msdn.microsoft.com/5647a099-77c9-4a49-91c3-94001828e99e)
- [NUnit](http://www.nunit.org/)
