﻿namespace Nessos.FsPickler

    open System
    open System.Collections
    open System.Collections.Generic
    open System.IO
    open System.Text

    open Nessos.FsPickler.RootObjectSerialization

    [<AbstractClass>]
    [<AutoSerializableAttribute(false)>]
    type StringPickler (formatP : IStringPickleFormatProvider, ?tyConv) =
        inherit BinaryPickler(formatP, ?tyConv = tyConv)

        let resolver = base.Resolver
        let reflectionCache = base.ReflectionCache

        /// <summary>Serialize value to the underlying stream.</summary>
        /// <param name="writer">target writer.</param>
        /// <param name="value">value to be serialized.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Serialize<'T>(writer : TextWriter, value : 'T, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : unit =

            let pickler = resolver.Resolve<'T> ()
            use formatter = initTextWriter formatP writer leaveOpen
            writeRootObject resolver reflectionCache formatter streamingContext pickler value

        /// <summary>Serialize value to the underlying stream using given pickler.</summary>
        /// <param name="pickler">pickler used for serialization.</param>
        /// <param name="writer">target writer.</param>
        /// <param name="value">value to be serialized.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Serialize<'T>(pickler : Pickler<'T>, writer : TextWriter, value : 'T, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : unit =

            use formatter = initTextWriter formatP writer leaveOpen
            writeRootObject resolver reflectionCache formatter streamingContext pickler value

        /// <summary>Serialize object of given type to the underlying stream.</summary>
        /// <param name="valueType">type of the given object.</param>
        /// <param name="writer">target writer.</param>
        /// <param name="value">value to be serialized.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="encoding">encoding passed to the binary writer.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Serialize(valueType : Type, writer : TextWriter, value : obj, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : unit =

            let pickler = resolver.Resolve valueType
            use formatter = initTextWriter formatP writer leaveOpen
            writeRootObjectUntyped resolver reflectionCache formatter streamingContext pickler value

        /// <summary>Serialize object to the underlying stream using given pickler.</summary>
        /// <param name="pickler">untyped pickler used for serialization.</param>
        /// <param name="writer">target writer.</param>
        /// <param name="value">value to be serialized.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Serialize(pickler : Pickler, writer : TextWriter, value : obj, 
                                            [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : unit =

            use formatter = initTextWriter formatP writer leaveOpen
            writeRootObjectUntyped resolver reflectionCache formatter streamingContext pickler value

        /// <summary>Deserialize value of given type from the underlying stream.</summary>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Deserialize<'T> (reader : TextReader, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : 'T =

            let pickler = resolver.Resolve<'T> ()
            use formatter = initTextReader formatP reader leaveOpen
            readRootObject resolver reflectionCache formatter streamingContext pickler

        /// <summary>Deserialize value of given type from the underlying stream, using given pickler.</summary>
        /// <param name="pickler">pickler used for serialization.</param>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Deserialize<'T> (pickler : Pickler<'T>, reader : TextReader, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : 'T =

            use formatter = initTextReader formatP reader leaveOpen
            readRootObject resolver reflectionCache formatter streamingContext pickler

        /// <summary>Deserialize object of given type from the underlying stream.</summary>
        /// <param name="valueType">anticipated value type.</param>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Deserialize (valueType : Type, reader : TextReader, 
                                    [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : obj =

            let pickler = resolver.Resolve valueType
            use formatter = initTextReader formatP reader leaveOpen
            readRootObjectUntyped resolver reflectionCache formatter streamingContext pickler

        /// <summary>Deserialize object from the underlying stream using given pickler.</summary>
        /// <param name="pickler">untyped pickler used for deserialization.</param>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <return>number of elements written to the stream.</return>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        member __.Deserialize (pickler : Pickler, reader : TextReader, 
                                    [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : obj =

            use formatter = initTextReader formatP reader leaveOpen
            readRootObjectUntyped resolver reflectionCache formatter streamingContext pickler

        /// <summary>Serialize a sequence of objects to the underlying stream.</summary>
        /// <param name="writer">target writer.</param>
        /// <param name="sequence">input sequence.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        /// <return>number of elements written to the stream.</return>
        member __.SerializeSequence<'T>(writer : TextWriter, sequence:seq<'T>, 
                                            [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : int =

            let pickler = resolver.Resolve<'T> ()
            use formatter = initTextWriter formatP writer leaveOpen
            writeTopLevelSequence resolver reflectionCache formatter streamingContext pickler sequence

        /// <summary>Serialize a sequence of objects to the underlying stream.</summary>
        /// <param name="elementType">element type used in sequence.</param>
        /// <param name="writer">target writer.</param>
        /// <param name="sequence">input sequence.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        /// <return>number of elements written to the stream.</return>
        member __.SerializeSequence(elementType : Type, writer : TextWriter, sequence : IEnumerable, 
                                        [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : int =

            let pickler = resolver.Resolve elementType
            use formatter = initTextWriter formatP writer leaveOpen
            writeTopLevelSequenceUntyped resolver reflectionCache formatter streamingContext pickler sequence

        /// <summary>Lazily deserialize a sequence of objects from the underlying stream.</summary>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        /// <returns>An IEnumerator that lazily consumes elements from the stream.</returns>
        member __.DeserializeSequence<'T>(reader : TextReader, 
                                            [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : seq<'T> =

            let pickler = resolver.Resolve<'T> ()
            let formatter = initTextReader formatP reader leaveOpen
            readTopLevelSequence resolver reflectionCache formatter streamingContext pickler

        /// <summary>Lazily deserialize a sequence of objects from the underlying stream.</summary>
        /// <param name="elementType">element type used in sequence.</param>
        /// <param name="reader">source reader.</param>
        /// <param name="streamingContext">streaming context.</param>
        /// <param name="leaveOpen">Leave underlying stream open when finished. Defaults to false.</param>
        /// <returns>An IEnumerator that lazily consumes elements from the stream.</returns>
        member __.DeserializeSequence(elementType : Type, reader : TextReader, 
                                            [<O;D(null)>]?streamingContext, [<O;D(null)>]?leaveOpen) : IEnumerable =

            let pickler = resolver.Resolve elementType
            let formatter = initTextReader formatP reader leaveOpen
            readTopLevelSequenceUntyped resolver reflectionCache formatter streamingContext pickler


        /// <summary>
        ///     Pickles given value to byte array.
        /// </summary>
        /// <param name="pickler">Pickler to use.</param>
        /// <param name="value">Value to pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.PickleToString (pickler : Pickler<'T>, value : 'T, [<O;D(null)>]?streamingContext) : string =
            pickleString (fun m v -> f.Serialize(pickler, m, v, ?streamingContext = streamingContext)) value

        /// <summary>
        ///     Pickles given value to byte array.
        /// </summary>
        /// <param name="value">Value to pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.PickleToString (value : 'T, [<O;D(null)>]?streamingContext) : string =
            pickleString (fun m v -> f.Serialize(m, v, ?streamingContext = streamingContext)) value

        /// <summary>
        ///     Pickles given value to byte array.
        /// </summary>
        /// <param name="pickler">pickler to use.</param>
        /// <param name="value">value to pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.PickleToString (pickler: Pickler, value : obj, [<O;D(null)>]?streamingContext) : string =
            pickleString (fun m v -> f.Serialize(pickler, m, v, ?streamingContext = streamingContext)) value

        /// <summary>
        ///     Pickles given value to byte array.
        /// </summary>
        /// <param name="valueType">type of pickled value.</param>
        /// <param name="value">value to pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.PickleToString (valueType : Type, value : obj, [<O;D(null)>]?streamingContext) : string =
            pickleString (fun m v -> f.Serialize(valueType, m, v, ?streamingContext = streamingContext)) value

        /// <summary>
        ///     Unpickles value using given pickler.
        /// </summary>
        /// <param name="pickler">Pickler to use.</param>
        /// <param name="pickle">Pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.UnPickleOfString (pickler : Pickler<'T>, pickle : string, [<O;D(null)>]?streamingContext) : 'T =
            unpickleString (fun m -> f.Deserialize(pickler, m, ?streamingContext = streamingContext)) pickle

        /// <summary>
        ///     Unpickle value to given type.
        /// </summary>
        /// <param name="pickle">Pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.UnPickleOfString<'T> (pickle : string, [<O;D(null)>]?streamingContext) : 'T =
            unpickleString (fun m -> f.Deserialize<'T>(m, ?streamingContext = streamingContext)) pickle

        /// <summary>
        ///     Unpickle value to given type.
        /// </summary>
        /// <param name="valueType">type of pickled value.</param>
        /// <param name="pickle">Pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.UnPickleOfString (valueType : Type, pickle : string, [<O;D(null)>]?streamingContext) : obj =
            unpickleString (fun m -> f.Deserialize(valueType, m, ?streamingContext = streamingContext)) pickle

        /// <summary>
        ///     Unpickle value to given type.
        /// </summary>
        /// <param name="pickler">pickler to use.</param>
        /// <param name="pickle">Pickle.</param>
        /// <param name="streamingContext">streaming context.</param>
        member f.UnPickleOfString (pickler : Pickler, pickle : string, [<O;D(null)>]?streamingContext) : obj =
            unpickleString (fun m -> f.Deserialize(pickler, m, ?streamingContext = streamingContext)) pickle


    type XmlPickler =
        inherit StringPickler
        
        val private format : XmlPickleFormatProvider

        new (?tyConv, ?indent) =
            let xml = new XmlPickleFormatProvider(defaultArg indent false)
            { 
                inherit StringPickler(xml, ?tyConv = tyConv)
                format = xml    
            }

        member x.Indent
            with get () = x.format.Indent
            and set b = x.format.Indent <- b

    type JsonPickler =
        inherit StringPickler
        
        val private format : JsonPickleFormatProvider

        new (?tyConv, ?indent, ?omitHeader) =
            let indent = defaultArg indent false
            let omitHeader = defaultArg omitHeader false
            let json = new JsonPickleFormatProvider(indent, omitHeader)
            { 
                inherit StringPickler(json, ?tyConv = tyConv)
                format = json    
            }

        member x.Indent
            with get () = x.format.Indent
            and set b = x.format.Indent <- b

        member x.OmitHeader
            with get () = x.format.OmitHeader
            and set b = x.format.OmitHeader <- b